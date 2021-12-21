using Cpu.Instructions;

namespace Cpu.Opcodes
{
    /// <summary>
    /// Contains all the necessary information about a instruction such as:
    /// <para>Opcode to execute, number of cycles to execute, bytes to read, if any and the <see cref="IInstruction"/> implementation to execute</para>
    /// </summary>
    public sealed class OpcodeInformation : IEquatable<OpcodeInformation>
    {
        #region Properties
        /// <summary>
        /// Value for the instruction Operand code
        /// </summary>
        public byte Opcode { get; }

        /// <summary>
        /// Number of Cycles to run the instruction
        /// </summary>
        public byte Cycles { get; }

        /// <summary>
        /// Number of bytes that make the operand for the instruction
        /// </summary>
        public byte Bytes { get; }

        /// <summary>
        /// Instruction to be executed by the Opcode
        /// </summary>
        public IInstruction Instruction { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Instantiates a new Opcode Information
        /// </summary>
        /// <param name="opcode">Opcode of reference</param>
        /// <param name="cycles">Cycles of the opcode</param>
        /// <param name="bytes">Length of the opcode</param>
        public OpcodeInformation(
            byte opcode,
            byte cycles,
            byte bytes)
        {
            this.Bytes = bytes;
            this.Cycles = cycles;
            this.Opcode = opcode;
        }
        #endregion

        /// <inheritdoc/>
        public bool Equals(OpcodeInformation other)
        {
            return this.Opcode.Equals(other.Opcode);
        }

        /// <inheritdoc/>
        public override bool Equals(object other)
        {
            return other is OpcodeInformation info
                && this.Equals(info);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return this.Opcode.GetHashCode();
        }

        /// <summary>
        /// Sets the <see cref="IInstruction"/> reference for this opcode
        /// </summary>
        /// <param name="instruction"> reference</param>
        /// <returns>Current instance</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="instruction"/> is null</exception>
        /// <exception cref="ArgumentException">If reference is already set</exception>
        public OpcodeInformation SetInstruction(IInstruction instruction)
        {
            if (instruction is null)
            {
                throw new ArgumentNullException(nameof(instruction), "Cannot be null");
            }

            if (this.Instruction is not null)
            {
                throw new ArgumentException("Instruction is already set", nameof(instruction));
            }

            this.Instruction = instruction;
            return this;
        }
    }
}
