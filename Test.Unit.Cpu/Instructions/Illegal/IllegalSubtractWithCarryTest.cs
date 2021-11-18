using Cpu.Instructions.Exceptions;
using Cpu.Instructions.Illegal;
using Cpu.States;
using Moq;
using Test.Unit.Cpu.Utils;
using Xunit;

namespace Test.Unit.Cpu.Instructions.Illegal
{
    public sealed record IllegalSubtractWithCarryTest : IClassFixture<IllegalSubtractWithCarry>
    {
        #region Properties
        private IllegalSubtractWithCarry Subject { get; }
        #endregion

        #region Constructors
        public IllegalSubtractWithCarryTest(IllegalSubtractWithCarry subject)
        {
            this.Subject = subject;
        }
        #endregion

        [Theory]
        [InlineData(0xEB)]
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
        public void Execute_Equals_WritesZeroFlag()
        {
            const byte accumulator = 0b_0000_0000;
            const byte value = 0b_0000_0000;
            const byte result = 0b_0000_0000;

            var stateMock = SetupMock(accumulator);

            _ = this.Subject.Execute(stateMock.Object, value);

            stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());

            stateMock.VerifySet(state => state.Flags.IsZero = true, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsNegative = false, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsOverflow = false, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsCarry = true, Times.Once());
        }

        [Fact]
        public void Execute_ResultSeventhBitSet_WritesNegativeFlag()
        {
            const byte accumulator = 0b_0000_0101;
            const byte value = 0b_000_0110;
            const byte result = 0b_1111_1111;

            var stateMock = SetupMock(accumulator);

            _ = this.Subject.Execute(stateMock.Object, value);

            stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());

            stateMock.VerifySet(state => state.Flags.IsZero = false, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsNegative = true, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsOverflow = false, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsCarry = false, Times.Once());
        }

        [Fact]
        public void Execute_ResultOverflows_WritesOverflowFlag()
        {
            const byte accumulator = 0b_0000_0101;
            const byte value = 0b_000_0001;
            const byte result = 0b_0000_0100;

            var stateMock = SetupMock(accumulator);

            _ = this.Subject.Execute(stateMock.Object, value);

            stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());

            stateMock.VerifySet(state => state.Flags.IsZero = false, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsNegative = false, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsOverflow = true, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsCarry = true, Times.Once());
        }

        [Fact]
        public void Execute_ResultPositiveBit_WritesCarryFlag()
        {
            const byte accumulator = 0b_0000_0101;
            const byte value = 0b_0000_0001;
            const byte result = 0b_0000_0100;

            var stateMock = SetupMock(accumulator);

            _ = this.Subject.Execute(stateMock.Object, value);

            stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());

            stateMock.VerifySet(state => state.Flags.IsZero = false, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsNegative = false, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsOverflow = true, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsCarry = true, Times.Once());
        }

        [Fact]
        public void Execute_NoCarry_Sums()
        {
            const byte accumulator = 0b_0000_0101;
            const byte value = 0b_000_0001;
            const byte result = 0b_0000_0011;

            var stateMock = SetupMock(accumulator);

            _ = stateMock
                .Setup(s => s.Flags.IsCarry)
                .Returns(false);

            _ = this.Subject.Execute(stateMock.Object, value);

            stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());
        }

        [Fact]
        public void Execute_Immediate_Subtracts()
        {
            const byte accumulator = 0b_0000_0101;
            const byte value = 0b_000_0001;
            const byte result = 0b_0000_0100;

            var stateMock = SetupMock(accumulator);

            _ = this.Subject.Execute(stateMock.Object, value);

            stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());
        }

        private static Mock<ICpuState> SetupMock(byte accumulator)
        {
            var stateMock = TestUtils.GenerateStateMock();

            _ = stateMock
                .Setup(s => s.ExecutingOpcode)
                .Returns(0xEB);

            _ = stateMock
                .Setup(s => s.Registers.Accumulator)
                .Returns(accumulator);

            _ = stateMock
                .Setup(s => s.Flags.IsCarry)
                .Returns(true);

            return stateMock;
        }
    }
}
