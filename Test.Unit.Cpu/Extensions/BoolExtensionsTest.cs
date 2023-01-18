using Cpu.Extensions;
using Xunit;

namespace Test.Unit.Cpu.Extensions;

public sealed record BoolExtensionsTest
{
    [Theory]
    [InlineData(true, 1)]
    [InlineData(false, 0)]
    public void AsBin_Converts(bool value, int expected)
    {
        Assert.Equal(expected, value.AsBin());
    }
}
