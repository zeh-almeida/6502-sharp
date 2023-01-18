using Microsoft.Extensions.Logging;

namespace Cpu.Execution;

/// <summary>
/// Defines the <see cref="EventId"/> used to log <see cref="Machine"/> operations
/// </summary>
internal static class MachineEvents
{
    /// <summary>
    /// Signifies data being load into the machine, like a new program
    /// </summary>
    public static EventId OnLoadData { get; } = new EventId(1000, "Loading machine data");

    /// <summary>
    /// Signifies generation of a snapshot of the machine
    /// </summary>
    public static EventId OnSaveData { get; } = new EventId(1100, "Saving machine data");

    /// <summary>
    /// Signifies the execution of a interrupt, software or hardware
    /// </summary>
    public static EventId OnInterrupt { get; } = new EventId(1200, "Processing interrupt");

    /// <summary>
    /// Signifies the result of a opcode decoding process
    /// </summary>
    public static EventId OnDecode { get; } = new EventId(1300, "Decoded Instruction");

    /// <summary>
    /// Signifies the current state of the flags
    /// </summary>
    public static EventId OnFlags { get; } = new EventId(1400, "Flag result");

    /// <summary>
    /// Signifies the current state of the registers
    /// </summary>
    public static EventId OnRegisters { get; } = new EventId(1500, "Register result");

    /// <summary>
    /// Signifies a clock execution
    /// </summary>
    public static EventId OnExecute { get; } = new EventId(1600, "Execution");
}
