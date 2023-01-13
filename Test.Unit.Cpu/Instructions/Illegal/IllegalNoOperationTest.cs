using Cpu.Instructions.Illegal;
using Test.Unit.Cpu.Utils;
using Xunit;

namespace Test.Unit.Cpu.Instructions.Illegal
{
    public sealed record IllegalNoOperationTest : IClassFixture<IllegalNoOperation>
    {
        #region Properties
        private IllegalNoOperation Subject { get; }
        #endregion

        #region Constructors
        public IllegalNoOperationTest(IllegalNoOperation subject)
        {
            this.Subject = subject;
        }
        #endregion

        [Theory]
        [InlineData(0x1A)]
        [InlineData(0x3A)]
        [InlineData(0x5A)]
        [InlineData(0x7A)]
        [InlineData(0xDA)]
        [InlineData(0xFA)]
        [InlineData(0x80)]
        [InlineData(0x82)]
        [InlineData(0x89)]
        [InlineData(0xC2)]
        [InlineData(0xE2)]
        [InlineData(0x04)]
        [InlineData(0x44)]
        [InlineData(0x64)]
        [InlineData(0x14)]
        [InlineData(0x34)]
        [InlineData(0x54)]
        [InlineData(0x74)]
        [InlineData(0xD4)]
        [InlineData(0xF4)]
        [InlineData(0x0C)]
        [InlineData(0x1C)]
        [InlineData(0x3C)]
        [InlineData(0x5C)]
        [InlineData(0x7C)]
        [InlineData(0xDC)]
        [InlineData(0xFC)]
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
        public void Execute_State_NoUsages()
        {
            var stateMock = TestUtils.GenerateStateMock();

            this.Subject.Execute(stateMock.Object, 0);

            stateMock.VerifyNoOtherCalls();
        }
    }
}
