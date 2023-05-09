namespace Cpu.Opcodes.Exceptions;

/// <summary>
/// Represents errors that occur when an instruction is asked to run an incompatible opcode.
/// </summary>
public sealed class MisconfiguredOpcodeException : Exception
{
    #region Properties
    /// <summary>
    /// Opcode which caused the error
    /// </summary>
    public string OpcodeName { get; }

    /// <inheritdoc/>
    public override string Message => $"Opcode configuration='{this.OpcodeName}' is malformed";
    #endregion

    #region Constructors
    /// <summary>
    /// Initializes a new instance of the Cpu.Opcodes.Exceptions.MisconfiguredOpcodeException class
    /// with the offending opcode.
    /// </summary>
    /// <param name="opcodeName">name of the resource not configured</param>
    public MisconfiguredOpcodeException(string opcodeName)
    {
        this.OpcodeName = opcodeName;
    }
    #endregion
}
