using Cpu.Extensions;
using System.Collections;
using Xunit;

namespace Test.Unit.Cpu.Extensions;

public sealed record BitArrayExtensionsTest
{
    [Fact]
    public void AsEightBit_Null_Throws()
    {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8604 // Possible null reference argument.
        BitArray target = null;
        _ = Assert.Throws<ArgumentNullException>(() => target.AsEightBit());
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
    }

    [Theory]
    [InlineData(new int[] { 0 }, 0)]
    [InlineData(new int[] { 0x01 }, 0x01)]
    [InlineData(new int[] { 0x02 }, 0x02)]
    [InlineData(new int[] { 0x03 }, 0x03)]
    [InlineData(new int[] { 0xFF01 }, 0x01)]
    [InlineData(new int[] { 0xFF02 }, 0x02)]
    [InlineData(new int[] { 0xFF03 }, 0x03)]
    [InlineData(new int[] { 0xFFFF01 }, 0x01)]
    [InlineData(new int[] { 0xFFFF02 }, 0x02)]
    [InlineData(new int[] { 0xFFFF03 }, 0x03)]
    public void AsEightBit_Input_Returns(int[] data, byte expected)
    {
        var target = new BitArray(data);
        Assert.Equal(expected, target.AsEightBit());
    }

    [Fact]
    public void AsSixteenBit_Null_Throws()
    {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8604 // Possible null reference argument.
        BitArray target = null;
        _ = Assert.Throws<ArgumentNullException>(() => target.AsSixteenBit());
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
    }

    [Theory]
    [InlineData(new int[] { 0 }, 0)]
    [InlineData(new int[] { 0x01 }, 0x0001)]
    [InlineData(new int[] { 0x02 }, 0x0002)]
    [InlineData(new int[] { 0x03 }, 0x0003)]
    [InlineData(new int[] { 0xFF01 }, 0xFF01)]
    [InlineData(new int[] { 0xFF02 }, 0xFF02)]
    [InlineData(new int[] { 0xFF03 }, 0xFF03)]
    [InlineData(new int[] { 0xFFFF01 }, 0xFF01)]
    [InlineData(new int[] { 0xFFFF02 }, 0xFF02)]
    [InlineData(new int[] { 0xFFFF03 }, 0xFF03)]
    public void AsSixteenit_Input_Returns(int[] data, ushort expected)
    {
        var target = new BitArray(data);
        Assert.Equal(expected, target.AsSixteenBit());
    }
}
