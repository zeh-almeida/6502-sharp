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
        /// Full assembly name of the Target Instruction of this Opcode
        /// </summary>
        public string? InstructionQualifier { get; }

        /// <summary>
        /// Instruction to be executed by the Opcode
        /// </summary>
        public IInstruction? Instruction { get; }
        #endregion

        /// <summary>
        /// Sets the <see cref="IInstruction"/> reference for this opcode
        /// </summary>
        /// <param name="instruction"> reference</param>
        /// <returns>Current instance</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="instruction"/> is null</exception>
        /// <exception cref="ArgumentException">If reference is already set</exception>
        IOpcodeInformation SetInstruction(IInstruction instruction);
    }
}
