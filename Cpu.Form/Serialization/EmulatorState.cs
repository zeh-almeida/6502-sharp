namespace Cpu.Forms.Serialization;

/// <summary>
/// Defines the current state of the Emulator
/// </summary>
public sealed record EmulatorState
{
    #region Properties
    /// <summary>
    /// Path to the program being executed by the Emulator
    /// </summary>
    public string ProgramPath { get; set; }

    /// <summary>
    /// State of the emulators memory
    /// </summary>
    public ReadOnlyMemory<byte> State { get; set; }
    #endregion

    #region Constructors
    /// <summary>
    /// Instantiates a new EmulatorState
    /// </summary>
    public EmulatorState()
    {
        this.ProgramPath = string.Empty;
        this.State = Array.Empty<byte>();
    }
    #endregion
}
