using Cpu.Instructions.Exceptions;
using Cpu.Instructions.Illegal;
using Cpu.States;
using Moq;
using Test.Unit.Cpu.Utils;
using Xunit;

namespace Test.Unit.Cpu.Instructions.Illegal
{
    public sealed record AndAccumulatorXHighByteTest : IClassFixture<AndAccumulatorXHighByte>
    {
        #region Properties
        private AndAccumulatorXHighByte Subject { get; }
        #endregion

        #region Constructors
        public AndAccumulatorXHighByteTest(AndAccumulatorXHighByte subject)
        {
            this.Subject = subject;
        }
        #endregion

        [Theory]
        [InlineData(0x93)]
        [InlineData(0x9F)]
        public void HasOpcode_Matches_True(byte opcode)
        {
            Assert.True(this.Subject.HasOpcode(opcode));
        }

        [Fact]
        public void HashCode_Matches_True()
        {
            Assert.Equal(this.Subject.GetHashCode(), this.Subject.Opcodes.GetHashCode());
        }

        [Fact]
        public void Equals_Object_IsTrueForInstruction()
        {
            Assert.True(this.Subject.Equals(this.Subject));
            Assert.True(this.Subject.Equals(this.Subject as object));
        }

        [Fact]
        public void Equals_Object_IsFalseForNonInstructions()
        {
            Assert.False(this.Subject.Equals(1));
        }

        [Fact]
        public void Execute_UnknownOpcode_Throws()
        {
            var stateMock = SetupMock(0x00, 0x00, 0x00);
            _ = Assert.Throws<UnknownOpcodeException>(() => this.Subject.Execute(stateMock.Object, 0));
        }

        [Theory]
        [InlineData(0x4041, 0x20, 0xEA, 0x00)]
        [InlineData(0x4041, 0xFF, 0xEA, 0x40)]
        [InlineData(0x4041, 0xFF, 0xFF, 0x41)]
        [InlineData(0x0040, 0x20, 0xEA, 0x00)]
        [InlineData(0x4040, 0xFF, 0xEA, 0x40)]
        public void Value_WriteZeroPageY(ushort address, byte registerX, byte accumulator, byte result)
        {
            var stateMock = SetupMock(0x93, accumulator, registerX);

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Registers.Accumulator, Times.Once());
            stateMock.Verify(state => state.Registers.IndexX, Times.Once());

            stateMock.Verify(state => state.Memory.WriteZeroPageY(address, result), Times.Once());
        }

        [Theory]
        [InlineData(0x40, 0x20, 0xEA, 0x00)]
        [InlineData(0x40, 0xFF, 0xEA, 0x40)]
        [InlineData(0x40, 0xFF, 0xFF, 0x41)]
        [InlineData(0x00, 0x20, 0xEA, 0x00)]
        public void Value_WriteAbsoluteY(ushort address, byte registerX, byte accumulator, byte result)
        {
            var stateMock = SetupMock(0x9F, accumulator, registerX);

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Registers.Accumulator, Times.Once());
            stateMock.Verify(state => state.Registers.IndexX, Times.Once());

            stateMock.Verify(state => state.Memory.WriteAbsoluteY(address, result), Times.Once());
        }

        private static Mock<ICpuState> SetupMock(byte opcode, byte accumulator, byte registerX)
        {
            var stateMock = TestUtils.GenerateStateMock();

            _ = stateMock
                .Setup(s => s.ExecutingOpcode)
                .Returns(opcode);

            _ = stateMock
                .Setup(s => s.Registers.Accumulator)
                .Returns(accumulator);

            _ = stateMock
                .Setup(s => s.Registers.IndexX)
                .Returns(registerX);

            return stateMock;
        }
    }
}
