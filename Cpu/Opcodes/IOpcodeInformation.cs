using Cpu.Instructions;

namespace Cpu.Opcodes
{
    /// <summary>
    /// Contains all the necessary information about a instruction such as:
    /// <para>Opcode to execute, number of cycles to execute, bytes to read, if any and the <see cref="IInstruction"/> implementation to execute</para>
    /// </summary>
    public interface IOpcodeInformation
        : IEquatable<IOpcodeInformation>
    {
        #region Properties
        /// <summary>
        /// Value for the instruction Operand code
        /// </summary>
        public byte Opcode { get; }

        /// <summary>
        /// Number of bytes that make the operand for the instruction
        /// </summary>
        public byte Bytes { get; }

        /// <summary>
        /// Minimum amount of Cycles the instruction can take to execute
        /// </summary>
        public byte MinimumCycles { get; }

        /// <summary>
        /// Maximum amount of Cycles the instruction can take to execute
        /// </summary>
        public byte MaximumCycles { get; }

        /// <summary>
        /// Instruction to be executed by the Opcode
        /// </summary>
        public IInstruction? Instruction { get; }
        #endregion
    }
}
