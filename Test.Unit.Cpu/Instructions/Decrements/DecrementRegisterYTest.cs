using Cpu.Instructions.Decrements;
using Cpu.Instructions.Exceptions;
using Cpu.States;
using Moq;
using Test.Unit.Cpu.Utils;
using Xunit;

namespace Test.Unit.Cpu.Instructions.Decrements
{
    public sealed record DecrementRegisterYTest : IClassFixture<DecrementRegisterY>
    {
        #region Properties
        private DecrementRegisterY Subject { get; }
        #endregion

        #region Constructors
        public DecrementRegisterYTest(DecrementRegisterY subject)
        {
            this.Subject = subject;
        }
        #endregion

        [Theory]
        [InlineData(0x88)]
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

        [Fact]
        public void Execute_DecrementToZero_WritesZeroFlag()
        {
            const byte value = 0b_0000_0001;
            const byte result = 0b_0000_0000;

            var stateMock = SetupMock(0x88, value);

            _ = this.Subject.Execute(stateMock.Object, 0);

            stateMock.Verify(state => state.Registers.IndexY, Times.Once());
            stateMock.VerifySet(state => state.Registers.IndexY = result, Times.Once());

            stateMock.VerifySet(state => state.Flags.IsZero = true, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsNegative = false, Times.Once());
        }

        [Fact]
        public void Execute_ResultSeventhBitSet_WritesNegativeFlag()
        {
            const byte value = 0b_1111_1111;
            const byte result = 0b_1111_1110;

            var stateMock = SetupMock(0x88, value);

            _ = this.Subject.Execute(stateMock.Object, 0);

            stateMock.Verify(state => state.Registers.IndexY, Times.Once());
            stateMock.VerifySet(state => state.Registers.IndexY = result, Times.Once());

            stateMock.VerifySet(state => state.Flags.IsZero = false, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsNegative = true, Times.Once());
        }

        [Fact]
        public void Execute_Increments_ChangesRegister()
        {
            const byte value = 0b_0000_0010;
            const byte result = 0b_0000_0001;

            var stateMock = SetupMock(0x88, value);

            _ = this.Subject.Execute(stateMock.Object, 0);

            stateMock.Verify(state => state.Registers.IndexY, Times.Once());
            stateMock.VerifySet(state => state.Registers.IndexY = result, Times.Once());
        }

        private static Mock<ICpuState> SetupMock(byte opcode, byte registerY)
        {
            var stateMock = TestUtils.GenerateStateMock();

            _ = stateMock
                .Setup(s => s.ExecutingOpcode)
                .Returns(opcode);

            _ = stateMock
                .Setup(state => state.Registers.IndexY)
                .Returns(registerY);

            return stateMock;
        }
    }
}
