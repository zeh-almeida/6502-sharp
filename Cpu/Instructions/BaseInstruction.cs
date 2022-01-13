using Cpu.Extensions;
using Cpu.Instructions.Exceptions;
using Cpu.Opcodes;
using Cpu.States;

namespace Cpu.Instructions
{
    /// <summary>
    /// Defines common functions of any <see cref="IInstruction"/> implementation
    /// </summary>
    public abstract class BaseInstruction : IInstruction
    {
        #region Properties
        /// <inheritdoc/>
        public IEnumerable<OpcodeInformation> Opcodes { get; }
        #endregion

        #region Constructors
        /// <summary>
        /// Instantiates the base class
        /// </summary>
        /// <param name="opcodeInfos">Allowed opcode and their respective information</param>
        protected BaseInstruction(params OpcodeInformation[] opcodeInfos)
        {
            this.Opcodes = opcodeInfos
                .Select(info => info.SetInstruction(this))
                .ToHashSet();
        }
        #endregion

        /// <inheritdoc/>
        public abstract void Execute(ICpuState currentState, ushort value);

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return this.Opcodes.GetHashCode();
        }

        /// <inheritdoc/>
        public override bool Equals(object? other)
        {
            return other is BaseInstruction instruction
                && this.Equals(instruction);
        }

        /// <inheritdoc/>
        public bool Equals(IInstruction? other)
        {
            return other is not null
                && this.Opcodes.Equals(other.Opcodes);
        }

        /// <inheritdoc/>
        public bool HasOpcode(byte opcode)
        {
            return this.Opcodes
                .Any(info => info.Opcode.Equals(opcode));
        }

        /// <inheritdoc/>
        public OpcodeInformation GatherInformation(byte opcode)
        {
            var result = this.Opcodes
                .FirstOrDefault(info => info.Opcode.Equals(opcode));

            return result
                ?? throw new UnknownOpcodeException(opcode, $"{this.GetType().Name} does not handle opcode {opcode.AsHex()}");
        }
    }
}
