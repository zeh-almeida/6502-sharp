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
    public void SetAmplitude_LessThanZero_IsZero()
    {
        this.Apu.Amplitude = -1;
        Assert.Equal(0, this.Apu.Amplitude);
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

    [Theory]
    [InlineData(7456)]
    [InlineData(14914)]
    public void ProcessFourStep_HalfFrame_IsTriggered(int targetCycle)
    {
        const byte memoryValue = 0b_0000_0000;
        var target = targetCycle + 1;

        _ = this.StateMock
            .Setup(mock => mock.Memory.ReadAbsolute(IApu.FrameCounterAddress))
            .Returns(memoryValue);

        while (!target.Equals(this.Apu.Cycles))
        {
            this.Apu.Cycle(this.StateMock.Object);
        }

        Assert.True(this.Apu.IsHalfFrame);
    }

    [Theory]
    [InlineData(3728)]
    [InlineData(7456)]
    [InlineData(11185)]
    [InlineData(14914)]
    public void ProcessFourStep_QuarterFrame_IsTriggered(int targetCycle)
    {
        const byte memoryValue = 0b_0000_0000;
        var target = targetCycle + 1;

        _ = this.StateMock
            .Setup(mock => mock.Memory.ReadAbsolute(IApu.FrameCounterAddress))
            .Returns(memoryValue);

        while (!target.Equals(this.Apu.Cycles))
        {
            this.Apu.Cycle(this.StateMock.Object);
        }

        Assert.True(this.Apu.IsQuarterFrame);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(14914)]
    public void ProcessFourStep_IrqEnabled_IsFrameInterrupt_IsTriggered(int targetCycle)
    {
        const byte memoryValue = 0b_0000_0000;
        var target = targetCycle + 1;
        var hasCycled = false;

        _ = this.StateMock
            .Setup(mock => mock.Memory.ReadAbsolute(IApu.FrameCounterAddress))
            .Returns(memoryValue);

        while (!target.Equals(this.Apu.Cycles) || !hasCycled)
        {
            this.Apu.Cycle(this.StateMock.Object);

            if (this.Apu.Cycles > 1)
            {
                hasCycled = true;
            }
        }

        Assert.True(this.Apu.IsFrameInterrupt);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(14914)]
    public void ProcessFourStep_IrqDisabled_IsFrameInterrupt_IsTriggered(int targetCycle)
    {
        const byte memoryValue = 0b_0100_0000;
        var target = targetCycle + 1;
        var hasCycled = false;

        _ = this.StateMock
            .Setup(mock => mock.Memory.ReadAbsolute(IApu.FrameCounterAddress))
            .Returns(memoryValue);

        while (!target.Equals(this.Apu.Cycles) || !hasCycled)
        {
            this.Apu.Cycle(this.StateMock.Object);

            if (this.Apu.Cycles > 1)
            {
                hasCycled = true;
            }
        }

        Assert.False(this.Apu.IsFrameInterrupt);
    }

    [Fact]
    public void ProcessFourStep_Step_ZeroesCycles()
    {
        const byte memoryValue = 0b_0000_0000;
        const int beforeCycle = 14914;
        const int target = 0 + 1;

        var hasCycled = false;

        _ = this.StateMock
            .Setup(mock => mock.Memory.ReadAbsolute(IApu.FrameCounterAddress))
            .Returns(memoryValue);

        while (!target.Equals(this.Apu.Cycles) || !hasCycled)
        {
            this.Apu.Cycle(this.StateMock.Object);

            if (this.Apu.Cycles >= beforeCycle)
            {
                hasCycled = true;
            }
        }

        Assert.Equal(1, this.Apu.Cycles);
    }

    [Theory]
    [InlineData(7456)]
    [InlineData(18640)]
    public void ProcessFiveStep_HalfFrame_IsTriggered(int targetCycle)
    {
        const byte memoryValue = 0b_1000_0000;
        var target = targetCycle + 1;

        _ = this.StateMock
            .Setup(mock => mock.Memory.ReadAbsolute(IApu.FrameCounterAddress))
            .Returns(memoryValue);

        while (!target.Equals(this.Apu.Cycles))
        {
            this.Apu.Cycle(this.StateMock.Object);
        }

        Assert.True(this.Apu.IsHalfFrame);
    }

    [Theory]
    [InlineData(3728)]
    [InlineData(7456)]
    [InlineData(11185)]
    [InlineData(18640)]
    public void ProcessFiveStep_QuarterFrame_IsTriggered(int targetCycle)
    {
        const byte memoryValue = 0b_1000_0000;
        var target = targetCycle + 1;

        _ = this.StateMock
            .Setup(mock => mock.Memory.ReadAbsolute(IApu.FrameCounterAddress))
            .Returns(memoryValue);

        while (!target.Equals(this.Apu.Cycles))
        {
            this.Apu.Cycle(this.StateMock.Object);
        }

        Assert.True(this.Apu.IsQuarterFrame);
    }

    [Fact]
    public void ProcessFiveStep_Step_ZeroesCycles()
    {
        const byte memoryValue = 0b_1000_0000;
        const int beforeCycle = 18641;
        const int target = 0 + 1;

        var hasCycled = false;

        _ = this.StateMock
            .Setup(mock => mock.Memory.ReadAbsolute(IApu.FrameCounterAddress))
            .Returns(memoryValue);

        while (!target.Equals(this.Apu.Cycles) || !hasCycled)
        {
            this.Apu.Cycle(this.StateMock.Object);

            if (this.Apu.Cycles >= beforeCycle)
            {
                hasCycled = true;
            }
        }

        Assert.Equal(1, this.Apu.Cycles);
    }
}