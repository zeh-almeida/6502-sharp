using Test.Integrated.Cpu.Common;
using Xunit;

namespace Test.Integrated.Cpu;

public sealed record CustomProgramTest : IClassFixture<MachineFixture>
{
    #region Properties
    private MachineFixture Fixture { get; }
    #endregion

    #region Constructors
    public CustomProgramTest(MachineFixture fixture)
    {
        this.Fixture = fixture;
    }
    #endregion

    [Theory]
    [InlineData("count_until")]
    public void Program_Executes(string programName)
    {
        var result = this.Fixture.Compute(programName);
        Assert.True(result.Length > 0);
    }
}
