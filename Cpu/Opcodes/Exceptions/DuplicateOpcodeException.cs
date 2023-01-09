using Cpu.Extensions;

namespace Cpu.Opcodes.Exceptions
{
    /// <summary>
    /// Represents errors that occur when an instruction is asked to run an incompatible opcode.
    /// </summary>
    public sealed class DuplicateOpcodeException : Exception
    {
        #region Properties
        /// <summary>
        /// Opcode which caused the error
        /// </summary>
        public byte UnknownOpcode { get; }

        /// <inheritdoc/>
        public override string Message => $"OP Code='{UShortExtensions.AsHex(this.UnknownOpcode)}' is duplicated";
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the Cpu.Opcodes.Exceptions.DuplicateOpcodeException class
        /// with the offending opcode.
        /// </summary>
        /// <param name="unknownOpcode">Opcode which caused the error</param>
        public DuplicateOpcodeException(byte unknownOpcode)
            : base()
        {
            this.UnknownOpcode = unknownOpcode;
        }
        #endregion
    }
}
