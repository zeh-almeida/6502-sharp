using Cpu.Extensions;
using Cpu.States;

namespace NesApu;

/// <summary>
/// Implements <see cref="IApu"/> to handle audio processing
/// </summary>
public sealed record Apu : IApu
{
    #region Variables
    private double _amplitude;
    #endregion

    #region Properties
    /// <inheritdoc/>
    public SequencerMode SequencerMode { get; private set; }

    /// <inheritdoc/>
    public bool IsIrqDisable { get; private set; }

    /// <inheritdoc/>
    public bool IsFrameInterrupt { get; private set; }

    /// <inheritdoc/>
    public int Cycles { get; private set; }

    /// <inheritdoc/>
    public double Amplitude
    {
        get => this._amplitude;

        set
        {
            this._amplitude += value;

            if (this._amplitude < 0)
            {
                this._amplitude = 0;
            }
        }
    }
    #endregion

    #region Constructors
    /// <summary>
    /// Instantiates a new <see cref="Apu"/>
    /// </summary>
    public Apu()
    {
        this.Amplitude = 0.05;
    }
    #endregion

    /// <inheritdoc/>
    public void Cycle(ICpuState cpuState)
    {
        this.LoadFrameCounter(cpuState);

        if (SequencerMode.FiveStep.Equals(this.SequencerMode))
        {
            this.ProcessFiveStep();
        }
        else
        {
            this.ProcessFourStep();
        }

        this.Cycles++;
    }

    private void LoadFrameCounter(ICpuState cpuState)
    {
        var value = cpuState.Memory.ReadAbsolute(IApu.FrameCounterAddress);

        this.SequencerMode = value.IsLastBitSet()
                           ? SequencerMode.FiveStep
                           : SequencerMode.FourStep;

        this.IsIrqDisable = value.IsBitSet(6);

        if (this.IsIrqDisable)
        {
            this.IsFrameInterrupt = false;
        }
    }

    private void ProcessFiveStep()
    {
        switch (this.Cycles)
        {
            case 3728:
            case 11185:
                this.ExecuteQuarterFrame();
                break;

            case 7456:
            case 18640:
                this.ExecuteQuarterFrame();
                this.ExecuteHalfFrame();
                break;

            case 18641:
                this.Cycles = 0;
                break;
        }
    }

    private void ProcessFourStep()
    {
        switch (this.Cycles)
        {
            case 3728:
            case 11185:
                this.ExecuteQuarterFrame();
                break;

            case 7456:
                this.ExecuteQuarterFrame();
                this.ExecuteHalfFrame();
                break;

            case 14914:
                if (!this.IsIrqDisable)
                {
                    this.IsFrameInterrupt = true;
                }

                this.ExecuteQuarterFrame();
                this.ExecuteHalfFrame();
                break;

            case 14915:
                if (!this.IsIrqDisable)
                {
                    this.IsFrameInterrupt = true;
                }

                this.Cycles = 0;
                break;
        }
    }

    private void ExecuteQuarterFrame() { }

    private void ExecuteHalfFrame() { }
}