using Cpu.Extensions;
using System.Collections;

namespace Cpu.Flags;

/// <summary>
/// Implements the <see cref="IFlagManager"/> interface to manipulate flags
/// </summary>
public sealed record FlagManager : IFlagManager
{
    #region Properties
    /// <inheritdoc/>
    public bool IsCarry { get; set; }

    /// <inheritdoc/>
    public bool IsZero { get; set; }

    /// <inheritdoc/>
    public bool IsInterruptDisable { get; set; }

    /// <inheritdoc/>
    public bool IsDecimalMode { get; set; }

    /// <inheritdoc/>
    public bool IsBreakCommand { get; set; }

    /// <inheritdoc/>
    public bool IsOverflow { get; set; }

    /// <inheritdoc/>
    public bool IsNegative { get; set; }
    #endregion

    /// <inheritdoc/>
    #region Save/Load
    public byte Save()
    {
        var bits = new BitArray(new bool[]
        {
            this.IsCarry,
            this.IsZero,
            this.IsInterruptDisable,
            this.IsDecimalMode,
            this.IsBreakCommand,
            this.IsOverflow,
            this.IsNegative,
        });

        return bits.AsEightBit();
    }

    /// <inheritdoc/>
    public void Load(in byte value)
    {
        var bits = new BitArray(new int[] { value });

        this.IsCarry = bits[0];
        this.IsZero = bits[1];
        this.IsInterruptDisable = bits[2];
        this.IsDecimalMode = bits[3];
        this.IsBreakCommand = bits[4];
        this.IsOverflow = bits[5];
        this.IsNegative = bits[6];
    }
    #endregion

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"N:{this.IsNegative.AsBinary()};Z:{this.IsZero.AsBinary()};C:{this.IsCarry.AsBinary()};I:{this.IsInterruptDisable.AsBinary()};D:{this.IsDecimalMode.AsBinary()};V:{this.IsOverflow.AsBinary()}";
    }
}
