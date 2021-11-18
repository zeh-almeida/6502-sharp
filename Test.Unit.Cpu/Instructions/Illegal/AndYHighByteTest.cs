using Cpu.Instructions.Exceptions;
using Cpu.Instructions.Illegal;
using Cpu.States;
using Moq;
using Test.Unit.Cpu.Utils;
using Xunit;

namespace Test.Unit.Cpu.Instructions.Illegal
{
    public sealed record AndYHighByteTest : IClassFixture<AndYHighByte>
    {
        #region Properties
        private AndYHighByte Subject { get; }
        #endregion

        #region Constructors
        public AndYHighByteTest(AndYHighByte subject)
        {
            this.Subject = subject;
        }
        #endregion

        [Theory]
        [InlineData(0x9C)]
        public void HasOpcode_Matches_True(byte opcode)
        {
            Assert.True(this.Subject.HasOpcode(opcode));
            Assert.NotNull(this.Subject.GatherInformation(opcode));
        }

        [Fact]
        public void GatherInformation_NoMatch_Throws()
        {
            _ = Assert.Throws<UnknownOpcodeException>(() => this.Subject.GatherInformation(0xFF));
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
        public void Value_WriteAbsoluteX(ushort address, byte registerY, byte result)
        {
            var stateMock = SetupMock(registerY);

            _ = this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Registers.IndexY, Times.Once());

            stateMock.Verify(state => state.Memory.WriteAbsoluteX(address, result), Times.Once());
        }

        private static Mock<ICpuState> SetupMock(byte registerY)
        {
            var stateMock = TestUtils.GenerateStateMock();

            _ = stateMock
                .Setup(s => s.ExecutingOpcode)
                .Returns(0x9C);

            _ = stateMock
                .Setup(s => s.Registers.IndexY)
                .Returns(registerY);

            return stateMock;
        }
    }
}
