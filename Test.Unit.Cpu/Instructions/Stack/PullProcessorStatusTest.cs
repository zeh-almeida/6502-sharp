
using Cpu.Instructions.Exceptions;
using Cpu.Instructions.Stack;
using Moq;
using Test.Unit.Cpu.Utils;
using Xunit;

namespace Test.Unit.Cpu.Instructions.Stack
{
    public sealed record PullProcessorStatusTest : IClassFixture<PullProcessorStatus>
    {
        #region Properties
        private PullProcessorStatus Subject { get; }
        #endregion

        #region Constructors
        public PullProcessorStatusTest(PullProcessorStatus subject)
        {
            this.Subject = subject;
        }
        #endregion

        [Theory]
        [InlineData(0x28)]
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
        public void Execute_State_WritesStack()
        {
            const byte value = 0b_1001_1001;
            var stateMock = TestUtils.GenerateStateMock();

            _ = stateMock
                .Setup(s => s.Stack.Pull())
                .Returns(value);

            _ = this.Subject.Execute(stateMock.Object, 0);

            stateMock.Verify(state => state.Flags.Load(value), Times.Once());
        }
    }
}
