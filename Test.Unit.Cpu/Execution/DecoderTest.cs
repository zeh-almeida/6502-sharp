using Cpu.Execution;
using Cpu.Instructions;
using Cpu.Instructions.Branches;
using Cpu.Instructions.Exceptions;
using Cpu.Instructions.JumpToSubroutines;
using Cpu.Instructions.StatusChanges;
using Cpu.States;
using Moq;
using Test.Unit.Cpu.Utils;
using Xunit;

namespace Test.Unit.Cpu.Execution
{
    public sealed record DecoderTest
    {
        #region Properties
        private Mock<ICpuState> StateMock { get; }
        #endregion

        #region Constructors
        public DecoderTest()
        {
            this.StateMock = TestUtils.GenerateStateMock();
        }
        #endregion

        [Fact]
        public void InstructionNotFound_Decode_Fails()
        {
            const ushort pcAddress = 1;
            const int streamByte = 0x38;

            _ = this.StateMock
                .Setup(mock => mock.Registers.ProgramCounter)
                .Returns(pcAddress);

            _ = this.StateMock
                .Setup(mock => mock.Memory.ReadAbsolute(pcAddress))
                .Returns(streamByte);

            var subject = new Decoder(Array.Empty<IInstruction>());
            _ = Assert.Throws<UnknownOpcodeException>(() => subject.Decode(this.StateMock.Object));
        }

        [Fact]
        public void InstructionNoParam_Decode_Successful()
        {
            const ushort pcAddress = 1;
            const int streamByte = 0x38;

            var instruction = new SetCarryFlag();
            var opcodeInfo = instruction.GatherInformation(streamByte);

            _ = this.StateMock
                .Setup(mock => mock.Registers.ProgramCounter)
                .Returns(pcAddress);

            _ = this.StateMock
                .Setup(mock => mock.Memory.ReadAbsolute(pcAddress))
                .Returns(streamByte);

            var subject = new Decoder(new IInstruction[] { instruction });
            var result = subject.Decode(this.StateMock.Object);

            Assert.NotNull(result);

            Assert.Equal(instruction, result.Instruction);
            Assert.Equal(opcodeInfo.Cycles, result.Cycles);
            Assert.Equal(0, result.ValueParameter);
        }

        [Fact]
        public void InstructionWithTwoParams_Decode_Successful()
        {
            const ushort pcAddress = 1;
            const ushort pcParam1Address = 2;
            const ushort pcParam2Address = 3;
            const int streamByte = 0x20;

            const int firstParamByte = 0b_0000_1111;
            const int secondParamByte = 0b_1111_0000;
            const ushort paramValue = 0b_1111_0000_0000_1111;

            var instruction = new JumpToSubroutine();
            var opcodeInfo = instruction.GatherInformation(streamByte);

            _ = this.StateMock
                .Setup(mock => mock.Registers.ProgramCounter)
                .Returns(pcAddress);

            _ = this.StateMock
                .Setup(mock => mock.Memory.ReadAbsolute(pcAddress))
                .Returns(streamByte);

            _ = this.StateMock
                .Setup(mock => mock.Memory.ReadAbsolute(pcParam1Address))
                .Returns(firstParamByte);

            _ = this.StateMock
                .Setup(mock => mock.Memory.ReadAbsolute(pcParam2Address))
                .Returns(secondParamByte);

            var subject = new Decoder(new IInstruction[] { instruction });
            var result = subject.Decode(this.StateMock.Object);

            Assert.NotNull(result);

            Assert.Equal(instruction, result.Instruction);
            Assert.Equal(opcodeInfo.Cycles, result.Cycles);
            Assert.Equal(paramValue, result.ValueParameter);
        }

        [Fact]
        public void InstructionWithOneParam_Decode_Successful()
        {
            const ushort pcAddress = 1;
            const ushort pcParamAddress = 2;
            const int streamByte = 0xB0;
            const int paramByte = 0b_0000_1111;

            var instruction = new BranchCarrySet();
            var opcodeInfo = instruction.GatherInformation(streamByte);

            _ = this.StateMock
                .Setup(mock => mock.Registers.ProgramCounter)
                .Returns(pcAddress);

            _ = this.StateMock
                .Setup(mock => mock.Memory.ReadAbsolute(pcAddress))
                .Returns(streamByte);

            _ = this.StateMock
                .Setup(mock => mock.Memory.ReadAbsolute(pcParamAddress))
                .Returns(paramByte);

            var subject = new Decoder(new IInstruction[] { instruction });
            var result = subject.Decode(this.StateMock.Object);

            Assert.NotNull(result);

            Assert.Equal(instruction, result.Instruction);
            Assert.Equal(opcodeInfo.Cycles, result.Cycles);
            Assert.Equal(paramByte, result.ValueParameter);
        }
    }
}
