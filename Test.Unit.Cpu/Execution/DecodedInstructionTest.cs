using Cpu.Execution;
using Cpu.Instructions;
using Cpu.Opcodes;
using Moq;
using Xunit;

namespace Test.Unit.Cpu.Execution
{
    public sealed record DecodedInstructionTest
    {
        #region Constants
        private const ushort Value = 0x01;

        private const int Cycles = 1;

        private const byte Opcode = 0x01;

        private const byte Bytes = 1;
        #endregion

        [Fact]
        public void Instruction_Equals_Defined()
        {
            var instructionMock = new Mock<IInstruction>();
            var opcodeMock = new Mock<IOpcodeInformation>();

            _ = instructionMock
                .Setup(mock => mock.GetHashCode())
                .Returns(1);

            _ = opcodeMock
                .Setup(mock => mock.Opcode)
                .Returns(Opcode);

            _ = opcodeMock
                .Setup(mock => mock.MaximumCycles)
                .Returns(Cycles);

            _ = opcodeMock
                .Setup(mock => mock.MinimumCycles)
                .Returns(Cycles);

            _ = opcodeMock
                .Setup(mock => mock.Bytes)
                .Returns(Bytes);

            var instruction = instructionMock.Object;
            var opcodeInfo = opcodeMock.Object;

            var subject = new DecodedInstruction(opcodeInfo, instruction, Value);
            Assert.Equal(instruction.GetHashCode(), subject.Instruction?.GetHashCode());
        }

        [Fact]
        public void Cycles_Equals_Defined()
        {
            var instructionMock = new Mock<IInstruction>();
            var opcodeMock = new Mock<IOpcodeInformation>();

            _ = instructionMock
                .Setup(mock => mock.GetHashCode())
                .Returns(1);

            _ = opcodeMock
                .Setup(mock => mock.Opcode)
                .Returns(Opcode);

            _ = opcodeMock
                .Setup(mock => mock.MaximumCycles)
                .Returns(Cycles);

            _ = opcodeMock
                .Setup(mock => mock.MinimumCycles)
                .Returns(Cycles);

            _ = opcodeMock
                .Setup(mock => mock.Bytes)
                .Returns(Bytes);

            var instruction = instructionMock.Object;
            var opcodeInfo = opcodeMock.Object;

            var subject = new DecodedInstruction(opcodeInfo, instruction, Value);

            Assert.Equal(Cycles, subject.Cycles);
        }

        [Fact]
        public void Bytes_Equals_Defined()
        {
            var instructionMock = new Mock<IInstruction>();
            var opcodeMock = new Mock<IOpcodeInformation>();

            _ = instructionMock
                .Setup(mock => mock.GetHashCode())
                .Returns(1);

            _ = opcodeMock
                .Setup(mock => mock.Opcode)
                .Returns(Opcode);

            _ = opcodeMock
                .Setup(mock => mock.MaximumCycles)
                .Returns(Cycles);

            _ = opcodeMock
                .Setup(mock => mock.MinimumCycles)
                .Returns(Cycles);

            _ = opcodeMock
                .Setup(mock => mock.Bytes)
                .Returns(Bytes);

            var instruction = instructionMock.Object;
            var opcodeInfo = opcodeMock.Object;

            var subject = new DecodedInstruction(opcodeInfo, instruction, Value);

            Assert.Equal(Value, subject.ValueParameter);
        }

        [Fact]
        public void ToString_IsFormatted()
        {
            var instructionMock = new Mock<IInstruction>();
            var opcodeMock = new Mock<IOpcodeInformation>();

            _ = instructionMock
                .Setup(mock => mock.GetHashCode())
                .Returns(1);

            _ = opcodeMock
                .Setup(mock => mock.Opcode)
                .Returns(Opcode);

            _ = opcodeMock
                .Setup(mock => mock.MaximumCycles)
                .Returns(Cycles);

            _ = opcodeMock
                .Setup(mock => mock.MinimumCycles)
                .Returns(Cycles);

            _ = opcodeMock
                .Setup(mock => mock.Bytes)
                .Returns(Bytes);

            var instruction = instructionMock.Object;
            var opcodeInfo = opcodeMock.Object;

            var subject = new DecodedInstruction(opcodeInfo, instruction, Value);

            Assert.Equal("0x01 (0x0001)", subject.ToString());
        }
    }
}
