using NesApu.Utilities;

namespace NesApu.Channel;

/// <summary>
/// Describes a Noise Channel.
/// <see href="https://wiki.nesdev.com/w/index.php/APU_Noise"/>
/// </summary>
public sealed class Noise : AbstractChannel
{
    #region Constants
    /// <summary>
    /// PAL-based <see cref="AbstractChannel.Period"/> values
    /// </summary>
    private static ushort[] PeriodModes { get; } =
    {
        4, 8, 14, 30, 60, 88, 118, 148, 188, 236, 354, 472, 708,  944, 1890, 3778 // PAL Based
    };
    #endregion

    #region Properties
    /// <summary>
    /// Indicates if should produce sound
    /// </summary>
    public bool VolumeStart { get; private set; }

    /// <summary>
    /// Changes the Feedback factor for  <see cref="ShiftRegister"/>
    /// </summary>
    public bool Mode { get; private set; }

    /// <summary>
    /// Volume value from the envelope
    /// </summary>
    public bool ConstantVolume { get; private set; }

    /// <summary>
    /// Sample source for constant volume
    /// </summary>
    public byte VolumeReset { get; private set; }

    /// <summary>
    /// divisor of <see cref="VolumeDecay"/>
    /// </summary>
    public byte VolumeDivider { get; private set; }

    /// <summary>
    /// Steps of the volume
    /// </summary>
    public byte VolumeDecay { get; private set; }

    /// <summary>
    /// Checks if sample is necessary
    /// </summary>
    public ushort ShiftRegister { get; private set; }

    private IRandomGenerator Random { get; }
    #endregion

    #region Constructors
    /// <summary>
    /// Instantiates a new <see cref="Noise"/>
    /// </summary>
    /// <param name="amplitude">Initial <see cref="AbstractChannel.Amplitude"/></param>
    /// <param name="random"><see cref="IRandomGenerator"/> for sample generation</param>
    public Noise(double amplitude, IRandomGenerator random)
        : base(amplitude)
    {
        this.Random = random;
        this.ShiftRegister = 1;
    }
    #endregion

    /// <inheritdoc/>
    public override short GetSample()
    {
        if (this.LengthCounter == 0)
        {
            // Length counter mute
            return 0;
        }

        if ((this.ShiftRegister & 0x01) == 0)
        {
            return 0;
        }

        var sampleSource = this.ConstantVolume ? this.VolumeReset : this.VolumeDecay;
        return (short)(this.Random.NextDouble() * this.Amplitude * (sampleSource / 16.0) * ushort.MaxValue);
    }

    /// <inheritdoc/>
    public override void HalfFrame()
    {
        // clock Length counter
        /*
         * https://wiki.nesdev.com/w/index.php/APU_Frame_Counter
         * https://wiki.nesdev.com/w/index.php/APU_Length_Counter
         */
        if (!this.LengthCounterHalt)
        {
            if (this.LengthCounter > 0)
            {
                this.LengthCounter--;
            }
        }
    }

    /// <inheritdoc/>
    public override void QuarterFrame()
    {
        /* same as Pulse */

        // volume envelope
        /* https://wiki.nesdev.com/w/index.php/APU_Envelope */
        if (!this.VolumeStart)
        {
            if (this.VolumeDivider == 0)
            {
                this.VolumeDivider = this.VolumeReset;

                if (this.VolumeDecay == 0 && this.LengthCounterHalt)
                {
                    this.VolumeDecay = 0x0f;
                }
                else if (this.VolumeDecay > 0)
                {
                    this.VolumeDecay--;
                }
            }
            else
            {
                this.VolumeDivider--;
            }
        }
        else
        {
            this.VolumeStart = false;
            this.VolumeDecay = 0x0f;
            this.VolumeDivider = this.VolumeReset;
        }
    }

    /// <summary>
    /// Sets <see cref="Mode"/> based on value
    /// and <see cref="AbstractChannel.Period"/> based on <see cref="PeriodModes"/>
    /// </summary>
    /// <param name="value">Value to base mode and period on</param>
    public void SetModePeriod(byte value)
    {
        this.Mode = (value & 0x80) > 0;
        this.Period = PeriodModes[value & 0x0f];
    }

    /// <summary>
    /// Sets <see cref="AbstractChannel.LengthCounter"/> based on value
    /// and sets <see cref="VolumeStart"/> to true
    /// </summary>
    /// <param name="value">Value to base length on</param>
    public void SetLengthAndStartVolume(byte value)
    {
        this.SetLength((value & 0xf8) >> 3);
        this.VolumeStart = true;
    }

    /// <summary>
    /// Sets <see cref="AbstractChannel.LengthCounterHalt"/>
    /// and <see cref="ConstantVolume"/>
    /// and <see cref="VolumeReset"/> based on value
    /// </summary>
    /// <param name="value">Value to base halt, constant volume and volume reset on</param>
    public void SetHaltAndConstantVolume(byte value)
    {
        this.LengthCounterHalt = (value & 0x20) > 0;
        this.ConstantVolume = (value & 0x10) > 0;
        this.VolumeReset = (byte)(value & 0x0f);
    }

    /// <inheritdoc/>
    protected override void OnTimer0()
    {
        var feedbackFactor = this.Mode ? 6 : 1;

        this.ShiftRegister <<= 1;
        this.ShiftRegister |= (byte)((this.ShiftRegister & 0x01) ^ ((this.ShiftRegister & 0x01) << feedbackFactor));
    }
}
