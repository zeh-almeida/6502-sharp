using Cpu.Instructions.Illegal;
using Cpu.States;
using Moq;
using Test.Unit.Cpu.Utils;
using Xunit;

namespace Test.Unit.Cpu.Instructions.Illegal
{
    public sealed record AndXHighByteTest : IClassFixture<AndXHighByte>
    {
        #region Properties
        private AndXHighByte Subject { get; }
        #endregion

        #region Constructors
        public AndXHighByteTest(AndXHighByte subject)
        {
            this.Subject = subject;
        }
        #endregion

        [Theory]
        [InlineData(0x9E)]
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

        [Theory]
        [InlineData(0x40, 0x20, 0x00)]
        [InlineData(0x40, 0xFF, 0x41)]
        [InlineData(0x00, 0x20, 0x00)]
        public void Value_WriteAbsoluteY(ushort address, byte registerX, byte result)
        {
            var stateMock = SetupMock(registerX);

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Registers.IndexX, Times.Once());

            stateMock.Verify(state => state.Memory.WriteAbsoluteY(address, result), Times.Once());
        }

        private static Mock<ICpuState> SetupMock(byte registerX)
        {
            var stateMock = TestUtils.GenerateStateMock();

            _ = stateMock
                .Setup(s => s.ExecutingOpcode)
                .Returns(0x9E);

            _ = stateMock
                .Setup(s => s.Registers.IndexX)
                .Returns(registerX);

            return stateMock;
        }
    }
}
