using Cpu.States;

namespace NesApu;

/// <summary>
/// Allows manipulation and execution of audio operations for the NES console
/// </summary>
public interface IApu
{
    #region Constants
    /// <summary>
    /// Memory address for the Frame Counter data
    /// <see href="https://wiki.nesdev.org/w/index.php?title=APU#Frame_Counter_.28.244017.29"/>
    /// </summary>
    public const ushort FrameCounterAddress = 4017;
    #endregion

    #region Properties
    /// <summary>
    /// Type of sequencer used aby the APU at the moment
    /// </summary>
    public SequencerMode SequencerMode { get; }

    /// <summary>
    /// Flgas with an IRQ must be executed
    /// </summary>
    public bool IsIrqDisable { get; }

    /// <summary>
    /// TODO
    /// </summary>
    public bool IsFrameInterrupt { get; }

    /// <summary>
    /// Current count of APU cycles
    /// </summary>
    public int Cycles { get; }

    /// <summary>
    /// Current audio aplitude
    /// </summary>
    public double Amplitude { get; set; }
    #endregion

    /// <summary>
    /// Cycles the APU to process audio data
    /// </summary>
    /// <param name="cpuState"></param>
    void Cycle(ICpuState cpuState);
}