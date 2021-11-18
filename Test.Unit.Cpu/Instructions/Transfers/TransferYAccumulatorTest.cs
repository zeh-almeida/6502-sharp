using Cpu.Instructions.Exceptions;
using Cpu.Instructions.Transfers;
using Cpu.States;
using Moq;
using Test.Unit.Cpu.Utils;
using Xunit;

namespace Test.Unit.Cpu.Instructions.Transfers
{
    public sealed record TransferYAccumulatorTest
    {
        #region Properties
        private TransferYAccumulator Subject { get; }
        #endregion

        #region Constructors
        public TransferYAccumulatorTest()
        {
            this.Subject = new TransferYAccumulator();
        }
        #endregion

        [Theory]
        [InlineData(0x98)]
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
        public void Execute_ZeroIndex_ChangesZeroFlag()
        {
            const byte value = 0;
            var stateMock = SetupMock(value);

            _ = this.Subject.Execute(stateMock.Object, 0);

            BasicExecutionAssertion(stateMock, value);

            stateMock.VerifySet(state => state.Flags.IsZero = true, Times.Once());
        }

        [Fact]
        public void Execute_NegativeIndex_ChangesNegativeFlag()
        {
            const byte value = 0b_1000_0000;
            var stateMock = SetupMock(value);

            _ = this.Subject.Execute(stateMock.Object, 0);

            BasicExecutionAssertion(stateMock, value);

            stateMock.VerifySet(state => state.Flags.IsNegative = true, Times.Once());
        }

        [Fact]
        public void Execute_PositiveIndex_WritesRegister()
        {
            const byte value = 1;
            var stateMock = SetupMock(value);

            _ = this.Subject.Execute(stateMock.Object, 0);

            BasicExecutionAssertion(stateMock, value);

            stateMock.VerifySet(state => state.Flags.IsNegative = false, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsZero = false, Times.Once());
        }

        private static Mock<ICpuState> SetupMock(byte value)
        {
            var stateMock = TestUtils.GenerateStateMock();

            _ = stateMock
                .Setup(s => s.Registers.IndexY)
                .Returns(value);

            return stateMock;
        }

        private static void BasicExecutionAssertion(Mock<ICpuState> stateMock, byte value)
        {
            stateMock.Verify(state => state.Registers.IndexY, Times.Once());
            stateMock.VerifySet(state => state.Registers.Accumulator = value, Times.Once());
        }
    }
}
