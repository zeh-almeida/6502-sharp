using Cpu.Instructions.Jumps;
using Moq;
using Test.Unit.Cpu.Utils;
using Xunit;

namespace Test.Unit.Cpu.Instructions.Jumps
{
    public sealed record ReturnFromSubroutineTest
    {
        #region Properties
        private ReturnFromSubroutine Subject { get; }
        #endregion

        #region Constructors
        public ReturnFromSubroutineTest()
        {
            this.Subject = new ReturnFromSubroutine();
        }
        #endregion

        [Theory]
        [InlineData(0x60)]
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
        public void Execute_Pointer_WritesFromStack()
        {
            const ushort value = 4;
            const ushort result = 4;

            var stateMock = TestUtils.GenerateStateMock();

            _ = stateMock
                .Setup(state => state.Stack.Pull16())
                .Returns(value);

            this.Subject.Execute(stateMock.Object, 0);

            stateMock.Verify(state => state.Stack.Pull16(), Times.Exactly(1));
            stateMock.VerifySet(state => state.Registers.ProgramCounter = result, Times.Once());
        }
    }
}
