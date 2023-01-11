
using Cpu.Instructions.Exceptions;
using Cpu.Instructions.SystemFunctions;
using Moq;
using Test.Unit.Cpu.Utils;
using Xunit;

namespace Test.Unit.Cpu.Instructions.SystemFunctions
{
    public sealed record ReturnFromInterruptTest
    {
        #region Properties
        private ReturnFromInterrupt Subject { get; }
        #endregion

        #region Constructors
        public ReturnFromInterruptTest()
        {
            this.Subject = new ReturnFromInterrupt();
        }
        #endregion

        [Theory]
        [InlineData(0x40)]
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
        public void Execute_Status_IsRestored()
        {
            const byte stateValue = 0b_1010_0101;

            var stateMock = TestUtils.GenerateStateMock();

            _ = stateMock
                .Setup(state => state.Stack.Pull())
                .Returns(stateValue);

            this.Subject.Execute(stateMock.Object, 0);

            stateMock.Verify(state => state.Stack.Pull(), Times.Exactly(1));
            stateMock.Verify(state => state.Stack.Pull16(), Times.Exactly(1));
            stateMock.Verify(state => state.Flags.Load(stateValue), Times.Once());
        }

        [Fact]
        public void Execute_InterruptProgram_IsSetup()
        {
            const ushort interruptCounter = 0b_1010_0000_1010_0000;

            var stateMock = TestUtils.GenerateStateMock();

            _ = stateMock
                .Setup(state => state.Stack.Pull16())
                .Returns(interruptCounter);

            this.Subject.Execute(stateMock.Object, 0);

            stateMock.Verify(state => state.Stack.Pull(), Times.Exactly(1));
            stateMock.Verify(state => state.Stack.Pull16(), Times.Exactly(1));
            stateMock.VerifySet(state => state.Registers.ProgramCounter = interruptCounter, Times.Once());
        }
    }
}
