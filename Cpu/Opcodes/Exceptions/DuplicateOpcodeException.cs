using Cpu.Extensions;

namespace Cpu.Opcodes.Exceptions;

/// <summary>
/// Represents errors that occur when an instruction is asked to run an incompatible opcode.
/// </summary>
/// <remarks>
/// Initializes a new instance of the Cpu.Opcodes.Exceptions.DuplicateOpcodeException class
/// with the offending opcode.
/// </remarks>
public sealed class DuplicateOpcodeException(byte unknownOpcode) : Exception()
{
    #region Properties
    /// <summary>
    /// Opcode which caused the error
    /// </summary>
    public byte UnknownOpcode { get; } = unknownOpcode;

    /// <inheritdoc/>
    public override string Message => $"OP Code='{UShortExtensions.AsHex(this.UnknownOpcode)}' is duplicated";
    #endregion
}
