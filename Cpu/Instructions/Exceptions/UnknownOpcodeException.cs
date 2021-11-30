using Cpu.Extensions;
using System;

namespace Cpu.Instructions.Exceptions
{
    /// <summary>
    /// Represents errors that occur when an instruction is asked to run an incompatible opcode.
    /// </summary>
    public sealed class UnknownOpcodeException : Exception
    {
        #region Properties
        /// <summary>
        /// Opcode which caused the error
        /// </summary>
        public byte UnknownOpcode { get; }

        /// <inheritdoc/>
        public override string Message => base.Message + $", OP Code={UShortExtensions.AsHex(this.UnknownOpcode)}";
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the Cpu.Instructions.Exceptions.UnknownOpcodeException class
        /// with the offending opcode.
        /// </summary>
        /// <param name="unknownOpcode">Opcode which caused the error</param>
        public UnknownOpcodeException(byte unknownOpcode)
            : base()
        {
            this.UnknownOpcode = unknownOpcode;
        }

        /// <summary>
        /// Initializes a new instance of the Cpu.Instructions.Exceptions.UnknownOpcodeException class
        /// with the offending opcode and a dedicated message.
        /// </summary>
        /// <param name="unknownOpcode">Opcode which caused the error</param>
        /// <param name="message">details of the error</param>
        public UnknownOpcodeException(byte unknownOpcode, string message)
            : base(message)
        {
            this.UnknownOpcode = unknownOpcode;
        }
        #endregion
    }
}
