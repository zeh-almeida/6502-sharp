using Moq;
using NesApu.Channel;
using NesApu.Utilities;
using Xunit;

namespace Test.Unit.NesApu.Channel;

public sealed record NoiseTest
{
    #region Properties
    private Noise Noise { get; set; }

    private Mock<IRandomGenerator> RandomMock { get; }
    #endregion

    #region Constructors
    public NoiseTest()
    {
        this.RandomMock = new Mock<IRandomGenerator>();
        this.Noise = new Noise(0, this.RandomMock.Object);
    }
    #endregion

    [Fact]
    public void Initialized_Amplitude_IsSet()
    {
        Assert.Equal(0.00, this.Noise.Amplitude);
    }

    [Fact]
    public void Initialized_ShiftRegister_IsSet()
    {
        Assert.Equal(1, this.Noise.ShiftRegister);
    }
}
