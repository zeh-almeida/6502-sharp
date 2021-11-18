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
        /// Number of Cycles to run the instruction
        /// </summary>
        /// <see cref="OpcodeInformation.Cycles"/>
        public byte Cycles => this.OpcodeInformation.Cycles;

        /// <summary>
        /// Number of bytes that make the operand for the instruction
        /// </summary>
        /// <see cref="OpcodeInformation.Bytes"/>
        public byte Bytes => this.OpcodeInformation.Bytes;

        /// <summary>
        /// Byte representing the instruction to be executed
        /// </summary>
        /// <see cref="OpcodeInformation.Opcode"/>
        public byte Opcode => this.OpcodeInformation.Opcode;

        /// <summary>
        /// Decoded <see cref="IInstruction"/>
        /// </summary>
        /// <see cref="OpcodeInformation.Instruction"/>
        public IInstruction Instruction => this.OpcodeInformation.Instruction;

        /// <summary>
        /// Contains all information regarding the decoded Opcode
        /// </summary>
        private OpcodeInformation OpcodeInformation { get; }
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
            this.OpcodeInformation = opcodeInformation;
        }
        #endregion
    }
}
