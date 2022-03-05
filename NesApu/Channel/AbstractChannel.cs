namespace NesApu.Channel;

/// <summary>
/// Describes a basic audio channel
/// </summary>
public abstract class AbstractChannel
{
    #region Constants
    /// <summary>
    /// Lookup table for <see cref="LengthCounter"/> and CPU cycles
    /// </summary>
    protected static byte[] LengthCounterLookup { get; } =
    {
        10, 254, 20, 2, 40, 4, 80, 6, 160, 8, 60, 10, 14, 12, 26, 14,
        12, 16, 24, 18, 48, 20, 96, 22, 192, 24, 72, 26, 16, 28, 32, 30
    };
    #endregion

    #region Properties
    /// <summary>
    /// Total periods for this channel to execute
    /// </summary>
    public ushort Period { get; set; }

    /// <summary>
    /// Remaining periods for this channel to execute
    /// </summary>
    protected ushort Remaining { get; set; }

    /// <summary>
    /// Relationship of <see cref="Period"/> and CPU cycles
    /// </summary>
    protected byte LengthCounter { get; set; }

    /// <summary>
    /// When true should disable <see cref="LengthCounter"/> execution
    /// </summary>
    protected bool LengthCounterHalt { get; set; }

    /// <summary>
    /// Current channel amplitude
    /// </summary>
    protected double Amplitude { get; set; }
    #endregion

    #region Constructors
    /// <summary>
    /// Instantiates a new <see cref="AbstractChannel"/>
    /// </summary>
    /// <param name="amplitude">Initial <see cref="Amplitude"/></param>
    protected AbstractChannel(double amplitude)
    {
        this.Remaining = 1;
        this.Period = 1;

        this.Amplitude = amplitude;
    }
    #endregion

    /// <summary>
    /// Executes a step for this channel
    /// </summary>
    public void Step()
    {
        this.Remaining--;

        if (this.Remaining == 0)
        {
            this.OnTimer0();
            this.Remaining = this.Period;
        }
    }

    /// <summary>
    /// Resets the <see cref="LengthCounter"/>
    /// </summary>
    public void ResetLength()
    {
        this.LengthCounter = 0;
    }

    /// <summary>
    /// Checks if the <see cref="LengthCounter"/> has value
    /// </summary>
    /// <returns>True if larger than zero, false otherwise</returns>
    public bool HasLength()
    {
        return this.LengthCounter > 0;
    }

    /// <summary>
    /// Samples the channel
    /// </summary>
    /// <returns>Channel sample sound</returns>
    public abstract short GetSample();

    /// <summary>
    /// Executes the channel's Quarter Frame
    /// </summary>
    public abstract void QuarterFrame();

    /// <summary>
    /// Executes the channel's Half Frame
    /// </summary>
    public abstract void HalfFrame();

    /// <summary>
    /// Sets the <see cref="LengthCounter"/> based on the value.
    /// <seealso cref="LengthCounterLookup"/>
    /// </summary>
    /// <param name="value"></param>
    protected void SetLength(int value)
    {
        this.LengthCounter = LengthCounterLookup[value];
    }

    /// <summary>
    /// Sets the channel for timer 0 execution
    /// </summary>
    protected abstract void OnTimer0();
}
