using Cpu.Instructions.Exceptions;
using Cpu.Instructions.Illegal;
using Cpu.States;
using Moq;
using Test.Unit.Cpu.Utils;
using Xunit;

namespace Test.Unit.Cpu.Instructions.Illegal
{
    public sealed record AndLeftShiftTest : IClassFixture<AndLeftShift>
    {
        #region Constants
        private const byte OpCode = 0x4B;
        #endregion

        #region Properties
        private AndLeftShift Subject { get; }
        #endregion

        #region Constructors
        public AndLeftShiftTest(AndLeftShift subject)
        {
            this.Subject = subject;
        }
        #endregion

        [Theory]
        [InlineData(0x4B)]
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
            const byte value = 0b_0000_0000;
            const byte accumulator = 0b_0000_0000;
            const byte result = 0b_0000_0000;

            var stateMock = SetupMock(accumulator);

            _ = this.Subject.Execute(stateMock.Object, value);

            stateMock.Verify(state => state.Registers.Accumulator, Times.Once());
            stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());

            stateMock.VerifySet(state => state.Flags.IsCarry = false, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsZero = true, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsNegative = false, Times.Once());
        }

        [Fact]
        public void Rotate_SeventhBitSet_WritesNegativeFlag()
        {
            const byte value = 0b_0100_0011;
            const byte accumulator = 0b_0100_0000;
            const byte result = 0b_1000_0000;

            var stateMock = SetupMock(accumulator);

            _ = this.Subject.Execute(stateMock.Object, value);

            stateMock.Verify(state => state.Registers.Accumulator, Times.Once());
            stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());

            stateMock.VerifySet(state => state.Flags.IsNegative = true, Times.Once());
        }

        [Fact]
        public void And_SeventhBitSet_WritesCarryFlag()
        {
            const byte value = 0b_1000_0001;
            const byte accumulator = 0b_1000_0001;
            const byte result = 0b_0000_0010;

            var stateMock = SetupMock(accumulator);

            _ = this.Subject.Execute(stateMock.Object, value);

            stateMock.Verify(state => state.Registers.Accumulator, Times.Once());
            stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());

            stateMock.VerifySet(state => state.Flags.IsCarry = true, Times.Once());
        }

        private static Mock<ICpuState> SetupMock(byte accumulator)
        {
            var stateMock = TestUtils.GenerateStateMock();

            _ = stateMock
                .Setup(s => s.ExecutingOpcode)
                .Returns(OpCode);

            _ = stateMock
                .Setup(s => s.Registers.Accumulator)
                .Returns(accumulator);

            return stateMock;
        }
    }
}
