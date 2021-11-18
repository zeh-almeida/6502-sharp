using Cpu.Instructions.Exceptions;
using Cpu.Instructions.Illegal;
using Cpu.States;
using Moq;
using Test.Unit.Cpu.Utils;
using Xunit;

namespace Test.Unit.Cpu.Instructions.Illegal
{
    public sealed record AndOperandAccumulatorXTest : IClassFixture<AndOperandAccumulatorX>
    {
        #region Properties
        private AndOperandAccumulatorX Subject { get; }
        #endregion

        #region Constructors
        public AndOperandAccumulatorXTest(AndOperandAccumulatorX subject)
        {
            this.Subject = subject;
        }
        #endregion

        [Theory]
        [InlineData(0xAB)]
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
        public void ZeroValue_WritesZeroFlag()
        {
            const ushort value = 0b_0000_0000;
            const byte accumulator = 0b_0000_0000;

            const byte result = 0b_0000_0000;

            var stateMock = SetupMock(accumulator);

            _ = this.Subject.Execute(stateMock.Object, value);

            stateMock.Verify(state => state.Registers.Accumulator, Times.Once());

            stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());
            stateMock.VerifySet(state => state.Registers.IndexX = result, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsZero = true, Times.Once());
        }

        [Fact]
        public void NegativeValue_WritesNegativeFlag()
        {
            const ushort value = 0b_1000_0001;
            const byte accumulator = 0b_1000_0101;

            const byte result = 0b_1000_0001;

            var stateMock = SetupMock(accumulator);

            _ = this.Subject.Execute(stateMock.Object, value);

            stateMock.Verify(state => state.Registers.Accumulator, Times.Once());

            stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());
            stateMock.VerifySet(state => state.Registers.IndexX = result, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsNegative = true, Times.Once());
        }

        private static Mock<ICpuState> SetupMock(byte accumulator)
        {
            var stateMock = TestUtils.GenerateStateMock();

            _ = stateMock
                .Setup(s => s.ExecutingOpcode)
                .Returns(0xAB);

            _ = stateMock
                .Setup(s => s.Registers.Accumulator)
                .Returns(accumulator);

            return stateMock;
        }
    }
}
