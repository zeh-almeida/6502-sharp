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
        #endregion

        #region Constructors
        /// <summary>
        /// Instantiates a new Opcode Information
        /// </summary>
        /// <param name="opcode">Opcode of reference</param>
        /// <param name="bytes">Length of the opcode</param>
        /// <param name="minimumCycles">Minimum cycles the opcode can take</param>
        /// <param name="maximumCycles">Maximum cycles the opcode can take</param>
        [JsonConstructor]
        public OpcodeInformation(
            byte opcode,
            byte bytes,
            byte minimumCycles,
            byte maximumCycles)
        {
            this.Bytes = bytes;
            this.Opcode = opcode;

            this.MinimumCycles = minimumCycles;

            this.MaximumCycles = maximumCycles > minimumCycles
                ? maximumCycles
                : minimumCycles;
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
    }
}
