using Cpu.Execution;
using Cpu.Instructions;
using Cpu.Instructions.Exceptions;
using Cpu.Opcodes;
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
        public void NoOpcode_NoInstruction_Decode_Fails()
        {
            const ushort pcAddress = 1;
            const int streamByte = 0x38;

            _ = this.StateMock
                .Setup(mock => mock.Registers.ProgramCounter)
                .Returns(pcAddress);

            _ = this.StateMock
                .Setup(mock => mock.Memory.ReadAbsolute(pcAddress))
                .Returns(streamByte);

            var subject = new Decoder(
                Array.Empty<IOpcodeInformation>(),
                Array.Empty<IInstruction>());

            _ = Assert.Throws<UnknownOpcodeException>(() => subject.Decode(this.StateMock.Object));
        }

        [Fact]
        public void Opcode_NoInstruction_Decode_Fails()
        {
            const ushort pcAddress = 1;

            const int streamByte = 0x38;
            const int cycles = 2;
            const int bytes = 1;

            var opcodeMock = new Mock<IOpcodeInformation>();
            var opcodeInfo = opcodeMock.Object;

            _ = this.StateMock
                .Setup(mock => mock.Registers.ProgramCounter)
                .Returns(pcAddress);

            _ = this.StateMock
                .Setup(mock => mock.Memory.ReadAbsolute(pcAddress))
                .Returns(streamByte);

            _ = opcodeMock.Setup(m => m.Opcode)
                .Returns(streamByte);

            _ = opcodeMock.Setup(m => m.Bytes)
                .Returns(bytes);

            _ = opcodeMock.Setup(m => m.MinimumCycles)
                .Returns(cycles);

            _ = opcodeMock.Setup(m => m.MaximumCycles)
                .Returns(cycles);

            var subject = new Decoder(
                new IOpcodeInformation[] { opcodeInfo },
                Array.Empty<IInstruction>());

            _ = Assert.Throws<UnknownOpcodeException>(() => subject.Decode(this.StateMock.Object));
        }

        [Fact]
        public void NoOpcode_Instruction_Decode_Fails()
        {
            const ushort pcAddress = 1;
            const int streamByte = 0x38;

            var instructionMock = new Mock<IInstruction>();
            var instruction = instructionMock.Object;

            _ = this.StateMock
                .Setup(mock => mock.Registers.ProgramCounter)
                .Returns(pcAddress);

            _ = this.StateMock
                .Setup(mock => mock.Memory.ReadAbsolute(pcAddress))
                .Returns(streamByte);

            _ = instructionMock.Setup(m => m.HasOpcode(It.IsAny<byte>()))
                .Returns(true);

            var subject = new Decoder(
                Array.Empty<IOpcodeInformation>(),
                new IInstruction[] { instruction });

            _ = Assert.Throws<UnknownOpcodeException>(() => subject.Decode(this.StateMock.Object));
        }

        [Fact]
        public void InstructionNoParam_Decode_Successful()
        {
            const ushort pcAddress = 1;

            const int streamByte = 0x38;
            const int cycles = 2;
            const int bytes = 1;

            var opcodeMock = new Mock<IOpcodeInformation>();
            var instructionMock = new Mock<IInstruction>();

            var state = this.StateMock.Object;
            var opcodeInfo = opcodeMock.Object;
            var instruction = instructionMock.Object;

            _ = this.StateMock
                .Setup(mock => mock.Registers.ProgramCounter)
                .Returns(pcAddress);

            _ = this.StateMock
                .Setup(mock => mock.Memory.ReadAbsolute(pcAddress))
                .Returns(streamByte);

            _ = opcodeMock.Setup(m => m.Opcode)
                .Returns(streamByte);

            _ = opcodeMock.Setup(m => m.Bytes)
                .Returns(bytes);

            _ = opcodeMock.Setup(m => m.MinimumCycles)
                .Returns(cycles);

            _ = opcodeMock.Setup(m => m.MaximumCycles)
                .Returns(cycles);

            _ = instructionMock.Setup(m => m.HasOpcode(It.IsAny<byte>()))
                .Returns(true);

            var subject = new Decoder(
                new IOpcodeInformation[] { opcodeInfo },
                new IInstruction[] { instruction });

            var result = subject.Decode(this.StateMock.Object);

            Assert.NotNull(result);

            Assert.Equal(cycles, result.Information.MinimumCycles);
            Assert.Equal(0, result.ValueParameter);
        }

        [Fact]
        public void InstructionWithTwoParams_Decode_Successful()
        {
            const ushort pcAddress = 1;
            const ushort pcParam1Address = 2;
            const ushort pcParam2Address = 3;

            const int streamByte = 0x20;
            const int cycles = 6;
            const int bytes = 3;

            const int firstParamByte = 0b_0000_1111;
            const int secondParamByte = 0b_1111_0000;
            const ushort paramValue = 0b_1111_0000_0000_1111;

            var opcodeMock = new Mock<IOpcodeInformation>();
            var instructionMock = new Mock<IInstruction>();

            var state = this.StateMock.Object;
            var opcodeInfo = opcodeMock.Object;
            var instruction = instructionMock.Object;

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

            _ = opcodeMock.Setup(m => m.Opcode)
                .Returns(streamByte);

            _ = opcodeMock.Setup(m => m.Bytes)
                .Returns(bytes);

            _ = opcodeMock.Setup(m => m.MinimumCycles)
                .Returns(cycles);

            _ = opcodeMock.Setup(m => m.MaximumCycles)
                .Returns(cycles);

            _ = instructionMock.Setup(m => m.HasOpcode(It.IsAny<byte>()))
                .Returns(true);

            var subject = new Decoder(
                new IOpcodeInformation[] { opcodeInfo },
                new IInstruction[] { instruction });

            var result = subject.Decode(this.StateMock.Object);

            Assert.NotNull(result);

            Assert.Equal(cycles, result.Information.MinimumCycles);
            Assert.Equal(paramValue, result.ValueParameter);
        }

        [Fact]
        public void InstructionWithOneParam_Decode_Successful()
        {
            const ushort pcAddress = 1;
            const ushort pcParamAddress = 2;
            const int paramByte = 0b_0000_1111;

            const int streamByte = 0xB0;
            const int cycles = 5;
            const int bytes = 2;

            var opcodeMock = new Mock<IOpcodeInformation>();
            var instructionMock = new Mock<IInstruction>();

            var state = this.StateMock.Object;
            var opcodeInfo = opcodeMock.Object;
            var instruction = instructionMock.Object;

            _ = this.StateMock
                .Setup(mock => mock.Registers.ProgramCounter)
                .Returns(pcAddress);

            _ = this.StateMock
                .Setup(mock => mock.Memory.ReadAbsolute(pcAddress))
                .Returns(streamByte);

            _ = this.StateMock
                .Setup(mock => mock.Memory.ReadAbsolute(pcParamAddress))
                .Returns(paramByte);

            _ = opcodeMock.Setup(m => m.Opcode)
                .Returns(streamByte);

            _ = opcodeMock.Setup(m => m.Bytes)
                .Returns(bytes);

            _ = opcodeMock.Setup(m => m.MinimumCycles)
                .Returns(cycles);

            _ = opcodeMock.Setup(m => m.MaximumCycles)
                .Returns(cycles);

            _ = instructionMock.Setup(m => m.HasOpcode(It.IsAny<byte>()))
                .Returns(true);

            var subject = new Decoder(
                new IOpcodeInformation[] { opcodeInfo },
                new IInstruction[] { instruction });

            var result = subject.Decode(state);

            Assert.NotNull(result);

            Assert.Equal(cycles, result.Information.MinimumCycles);
            Assert.Equal(paramByte, result.ValueParameter);
        }
    }
}
