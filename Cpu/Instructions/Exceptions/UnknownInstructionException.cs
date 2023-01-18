namespace Cpu.Instructions.Exceptions;

/// <summary>
/// Represents errors that occur when an instruction cannot be found.
/// </summary>
public sealed class UnknownInstructionException : Exception
{
    #region Properties
    /// <summary>
    /// Opcode which caused the error
    /// </summary>
    public string InstructionName { get; }

    /// <inheritdoc/>
    public override string Message => base.Message + $", Instruction={this.InstructionName}";
    #endregion

    #region Constructors
    /// <summary>
    /// Initializes a new instance of the Cpu.Instructions.Exceptions.UnknownInstructionException class
    /// with the offending instruction name.
    /// </summary>
    /// <param name="instructionName">Instruction name which caused the error</param>
    public UnknownInstructionException(string instructionName)
        : base()
    {
        this.InstructionName = instructionName;
    }

    /// <summary>
    /// Initializes a new instance of the Cpu.Instructions.Exceptions.UnknownInstructionException class
    /// with the offending instruction name and a dedicated message.
    /// </summary>
    /// <param name="instructionName">Instruction name which caused the error</param>
    /// <param name="message">details of the error</param>
    public UnknownInstructionException(string instructionName, string message)
        : base(message)
    {
        this.InstructionName = instructionName;
    }
    #endregion
}
