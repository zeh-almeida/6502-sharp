using Cpu.States;

namespace NesApu;

public sealed record Apu
{
    #region Constants
    private const ushort FrameCounterAddress = 4017;

    private const byte SequencerByte = 0x80;

    private const byte IrqByte = 0x40;
    #endregion

    #region Variables
    private double _amplitude;
    #endregion

    #region Properties
    public SequencerMode SequencerMode { get; private set; }

    public bool IsIrqDisable { get; private set; }

    public bool IsFrameInterrupt { get; private set; }

    public int Cycles { get; private set; }

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
    public Apu()
    {
        this.Amplitude = 0.05;
    }
    #endregion

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
        var value = cpuState.Memory.ReadAbsolute(FrameCounterAddress);

        this.SequencerMode = (value & SequencerByte) > 0
                           ? SequencerMode.FiveStep
                           : SequencerMode.FourStep;

        this.IsIrqDisable = (value & IrqByte) > 0;

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