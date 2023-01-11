
using Cpu.Instructions.Exceptions;
using Cpu.Instructions.Jumps;
using Moq;
using Test.Unit.Cpu.Utils;
using Xunit;

namespace Test.Unit.Cpu.Instructions.Jumps
{
    public sealed record JumpToSubroutineTest
    {
        #region Properties
        private JumpToSubroutine Subject { get; }
        #endregion

        #region Constructors
        public JumpToSubroutineTest()
        {
            this.Subject = new JumpToSubroutine();
        }
        #endregion

        [Theory]
        [InlineData(0x20)]
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
        public void Execute_Value_WritesProgramCounter()
        {
            const ushort value = 1;
            const byte pointer = 2;
            const ushort result = 2;

            var stateMock = TestUtils.GenerateStateMock();

            _ = stateMock
                .Setup(state => state.Registers.ProgramCounter)
                .Returns(pointer);

            this.Subject.Execute(stateMock.Object, value);

            stateMock.Verify(state => state.Stack.Push16(result), Times.Exactly(1));
            stateMock.VerifySet(state => state.Registers.ProgramCounter = value, Times.Once());
        }
    }
}
