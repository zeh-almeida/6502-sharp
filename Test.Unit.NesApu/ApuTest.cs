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

    [Fact]
    public void Cycle_SetFiveStep_FrameCounter()
    {
        const byte memoryValue = 0b_1000_0000;

        _ = this.StateMock
            .Setup(mock => mock.Memory.ReadAbsolute(IApu.FrameCounterAddress))
            .Returns(memoryValue);

        this.Apu.Cycle(this.StateMock.Object);

        Assert.Equal(SequencerMode.FiveStep, this.Apu.SequencerMode);
    }

    [Fact]
    public void Cycle_SetFourStep_FrameCounter()
    {
        const byte memoryValue = 0b_0000_0000;

        _ = this.StateMock
            .Setup(mock => mock.Memory.ReadAbsolute(IApu.FrameCounterAddress))
            .Returns(memoryValue);

        this.Apu.Cycle(this.StateMock.Object);

        Assert.Equal(SequencerMode.FourStep, this.Apu.SequencerMode);
    }

    [Fact]
    public void Cycle_SetIrqDisable_FrameCounter()
    {
        const byte memoryValue = 0b_0100_0000;

        _ = this.StateMock
            .Setup(mock => mock.Memory.ReadAbsolute(IApu.FrameCounterAddress))
            .Returns(memoryValue);

        this.Apu.Cycle(this.StateMock.Object);

        Assert.True(this.Apu.IsIrqDisable);
    }

    [Fact]
    public void Cycle_UnsetIrqDisable_FrameCounter()
    {
        const byte memoryValue = 0b_0000_0000;

        _ = this.StateMock
            .Setup(mock => mock.Memory.ReadAbsolute(IApu.FrameCounterAddress))
            .Returns(memoryValue);

        this.Apu.Cycle(this.StateMock.Object);

        Assert.False(this.Apu.IsIrqDisable);
    }

    [Fact]
    public void Cycle_Increase_CycleCount()
    {
        var currentCount = this.Apu.Cycles;

        this.Apu.Cycle(this.StateMock.Object);

        Assert.Equal(currentCount + 1, this.Apu.Cycles);
    }
}