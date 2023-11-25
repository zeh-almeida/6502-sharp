using CommunityToolkit.Diagnostics;
using Cpu.Extensions;

namespace Cpu.Registers;

/// <summary>
/// Implements <see cref="IRegisterManager"/> to manipulate the CPU state
/// </summary>
public sealed record RegisterManager : IRegisterManager
{
    #region Constants
    private const int StateLength = 6;
    #endregion

    #region Properties
    /// <inheritdoc/>
    public ushort ProgramCounter { get; set; }

    /// <inheritdoc/>
    public byte StackPointer { get; set; }

    /// <inheritdoc/>
    public byte Accumulator { get; set; }

    /// <inheritdoc/>
    public byte IndexX { get; set; }

    /// <inheritdoc/>
    public byte IndexY { get; set; }
    #endregion

    #region Load/Save
    /// <inheritdoc/>
    public ReadOnlyMemory<byte> Save()
    {
        (var lsb, var msb) = this.ProgramCounter.SignificantBits();

        return new byte[]
        {
            lsb,
            msb,
            this.StackPointer,
            this.Accumulator,
            this.IndexX,
            this.IndexY,
        };
    }

    /// <inheritdoc/>
    public void Load(in ReadOnlyMemory<byte> data)
    {
        Guard.IsEqualTo(data.Length, StateLength, nameof(data));

        var span = data.Span;

        this.ProgramCounter = span[0].CombineBytes(span[1]);
        this.StackPointer = span[2];
        this.Accumulator = span[3];
        this.IndexX = span[4];
        this.IndexY = span[5];
    }
    #endregion

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"PC:{this.ProgramCounter.AsHex()};SP:{this.StackPointer.AsHex()};A:{this.Accumulator.AsHex()};X:{this.IndexX.AsHex()};Y:{this.IndexY.AsHex()}";
    }
}
