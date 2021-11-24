using Test.Integrated.Cpu.Common;
using Xunit;

namespace Test.Integrated.Cpu
{
    public sealed record ANCInstructionTest : IClassFixture<MachineFixture>
    {
        #region Properties
        private MachineFixture Fixture { get; }
        #endregion

        #region Constructors
        public ANCInstructionTest(MachineFixture fixture)
        {
            this.Fixture = fixture;
        }
        #endregion

        [Theory]
        [InlineData("ancb")]
        public void Illegal_Instruction_Computes(string programName)
        {
            _ = this.Fixture.Compute(programName);
        }
    }
}
