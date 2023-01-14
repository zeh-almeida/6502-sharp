using Cpu.Instructions.Illegal;
using Cpu.States;
using Moq;
using Test.Unit.Cpu.Utils;
using Xunit;

namespace Test.Unit.Cpu.Instructions.Illegal
{
    public sealed record AndStackPointerTest : IClassFixture<AndStackPointer>
    {
        #region Constants
        private const byte OpCode = 0xBB;
        #endregion

        #region Properties
        private AndStackPointer Subject { get; }
        #endregion

        #region Constructors
        public AndStackPointerTest(AndStackPointer subject)
        {
            this.Subject = subject;
        }
        #endregion

        [Theory]
        [InlineData(0xBB)]
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
        public void Execute_WritesZeroFlag()
        {
            const ushort address = 0b_0000_0000;
            const byte value = 0b_0000_0000;
            const byte stackPointer = 0b_0000_0000;

            const byte result = 0b_0000_0000;

            var stateMock = SetupMock(stackPointer, address, value);

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.IncrementCycles(It.IsAny<int>()), Times.Never());

            stateMock.Verify(state => state.Memory.ReadAbsoluteY(address), Times.Once());
            stateMock.Verify(state => state.Registers.StackPointer, Times.Once());

            stateMock.VerifySet(state => state.Flags.IsZero = true, Times.Once());

            stateMock.VerifySet(state => state.Registers.IndexX = result, Times.Once());
            stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());
            stateMock.VerifySet(state => state.Registers.StackPointer = result, Times.Once());
        }

        [Fact]
        public void Execute_WritesZeroFlag_IncreaseCycle()
        {
            const ushort address = 0b_0000_0000;
            const byte value = 0b_0000_0000;
            const byte stackPointer = 0b_0000_0000;

            const byte result = 0b_0000_0000;

            var stateMock = SetupMock(stackPointer, address, value, true);

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.IncrementCycles(1), Times.Once());

            stateMock.Verify(state => state.Memory.ReadAbsoluteY(address), Times.Once());
            stateMock.Verify(state => state.Registers.StackPointer, Times.Once());

            stateMock.VerifySet(state => state.Flags.IsZero = true, Times.Once());

            stateMock.VerifySet(state => state.Registers.IndexX = result, Times.Once());
            stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());
            stateMock.VerifySet(state => state.Registers.StackPointer = result, Times.Once());
        }

        [Fact]
        public void Execute_WritesNegativeFlag()
        {
            const ushort address = 0b_0000_0000;
            const byte value = 0b_1000_0000;
            const byte stackPointer = 0b_1000_0000;

            const byte result = 0b_1000_0000;

            var stateMock = SetupMock(stackPointer, address, value);

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Memory.ReadAbsoluteY(address), Times.Once());
            stateMock.Verify(state => state.Registers.StackPointer, Times.Once());

            stateMock.VerifySet(state => state.Flags.IsNegative = true, Times.Once());

            stateMock.VerifySet(state => state.Registers.IndexX = result, Times.Once());
            stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());
            stateMock.VerifySet(state => state.Registers.StackPointer = result, Times.Once());
        }

        private static Mock<ICpuState> SetupMock(
            byte stackPointer,
            ushort address,
            byte value,
            bool crossPage = false)
        {
            var stateMock = TestUtils.GenerateStateMock();

            _ = stateMock
                .Setup(s => s.ExecutingOpcode)
                .Returns(OpCode);

            _ = stateMock
                .Setup(s => s.Registers.StackPointer)
                .Returns(stackPointer);

            _ = stateMock
                .Setup(s => s.Memory.ReadAbsoluteY(address))
                .Returns((crossPage, value));

            return stateMock;
        }
    }
}
