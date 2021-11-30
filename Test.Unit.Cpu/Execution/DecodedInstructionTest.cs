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
            var instruction = instructionMock.Object;

            var opcodeInfo = new OpcodeInformation(Opcode, Cycles, Bytes)
                .SetInstruction(instruction);

            _ = instructionMock
                .Setup(mock => mock.GetHashCode())
                .Returns(1);

            var subject = new DecodedInstruction(opcodeInfo, Value);

            Assert.Equal(instruction.GetHashCode(), subject.Instruction.GetHashCode());
        }

        [Fact]
        public void Cycles_Equals_Defined()
        {
            var instructionMock = new Mock<IInstruction>();
            var opcodeInfo = new OpcodeInformation(Opcode, Cycles, Bytes)
                .SetInstruction(instructionMock.Object);

            var subject = new DecodedInstruction(opcodeInfo, Value);

            Assert.Equal(Cycles, subject.Cycles);
        }

        [Fact]
        public void Bytes_Equals_Defined()
        {
            var instructionMock = new Mock<IInstruction>();
            var opcodeInfo = new OpcodeInformation(Opcode, Cycles, Bytes)
                .SetInstruction(instructionMock.Object);

            var subject = new DecodedInstruction(opcodeInfo, Value);

            Assert.Equal(Value, subject.ValueParameter);
        }

        [Fact]
        public void ToString_IsFormatted()
        {
            var opcodeInfo = new OpcodeInformation(Opcode, Cycles, Bytes);
            var subject = new DecodedInstruction(opcodeInfo, Value);

            Assert.Equal("0x01 (0x0001)", subject.ToString());
        }
    }
}
