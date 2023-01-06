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
        /// <see cref="OpcodeInformation.MinimumCycles"/>
        public byte Cycles { get; }

        /// <summary>
        /// Number of bytes that make the operand for the instruction
        /// </summary>
        /// <see cref="OpcodeInformation.Bytes"/>
        public byte Bytes { get; }

        /// <summary>
        /// Byte representing the instruction to be executed
        /// </summary>
        /// <see cref="OpcodeInformation.Opcode"/>
        public byte Opcode { get; }

        /// <summary>
        /// Decoded <see cref="IInstruction"/>
        /// </summary>
        /// <see cref="OpcodeInformation.Instruction"/>
        public IInstruction Instruction { get; }
        #endregion

        #region Constructors
        /// <summary>
        /// Instantiates a new container for the decoded information
        /// </summary>
        /// <param name="opcodeInformation">Information about the decoded opcode, including instruction, size and cycles</param>
        /// <param name="valueParameter">Value to use as operand for the decoded instruction</param>
        public DecodedInstruction(
            OpcodeInformation opcodeInformation,
            ushort valueParameter)
        {
            this.ValueParameter = valueParameter;

            if (opcodeInformation.Instruction is null)
            {
                throw new ArgumentNullException(nameof(opcodeInformation), "Instruction is null");
            }

            this.Bytes = opcodeInformation.Bytes;
            this.Opcode = opcodeInformation.Opcode;
            this.Cycles = opcodeInformation.MinimumCycles;
            this.Instruction = opcodeInformation.Instruction;
        }
        #endregion

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{this.Opcode.AsHex()} ({this.ValueParameter.AsHex()})";
        }
    }
}
