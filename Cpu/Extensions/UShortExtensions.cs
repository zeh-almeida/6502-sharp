namespace Cpu.Extensions;

/// <summary>
/// Additional functionality to the <see cref="ushort"/> struct
/// </summary>
public static class UShortExtensions
{
    /// <summary>
    /// Returns the 8 least significant bits from a 16-bit value.
    /// The first 8 bits will be zero.
    /// </summary>
    /// <param name="value">16-bit value</param>
    /// <returns>Least significant digits</returns>
    public static ushort LeastSignificantBits(this ushort value)
    {
        return (ushort)(value & 0x00FF);
    }

    /// <summary>
    /// Returns the 8 most significant bits from a 16-bit value.
    /// The last 8 bits will be zero.
    /// </summary>
    /// <param name="value">16-bit value</param>
    /// <returns>Most significant digits</returns>
    public static ushort MostSignificantBits(this ushort value)
    {
        return (ushort)(value & 0xFF00);
    }

    /// <summary>
    /// Combines the least and most significant bits into a single 16-bit number
    /// </summary>
    /// <param name="lsb">Least significant bits</param>
    /// <param name="msb">Most significant bits</param>
    /// <returns>Combined 16-bit value</returns>
    public static ushort CombineSignificantBits(this ushort lsb, in ushort msb)
    {
        return (ushort)(msb | lsb);
    }

    /// <summary>
    /// Checks if the first bit of an 16-bit number is set.
    /// </summary>
    /// <param name="value">Value to check</param>
    /// <returns>True if bit 0 is set, false otherwise</returns>
    public static bool IsFirstBitSet(this ushort value)
    {
        return value.IsBitSet(0);
    }

    /// <summary>
    /// Checks if the seventh bit of an 16-bit number is set.
    /// This is the same as checking the last bit of an 8-bit number.
    /// </summary>
    /// <param name="value">Value to check</param>
    /// <returns>True if bit 7 is set, false otherwise</returns>
    /// <see cref="ByteExtensions.IsLastBitSet(byte)"/>
    public static bool IsSeventhBitSet(this ushort value)
    {
        return value.IsBitSet(7);
    }

    /// <summary>
    /// Checks if the desired bit of an 16-bit number is set.
    /// </summary>
    /// <param name="value">Value to check</param>
    /// <param name="index">Index of the bit to check</param>
    /// <returns>True if the bit is set, false otherwise</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the index is less than 0 or bigger than 15</exception>
    public static bool IsBitSet(this ushort value, in int index)
    {
        if (index is < 0 or >= 16)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Index must be between 0-15");
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
            7 => 0b_1000_0000,

            8 => 0b_0000_0001_0000_0000,
            9 => 0b_0000_0010_0000_0000,
            10 => 0b_0000_0100_0000_0000,
            11 => 0b_0000_1000_0000_0000,
            12 => 0b_0001_0000_0000_0000,
            13 => 0b_0010_0000_0000_0000,
            14 => 0b_0100_0000_0000_0000,
            _ => 0b_1000_0000_0000_0000,
        };

        return mask.Equals(value & mask);
    }

    /// <summary>
    /// Checks if the 16-bit is zero
    /// </summary>
    /// <param name="value">Value to check</param>
    /// <returns>True if zero, false otherwise</returns>
    public static bool IsZero(this ushort value)
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
    public static ushort RotateRight(this ushort value, in bool isCarry)
    {
        var shiftedValue = value >> 1;
        var carryMask = isCarry ? 0b_1000_0000_0000_0000 : 0b_0000_0000_0000_0000;

        return (ushort)(shiftedValue | carryMask);
    }

    /// <summary>
    /// Rotates the bits to the left.
    /// If carry is set, sets the first bit of the new number.
    /// </summary>
    /// <param name="value">Value to shift</param>
    /// <param name="isCarry">If true, sets the first bit</param>
    /// <returns>Shifted value</returns>
    public static ushort RotateLeft(this ushort value, in bool isCarry)
    {
        var shiftedValue = value << 1;
        var carryMask = isCarry.AsBinary();

        return (ushort)(shiftedValue | carryMask);
    }

    /// <summary>
    /// Breaks the 16-bit into its least and most significant values.
    /// The numbers are ordered in little endian fashion
    /// </summary>
    /// <param name="value">Vale to break down</param>
    /// <returns>least and most significant value pair</returns>
    public static (byte lsb, byte msb) SignificantBits(this ushort value)
    {
        var lsb = (byte)LeastSignificantBits(value);
        var msb = (byte)(MostSignificantBits(value) >> 8);

        return (lsb, msb);
    }

    /// <summary>
    /// Formats the byte as an hexadecimal value, 0x0000
    /// </summary>
    /// <param name="opcode"> opcode to format</param>
    /// <returns>Formatted value</returns>
    public static string AsHex(this ushort opcode)
    {
        return $"0x{opcode:X4}";
    }

    /// <summary>
    /// Formats the byte as an assembly hexadecimal value, $0000
    /// </summary>
    /// <param name="opcode"> opcode to format</param>
    /// <returns>Formatted value</returns>
    public static string AsAssembly(this ushort opcode)
    {
        return $"${opcode:X4}";
    }

    /// <summary>
    /// Calculates the branch jump address.
    /// Jumps must be signed so a two-complement subtraction is performed
    /// </summary>
    /// <param name="value">Value to calculate from</param>
    /// <param name="offset">Value to offset to</param>
    /// <returns>Jump address</returns>
    public static ushort BranchAddress(this ushort value, in byte offset)
    {
        return offset < 0x80
             ? (ushort)(value + offset)
             : (ushort)(value - (0xFF - offset) - 1);
    }

    /// <summary>
    /// Checks if the new address has crossed a page when compared to the current address.
    /// Pages are sets of 8-bit values and when crosses, may incur in extra cycles for the CPU.
    /// </summary>
    /// <param name="currentAddress">Current address being looked</param>
    /// <param name="newAddress">Final address to be checked against</param>
    /// <returns>True if the page was crossed, false otherwise</returns>
    public static bool CheckPageCrossed(this ushort currentAddress, in ushort newAddress)
    {
        var previousBoundary = currentAddress.MostSignificantBits();
        var currentBoundary = newAddress.MostSignificantBits();

        return !previousBoundary.Equals(currentBoundary);
    }
}
