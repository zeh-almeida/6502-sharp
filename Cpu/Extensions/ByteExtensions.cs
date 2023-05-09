namespace Cpu.Extensions;

/// <summary>
/// Additional functionality to the <see cref="byte"/> struct
/// </summary>
public static class ByteExtensions
{
    /// <summary>
    /// Returns the 4 least significant bits from a 8-bit value.
    /// The first 4 bits will be zero.
    /// </summary>
    /// <param name="value">8-bit value</param>
    /// <returns>Least significant digits</returns>
    public static byte LeastSignificantBits(this byte value)
    {
        return (byte)(value & 0x0F);
    }

    /// <summary>
    /// Returns the 4 most significant bits from a 8-bit value.
    /// The last 4 bits will be zero.
    /// </summary>
    /// <param name="value">8-bit value</param>
    /// <returns>Most significant digits</returns>
    public static byte MostSignificantBits(this byte value)
    {
        return (byte)(value & 0xF0);
    }

    /// <summary>
    /// Combines the least and most significant bits into a single 8-bit number
    /// </summary>
    /// <param name="lsb">Least significant bits</param>
    /// <param name="msb">Most significant bits</param>
    /// <returns>Combined 8-bit value</returns>
    public static byte CombineSignificantBits(this byte lsb, byte msb)
    {
        return (byte)(msb | lsb);
    }

    /// <summary>
    /// Combine two 8-bit numbers into a 16-bit number
    /// </summary>
    /// <param name="lsb">Least significant bits</param>
    /// <param name="msb">Most significant bits</param>
    /// <returns>Combined 16-bit number</returns>
    public static ushort CombineBytes(this byte lsb, byte msb)
    {
        return (ushort)((msb << 8) | lsb);
    }

    /// <summary>
    /// Checks if the first bit of an 8-bit number is set.
    /// </summary>
    /// <param name="value">Value to check</param>
    /// <returns>True if bit 0 is set, false otherwise</returns>
    public static bool IsFirstBitSet(this byte value)
    {
        return value.IsBitSet(0);
    }

    /// <summary>
    /// Checks if the last bit of an 8-bit number is set.
    /// </summary>
    /// <param name="value">Value to check</param>
    /// <returns>True if bit 7 is set, false otherwise</returns>
    public static bool IsLastBitSet(this byte value)
    {
        return value.IsBitSet(7);
    }

    /// <summary>
    /// Checks if the desired bit of an 8-bit number is set.
    /// </summary>
    /// <param name="value">Value to check</param>
    /// <param name="index">Index of the bit to check</param>
    /// <returns>True if the bit is set, false otherwise</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the index is less than 0 or bigger than 7</exception>
    public static bool IsBitSet(this byte value, int index)
    {
        if (index is < 0 or >= 8)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Index must be between 0-7");
        }

        var mask = index switch
        {
            0 => 0b_0000_0001,
            1 => 0b_0000_0010,
            2 => 0b_0000_0100,
            3 => 0b_0000_1000,
            4 => 0b_0001_0000,
            5 => 0b_0010_0000,
            6 => 0b_0100_0000,
            _ => 0b_1000_0000,
        };

        return mask.Equals(value & mask);
    }

    /// <summary>
    /// Checks if the byte is zero
    /// </summary>
    /// <param name="value">Value to check</param>
    /// <returns>True if zero, false otherwise</returns>
    public static bool IsZero(this byte value)
    {
        return 0.Equals(value);
    }

    /// <summary>
    /// Rotates the bits to the right.
    /// If carry is set, sets the last bit of the new number.
    /// </summary>
    /// <param name="value">Value to shift</param>
    /// <param name="isCarry">If true, sets the last bit</param>
    /// <returns>Shifted value</returns>
    public static byte RotateRight(this byte value, bool isCarry)
    {
        var shiftedValue = value >> 1;
        var carryMask = isCarry ? 0b_1000_0000 : 0b_0000_0000;

        return (byte)(shiftedValue | carryMask);
    }

    /// <summary>
    /// Rotates the bits to the left.
    /// If carry is set, sets the first bit of the new number.
    /// </summary>
    /// <param name="value">Value to shift</param>
    /// <param name="isCarry">If true, sets the first bit</param>
    /// <returns>Shifted value</returns>
    public static byte RotateLeft(this byte value, bool isCarry)
    {
        var shiftedValue = value << 1;
        var carryMask = isCarry.AsBinary();

        return (byte)(shiftedValue | carryMask);
    }

    /// <summary>
    /// Breaks the byte into its least and most significant values.
    /// The numbers are ordered in little endian fashion
    /// </summary>
    /// <param name="value">Vale to break down</param>
    /// <returns>least and most significant value pair</returns>
    public static (byte lsb, byte msb) SignificantBits(this byte value)
    {
        var lsb = value.LeastSignificantBits();
        var msb = value.MostSignificantBits();

        return (lsb, msb);
    }

    /// <summary>
    /// Converts a BCD byte to a Hex byte
    /// </summary>
    /// <param name="value">BCD byte to convert</param>
    /// <returns>Hex byte</returns>
    /// <see href="https://github.com/amensch/e6502/blob/master/e6502CPU/Utility/CPUMath.cs"/>
    public static byte ToHex(this byte value)
    {
        return value <= 9
             ? value
             : (byte)(((value / 10) << 4) + (value % 10));
    }

    /// <summary>
    /// Converts a Hex byte to a BCD byte
    /// </summary>
    /// <param name="value">Hex byte to convert</param>
    /// <returns>BCD byte</returns>
    /// <see href="https://github.com/amensch/e6502/blob/master/e6502CPU/Utility/CPUMath.cs"/>
    public static byte ToBCD(this byte value)
    {
        return (value & 0x0f) > 0x09
            ? throw new InvalidOperationException($"Invalid BCD number: {value:X2}")
            : (byte)(((value >> 4) * 10) + (value & 0x0f));
    }

    /// <summary>
    /// Formats the byte as an hexadecimal value, 0x00
    /// </summary>
    /// <param name="opcode"> opcode to format</param>
    /// <returns>Formatted value</returns>
    public static string AsHex(this byte opcode)
    {
        return $"0x{opcode:X2}";
    }

    /// <summary>
    /// Formats the byte as an assembly hexadecimal value, $00
    /// </summary>
    /// <param name="opcode"> opcode to format</param>
    /// <returns>Formatted value</returns>
    public static string AsAssembly(this byte opcode)
    {
        return $"${opcode:X2}";
    }
}
