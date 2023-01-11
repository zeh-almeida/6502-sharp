using Cpu.Instructions;
using Cpu.Instructions.Exceptions;
using System.Text.Json.Serialization;

namespace Cpu.Opcodes
{
    /// <summary>
    /// Implements <see cref="IOpcodeInformation"/>
    /// </summary>
    public sealed class OpcodeInformation : IOpcodeInformation
    {
        #region Properties
        /// <inheritdoc/>
        public byte Opcode { get; }

        /// <inheritdoc/>
        public byte Bytes { get; }

        /// <inheritdoc/>
        public byte MinimumCycles { get; }

        /// <inheritdoc/>
        public byte MaximumCycles { get; }

        /// <inheritdoc/>
        public string? InstructionQualifier { get; }

        /// <inheritdoc/>
        public IInstruction? Instruction { get; private set; }
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
            this.Opcode = opcode;

            this.MinimumCycles = cycles;
            this.MaximumCycles = cycles;

            this.InstructionQualifier = null;
        }

        /// <summary>
        /// Instantiates a new Opcode Information
        /// </summary>
        /// <param name="opcode">Opcode of reference</param>
        /// <param name="bytes">Length of the opcode</param>
        /// <param name="instructionQualifier">Name of the instruction associated with the opcode</param>
        /// <param name="minimumCycles">Minimum cycles the opcode can take</param>
        /// <param name="maximumCycles">Maximum cycles the opcode can take</param>
        [JsonConstructor]
        public OpcodeInformation(
            byte opcode,
            byte bytes,
            string instructionQualifier,
            byte minimumCycles,
            byte maximumCycles)
        {
            ArgumentException.ThrowIfNullOrEmpty(instructionQualifier, nameof(instructionQualifier));

            this.Bytes = bytes;
            this.Opcode = opcode;

            this.MinimumCycles = minimumCycles;

            this.MaximumCycles = maximumCycles > minimumCycles
                ? maximumCycles
                : minimumCycles;

            this.InstructionQualifier = instructionQualifier;
            this.ApplyInstruction();
        }
        #endregion

        /// <inheritdoc/>
        public bool Equals(IOpcodeInformation? other)
        {
            return other is not null
                && this.Opcode.Equals(other.Opcode);
        }

        /// <inheritdoc/>
        public override bool Equals(object? other)
        {
            return other is OpcodeInformation info
                && this.Equals(info);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return this.Opcode.GetHashCode();
        }

        /// <inheritdoc/>
        public IOpcodeInformation SetInstruction(IInstruction instruction)
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

        private void ApplyInstruction()
        {
            if (string.IsNullOrWhiteSpace(this.InstructionQualifier))
            {
                throw new Exception("Could not qualify instruction");
            }

            var target = this.GetType().Assembly.FullName
                ?? throw new Exception("Could not qualify assembly");

            var handle = Activator.CreateInstance(target, this.InstructionQualifier)
                ?? throw new UnknownInstructionException(this.InstructionQualifier);

            if (handle.Unwrap() is not IInstruction instruction)
            {
                throw new UnknownInstructionException(this.InstructionQualifier);
            }

            _ = this.SetInstruction(instruction);
        }
    }
}
