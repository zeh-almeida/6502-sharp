using Cpu.Instructions.Exceptions;
using Cpu.Instructions.Increments;
using Cpu.States;
using Moq;
using Test.Unit.Cpu.Utils;
using Xunit;

namespace Test.Unit.Cpu.Instructions.Increments
{
    public sealed record IncrementRegisterYTest : IClassFixture<IncrementRegisterY>
    {
        #region Properties
        private IncrementRegisterY Subject { get; }
        #endregion

        #region Constructors
        public IncrementRegisterYTest(IncrementRegisterY subject)
        {
            this.Subject = subject;
        }
        #endregion

        [Theory]
        [InlineData(0xC8)]
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
        public void Execute_IncrementWraps_WritesZeroFlag()
        {
            const byte value = 0b_1111_1111;
            const byte result = 0b_0000_0000;

            var stateMock = SetupMock(0xC8, value);

            this.Subject.Execute(stateMock.Object, 0);

            stateMock.Verify(state => state.Registers.IndexY, Times.Once());
            stateMock.VerifySet(state => state.Registers.IndexY = result, Times.Once());

            stateMock.VerifySet(state => state.Flags.IsZero = true, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsNegative = false, Times.Once());
        }

        [Fact]
        public void Execute_ResultSeventhBitSet_WritesNegativeFlag()
        {
            const byte value = 0b_0111_1111;
            const byte result = 0b_1000_0000;

            var stateMock = SetupMock(0xC8, value);

            this.Subject.Execute(stateMock.Object, 0);

            stateMock.Verify(state => state.Registers.IndexY, Times.Once());
            stateMock.VerifySet(state => state.Registers.IndexY = result, Times.Once());

            stateMock.VerifySet(state => state.Flags.IsZero = false, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsNegative = true, Times.Once());
        }

        [Fact]
        public void Execute_Increments_ChangesRegister()
        {
            const byte value = 0b_0000_0000;
            const byte result = 0b_0000_0001;

            var stateMock = SetupMock(0xC8, value);

            this.Subject.Execute(stateMock.Object, 0);

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
