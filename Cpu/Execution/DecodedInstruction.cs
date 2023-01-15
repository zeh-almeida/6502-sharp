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
        /// Value for the instruction parameter
        /// </summary>
        public ushort ValueParameter { get; }

        /// <summary>
        /// Decoded <see cref="IInstruction"/>
        /// </summary>
        public IInstruction Instruction { get; }

        /// <summary>
        /// <see cref="IOpcodeInformation"/> about the instruction
        /// </summary>
        public IOpcodeInformation Information { get; }
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
            this.Information = opcodeInformation;
            this.ValueParameter = valueParameter;
            this.Instruction = instruction;
        }
        #endregion

        /// <inheritdoc/>
        public override string ToString()
        {
            var value = this.Information.Bytes > 2
                ? this.ValueParameter.AsAssembly()
                : ((byte)this.ValueParameter).AsAssembly();

            return this.Information.ToString().Replace("oper", value);
        }
    }
}
