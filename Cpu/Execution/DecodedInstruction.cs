using Cpu.Extensions;
using Cpu.Instructions;
using Cpu.Opcodes;

namespace Cpu.Execution
{
    /// <summary>
    /// Contains information about a decoded instruction
    /// </summary>
    public sealed record DecodedInstruction
    {
        #region Properties
        /// <summary>
        /// Value for the instruction Operand code
        /// </summary>
        public ushort ValueParameter { get; }

        /// <summary>
        /// Minimum number of Cycles to run the instruction
        /// </summary>
        /// <see cref="IOpcodeInformation.MinimumCycles"/>
        public byte Cycles { get; }

        /// <summary>
        /// Number of bytes that make the operand for the instruction
        /// </summary>
        /// <see cref="IOpcodeInformation.Bytes"/>
        public byte Bytes { get; }

        /// <summary>
        /// Byte representing the instruction to be executed
        /// </summary>
        /// <see cref="IOpcodeInformation.Opcode"/>
        public byte Opcode { get; }

        /// <summary>
        /// Decoded <see cref="IInstruction"/>
        /// </summary>
        public IInstruction Instruction { get; }
        #endregion

        #region Constructors
        /// <summary>
        /// Instantiates a new container for the decoded information
        /// </summary>
        /// <param name="opcodeInformation">Information about the decoded opcode, size and cycles</param>
        /// <param name="instruction">Instruction representation of the opcode</param>
        /// <param name="valueParameter">Value to use as operand for the decoded instruction</param>
        public DecodedInstruction(
            IOpcodeInformation opcodeInformation,
            IInstruction instruction,
            ushort valueParameter)
        {
            this.ValueParameter = valueParameter;
            this.Instruction = instruction;

            this.Bytes = opcodeInformation.Bytes;
            this.Opcode = opcodeInformation.Opcode;
            this.Cycles = opcodeInformation.MinimumCycles;
        }
        #endregion

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{this.Opcode.AsHex()} ({this.ValueParameter.AsHex()})";
        }
    }
}
