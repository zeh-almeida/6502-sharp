
using Cpu.Instructions.Exceptions;
using Cpu.Instructions.SystemFunctions;
using Test.Unit.Cpu.Utils;
using Xunit;

namespace Test.Unit.Cpu.Instructions.SystemFunctions
{
    public sealed record NoOperationTest
    {
        #region Properties
        private NoOperation Subject { get; }
        #endregion

        #region Constructors
        public NoOperationTest()
        {
            this.Subject = new NoOperation();
        }
        #endregion

        [Theory]
        [InlineData(0xEA)]
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
        public void Execute_State_NoUsages()
        {
            var stateMock = TestUtils.GenerateStateMock();

            this.Subject.Execute(stateMock.Object, 0);

            stateMock.VerifyNoOtherCalls();
        }
    }
}
