
using Cpu.Instructions.Exceptions;
using Cpu.Instructions.SystemFunctions;
using Moq;
using Test.Unit.Cpu.Utils;
using Xunit;

namespace Test.Unit.Cpu.Instructions.SystemFunctions
{
    public sealed record ForceInterruptTest
    {
        #region Properties
        private ForceInterrupt Subject { get; }
        #endregion

        #region Constructors
        public ForceInterruptTest()
        {
            this.Subject = new ForceInterrupt();
        }
        #endregion

        [Theory]
        [InlineData(0x00)]
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
        public void Execute_Status_IsStored()
        {
            const byte stateValue = 0b_1010_0101;

            var stateMock = TestUtils.GenerateStateMock();

            _ = stateMock
                .Setup(state => state.Flags.Save())
                .Returns(stateValue);

            this.Subject.Execute(stateMock.Object, 0);

            stateMock.Verify(state => state.Flags.Save(), Times.Once());

            stateMock.Verify(state => state.Stack.Push(stateValue), Times.Once());
        }

        [Fact]
        public void Execute_ProgramCounter_IsStored()
        {
            const ushort counter = 0b_1010_1010_0101_0101;

            var stateMock = TestUtils.GenerateStateMock();

            _ = stateMock
                .Setup(state => state.Registers.ProgramCounter)
                .Returns(counter);

            this.Subject.Execute(stateMock.Object, 0);

            stateMock.Verify(state => state.Registers.ProgramCounter, Times.Once());
            stateMock.Verify(state => state.Stack.Push16(counter), Times.Once());
        }

        [Fact]
        public void Execute_InterruptProgram_IsSetup()
        {
            const ushort interruptBits = 0b_1010_0000_0000_0101;
            const byte interruptMsb = 0b_1010_0000;
            const byte interruptLsb = 0b_0000_0101;

            const ushort lsbInterrupt = 0xFFFE;
            const ushort msbInterrupt = 0xFFFF;

            var stateMock = TestUtils.GenerateStateMock();

            _ = stateMock
                .Setup(state => state.Memory.ReadAbsolute(msbInterrupt))
                .Returns(interruptMsb);

            _ = stateMock
                .Setup(state => state.Memory.ReadAbsolute(lsbInterrupt))
                .Returns(interruptLsb);

            this.Subject.Execute(stateMock.Object, 0);

            stateMock.Verify(state => state.Memory.ReadAbsolute(lsbInterrupt), Times.Once());
            stateMock.Verify(state => state.Memory.ReadAbsolute(msbInterrupt), Times.Once());

            stateMock.VerifySet(state => state.Registers.ProgramCounter = interruptBits, Times.Once());
        }

        [Fact]
        public void Execute_InterruptProgram_FlagsSet()
        {
            const ushort interruptBits = 0b_1010_0000_0000_0101;
            const byte interruptMsb = 0b_1010_0000;
            const byte interruptLsb = 0b_0000_0101;

            const ushort lsbInterrupt = 0xFFFE;
            const ushort msbInterrupt = 0xFFFF;

            var stateMock = TestUtils.GenerateStateMock();

            _ = stateMock
                .Setup(state => state.Memory.ReadAbsolute(msbInterrupt))
                .Returns(interruptMsb);

            _ = stateMock
                .Setup(state => state.Memory.ReadAbsolute(lsbInterrupt))
                .Returns(interruptLsb);

            this.Subject.Execute(stateMock.Object, 0);

            stateMock.VerifySet(state => state.Flags.IsBreakCommand = true, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsInterruptDisable = true, Times.Once());

            stateMock.VerifySet(state => state.Registers.ProgramCounter = interruptBits, Times.Once());
        }
    }
}
