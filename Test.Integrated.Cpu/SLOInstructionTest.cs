using Test.Integrated.Cpu.Common;
using Xunit;

namespace Test.Integrated.Cpu
{
    public sealed record SLOInstructionTest : IClassFixture<MachineFixture>
    {
        #region Properties
        private MachineFixture Fixture { get; }
        #endregion

        #region Constructors
        public SLOInstructionTest(MachineFixture fixture)
        {
            this.Fixture = fixture;
        }
        #endregion

        [Theory]
        [InlineData("asoa")]
        [InlineData("asoax")]
        [InlineData("asoay")]
        [InlineData("asoix")]
        [InlineData("asoiy")]
        [InlineData("asoz")]
        [InlineData("asozx")]
        public void Illegal_Instruction_Computes(string programName)
        {
            _ = this.Fixture.Compute(programName);
        }
    }
}
