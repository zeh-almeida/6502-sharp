using Cpu.States;
using Moq;
using NesApu;
using Test.Unit.NesApu.Utils;
using Xunit;

namespace Test.Unit.NesApu;

public sealed record ApuTest
{
    #region Properties
    private Apu Apu { get; set; }

    private Mock<ICpuState> StateMock { get; }
    #endregion

    #region Constructors
    public ApuTest()
    {
        this.Apu = new Apu();
        this.StateMock = TestUtils.GenerateStateMock();
    }
    #endregion

    [Fact]
    public void Initialized_Amplitude_IsSet()
    {
        Assert.Equal(0.05, this.Apu.Amplitude);
    }
}