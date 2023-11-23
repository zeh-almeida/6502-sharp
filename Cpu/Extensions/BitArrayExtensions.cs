using System.Collections;
using System.Runtime.CompilerServices;

namespace Cpu.Extensions;

/// <summary>
/// Additional functionality to the <see cref="BitArray"/> class
/// </summary>
public static class BitArrayExtensions
{
    /// <summary>
    /// Transforms the bit array into an 8-bit number
    /// </summary>
    /// <param name="bitArray">Bit array</param>
    /// <returns>8-bit representation</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte AsEightBit(this BitArray bitArray)
    {
        ArgumentNullException.ThrowIfNull(bitArray, nameof(bitArray));

        var array = new byte[((bitArray.Length - 1) / 8) + 1];
        bitArray.CopyTo(array, 0);

        return array[0];
    }

    /// <summary>
    /// Transforms the bit array into an 16-bit number
    /// </summary>
    /// <param name="bitArray">Bit array</param>
    /// <returns>16-bit representation</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ushort AsSixteenBit(this BitArray bitArray)
    {
        ArgumentNullException.ThrowIfNull(bitArray, nameof(bitArray));

        var array = new byte[bitArray.Length];
        bitArray.CopyTo(array, 0);

        return BitConverter.ToUInt16(array, 0);
    }
}
