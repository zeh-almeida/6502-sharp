using Cpu.Execution;
using Cpu.Instructions.JumpToSubroutines;
using Cpu.States;
using Moq;
using System;
using System.Collections.Generic;
using Test.Unit.Cpu.Utils;
using Xunit;

namespace Test.Unit.Cpu.Execution
{
    public sealed record MachineTest
    {
        #region Constants
        private const int StreamByte = 0x20;
        #endregion

        #region Properties
        private Mock<ICpuState> StateMock { get; }

        private Mock<IDecoder> DecoderMock { get; }

        private DecodedInstruction Decoded { get; }

        private Machine Subject { get; }
        #endregion

        #region Constructors
        public MachineTest()
        {
            this.StateMock = TestUtils.GenerateStateMock();
            this.DecoderMock = new Mock<IDecoder>();

            var instruction = new JumpToSubroutine();
            var opcodeInfo = instruction.GatherInformation(StreamByte);

            this.Decoded = new DecodedInstruction(opcodeInfo, 65535);
            this.Subject = new Machine(this.StateMock.Object, this.DecoderMock.Object);
        }
        #endregion

        [Fact]
        public void Data_Load_Successful()
        {
            var data = new byte[10];

            this.Subject.Load(data);

            this.StateMock.Verify(mock => mock.Load(data), Times.Exactly(1));
        }

        [Fact]
        public void Data_Save_Successful()
        {
            var expected = new byte[10];

            _ = this.StateMock
                .Setup(mock => mock.Save())
                .Returns(expected);

            var result = this.Subject.Save();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void ProgramFinished_Cycle_IsFalse()
        {
            _ = this.StateMock
                .Setup(mock => mock.Registers.ProgramCounter)
                .Returns(ushort.MaxValue);

            var result = this.Subject.Cycle();

            Assert.False(result);
        }

        [Fact]
        public void DecodeStream_Cycle_Successful()
        {
            _ = this.StateMock
                .SetupSequence(mock => mock.Registers.ProgramCounter)
                .Returns(ushort.MaxValue - 1)
                .Returns(ushort.MaxValue - 1)
                .Returns(ushort.MaxValue);

            _ = this.DecoderMock
                .Setup(mock => mock.Decode(It.IsAny<ICpuState>()))
                .Returns(this.Decoded);

            var result = this.Subject.Cycle();

            Assert.True(result);
        }

        [Fact]
        public void AdvanceProgramCount_Cycle_Successful()
        {
            const ushort initialProgramCounter = 65532;
            const ushort finalProgramCounter = 65535;

            _ = this.StateMock
                .SetupSequence(mock => mock.Registers.ProgramCounter)
                .Returns(initialProgramCounter)
                .Returns(initialProgramCounter)
                .Returns(finalProgramCounter);

            _ = this.StateMock
                .Setup(mock => mock.ExecutingOpcode)
                .Returns(StreamByte);

            _ = this.DecoderMock
                .Setup(mock => mock.Decode(It.IsAny<ICpuState>()))
                .Returns(this.Decoded);

            var result = this.Subject.Cycle();

            Assert.True(result);

            this.StateMock.Verify(mock => mock.Registers.ProgramCounter, Times.Exactly(2));
            this.StateMock.VerifySet(mock => mock.Registers.ProgramCounter = finalProgramCounter, Times.Exactly(2));
        }

        [Fact]
        public void RemainingCycles_Count_Successful()
        {
            const int cycleCount = 6;
            const ushort initialProgramCounter = 65532;
            const ushort finalProgramCounter = 65535;

            _ = this.StateMock
                .SetupSequence(mock => mock.Registers.ProgramCounter)
                .Returns(initialProgramCounter)
                .Returns(initialProgramCounter)
                .Returns(initialProgramCounter)
                .Returns(finalProgramCounter);

            _ = this.StateMock
                .Setup(mock => mock.ExecutingOpcode)
                .Returns(StreamByte);

            _ = this.DecoderMock
                .Setup(mock => mock.Decode(It.IsAny<ICpuState>()))
                .Returns(this.Decoded);

            var results = new List<bool>();
            var result = false;

            do
            {
                result = this.Subject.Cycle();
                results.Add(result);

            } while (result);

            Assert.Equal(cycleCount, results.Count);
            Assert.Equal(0, this.Subject.CyclesLeft);
        }

        [Fact]
        public void HardwareInterrupt_Successful()
        {
            const int cycleCount = 12;

            const byte flagState = 0b_1111_0110;

            const ushort initialProgramCounter = 65532;
            const ushort pushedProgramCounter = 65534;
            const ushort finalProgramCounter = 65535;

            _ = this.StateMock
                .SetupSequence(mock => mock.Registers.ProgramCounter)
                .Returns(initialProgramCounter)
                .Returns(initialProgramCounter)
                .Returns(initialProgramCounter)
                .Returns(finalProgramCounter);

            _ = this.StateMock
                .Setup(mock => mock.ExecutingOpcode)
                .Returns(StreamByte);

            _ = this.StateMock
                .Setup(mock => mock.Flags.Save())
                .Returns(flagState);

            _ = this.DecoderMock
                .Setup(mock => mock.Decode(It.IsAny<ICpuState>()))
                .Returns(this.Decoded);

            this.Subject.IsHardwareInterrupt = true;

            var results = new List<bool>();
            var result = false;

            do
            {
                result = this.Subject.Cycle();
                results.Add(result);

            } while (result);

            this.StateMock.Verify(state => state.Stack.Push(flagState), Times.Once());
            this.StateMock.Verify(state => state.Stack.Push16(pushedProgramCounter), Times.Once());

            Assert.Equal(cycleCount, results.Count);
            Assert.False(this.Subject.IsHardwareInterrupt);
        }

        [Fact]
        public void DecodeStream_Action_Executes()
        {
            _ = this.StateMock
                .SetupSequence(mock => mock.Registers.ProgramCounter)
                .Returns(ushort.MaxValue - 1)
                .Returns(ushort.MaxValue - 1)
                .Returns(ushort.MaxValue);

            _ = this.DecoderMock
                .Setup(mock => mock.Decode(It.IsAny<ICpuState>()))
                .Returns(this.Decoded);

            var executionCount = 0;
            void action(ICpuState _)
            {
                executionCount++;
            }

            var result = this.Subject.Cycle(action);

            Assert.True(result);
            Assert.Equal(1, executionCount);
        }

        [Fact]
        public void DecodeStream_Throws_Fails()
        {
            _ = this.StateMock
                .Setup(mock => mock.Registers.ProgramCounter)
                .Returns(1);

            _ = this.DecoderMock
                .Setup(mock => mock.Decode(It.IsAny<ICpuState>()))
                .Throws<Exception>();

            var result = this.Subject.Cycle();

            Assert.False(result);
        }

        [Fact]
        public void ExecuteDecoded_Throws_Fails()
        {
            const ushort initialProgramCounter = 65532;
            const ushort finalProgramCounter = 65535;

            _ = this.StateMock
                .SetupSequence(mock => mock.Registers.ProgramCounter)
                .Returns(initialProgramCounter)
                .Returns(initialProgramCounter)
                .Returns(finalProgramCounter);

            _ = this.StateMock
                .SetupSet(mock => mock.ExecutingOpcode = StreamByte)
                .Throws<Exception>();

            _ = this.DecoderMock
                .Setup(mock => mock.Decode(It.IsAny<ICpuState>()))
                .Returns(this.Decoded);

            var result = this.Subject.Cycle();

            Assert.False(result);
        }
    }
}
