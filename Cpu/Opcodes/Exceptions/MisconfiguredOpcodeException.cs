namespace Cpu.Opcodes.Exceptions;

/// <summary>
/// Represents errors that occur when an instruction is asked to run an incompatible opcode.
/// </summary>
/// <remarks>
/// Initializes a new instance of the Cpu.Opcodes.Exceptions.MisconfiguredOpcodeException class
/// with the offending opcode.
/// </remarks>
public sealed class MisconfiguredOpcodeException(string opcodeName) : Exception
{
    #region Properties
    /// <summary>
    /// Opcode which caused the error
    /// </summary>
    public string OpcodeName { get; } = opcodeName;

    /// <inheritdoc/>
    public override string Message => $"Opcode configuration='{this.OpcodeName}' is malformed";
    #endregion
}
