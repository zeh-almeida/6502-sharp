using Cpu.Extensions;
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
        public string Mnemonic { get; }

        /// <summary>
        /// Representation of the Opcode as string.
        /// Is calculated when constructed.
        /// </summary>
        private string AsString { get; }
        #endregion

        #region Constructors
        /// <summary>
        /// Instantiates a new Opcode Information
        /// </summary>
        /// <param name="opcode">Opcode of reference</param>
        /// <param name="bytes">Length of the opcode</param>
        /// <param name="minimumCycles">Minimum cycles the opcode can take</param>
        /// <param name="maximumCycles">Maximum cycles the opcode can take</param>
        /// <param name="mnemonic">Visual representation of the opcode in Assembly</param>
        [JsonConstructor]
        public OpcodeInformation(
            byte opcode,
            byte bytes,
            byte minimumCycles,
            byte maximumCycles,
            string mnemonic)
        {
            this.Bytes = bytes;
            this.Opcode = opcode;
            this.Mnemonic = mnemonic;

            this.MinimumCycles = minimumCycles;

            this.MaximumCycles = maximumCycles > minimumCycles
                ? maximumCycles
                : minimumCycles;

            this.AsString = $"{this.Opcode.AsHex()}: {this.Mnemonic}";
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
        public override string ToString()
        {
            return this.AsString;
        }
    }
}
