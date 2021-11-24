using Test.Integrated.Cpu.Common;
using Xunit;

namespace Test.Integrated.Cpu
{
    public sealed record RLAInstructionTest : IClassFixture<MachineFixture>
    {
        #region Properties
        private MachineFixture Fixture { get; }
        #endregion

        #region Constructors
        public RLAInstructionTest(MachineFixture fixture)
        {
            this.Fixture = fixture;
        }
        #endregion

        [Theory]
        [InlineData("rlaa")]
        [InlineData("rlaax")]
        [InlineData("rlaay")]
        [InlineData("rlaix")]
        [InlineData("rlaiy")]
        [InlineData("rlaz")]
        [InlineData("rlazx")]
        public void Illegal_Instruction_Computes(string programName)
        {
            _ = this.Fixture.Compute(programName);
        }
    }
}
