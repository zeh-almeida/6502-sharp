using Cpu.Extensions;
using System.Text.Json.Serialization;

namespace Cpu.Opcodes;

/// <summary>
/// Implements <see cref="IOpcodeInformation"/>
/// <param name="opcode">Opcode of reference</param>
/// <param name="bytes">Length of the opcode</param>
/// <param name="minimumCycles">Minimum cycles the opcode can take</param>
/// <param name="maximumCycles">Maximum cycles the opcode can take</param>
/// <param name="mnemonic">Visual representation of the opcode in Assembly</param>
/// </summary>
[method: JsonConstructor]
public sealed class OpcodeInformation(
    byte opcode,
    byte bytes,
    byte minimumCycles,
    byte maximumCycles,
    string mnemonic) : IOpcodeInformation
{
    #region Properties
    /// <inheritdoc/>
    public byte Opcode { get; } = opcode;

    /// <inheritdoc/>
    public byte Bytes { get; } = bytes;

    /// <inheritdoc/>
    public byte MinimumCycles { get; } = minimumCycles;

    /// <inheritdoc/>
    public byte MaximumCycles { get; } = maximumCycles > minimumCycles
                                       ? maximumCycles
                                       : minimumCycles;

    /// <inheritdoc/>
    public string Mnemonic { get; } = mnemonic;
    #endregion

    /// <inheritdoc/>
    public bool Equals(IOpcodeInformation? other)
    {
        return other is not null
            && this.Opcode.Equals(other.Opcode);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is IOpcodeInformation info
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
        return $"{this.Opcode.AsHex()}: {this.Mnemonic}";
    }
}
