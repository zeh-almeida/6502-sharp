using Cpu.Memory;
using Cpu.Registers;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Test.Unit.Cpu.Memory;

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type. - Necessary for null tests
public sealed record MemoryManagerTest
{
    #region Properties
    private Mock<IRegisterManager> RegisterMock { get; }

    private Mock<ILogger<MemoryManager>> LoggerMock { get; }

    private MemoryManager Subject { get; }
    #endregion

    #region Constructors
    public MemoryManagerTest()
    {
        this.RegisterMock = new Mock<IRegisterManager>();
        this.LoggerMock = new Mock<ILogger<MemoryManager>>();

        this.Subject = new MemoryManager(
            this.LoggerMock.Object,
            this.RegisterMock.Object);
    }
    #endregion

    [Fact]
    public void WriteRead_Absolute_Successful()
    {
        const ushort address = 0;
        const byte value = 1;

        this.Subject.WriteAbsolute(address, value);
        var result = this.Subject.ReadAbsolute(address);

        Assert.Equal(value, result);
    }

    [Fact]
    public void WriteRead_AbsoluteX_Successful()
    {
        const ushort address = 0;
        const byte registerX = 2;

        const byte value = 1;

        _ = this.RegisterMock
            .Setup(s => s.IndexX)
            .Returns(registerX);

        this.Subject.WriteAbsoluteX(address, value);
        var result = this.Subject.ReadAbsoluteX(address);

        this.RegisterMock.Verify(state => state.IndexX, Times.Exactly(2));
        Assert.False(result.Item1);
        Assert.Equal(value, result.Item2);
    }

    [Fact]
    public void WriteRead_AbsoluteX_CrossesPages()
    {
        const ushort address = 0xFE;
        const byte registerX = 2;

        const byte value = 1;

        _ = this.RegisterMock
            .Setup(s => s.IndexX)
            .Returns(registerX);

        this.Subject.WriteAbsoluteX(address, value);
        var result = this.Subject.ReadAbsoluteX(address);

        this.RegisterMock.Verify(state => state.IndexX, Times.Exactly(2));
        Assert.True(result.Item1);
        Assert.Equal(value, result.Item2);
    }

    [Fact]
    public void WriteRead_AbsoluteY_Successful()
    {
        const ushort address = 0;
        const byte registerY = 2;

        const byte value = 1;

        _ = this.RegisterMock
            .Setup(s => s.IndexY)
            .Returns(registerY);

        this.Subject.WriteAbsoluteY(address, value);
        var result = this.Subject.ReadAbsoluteY(address);

        this.RegisterMock.Verify(state => state.IndexY, Times.Exactly(2));
        Assert.False(result.Item1);
        Assert.Equal(value, result.Item2);
    }

    [Fact]
    public void WriteRead_AbsoluteY_CrossesPages()
    {
        const ushort address = 0xFE;
        const byte registerY = 2;

        const byte value = 1;

        _ = this.RegisterMock
            .Setup(s => s.IndexY)
            .Returns(registerY);

        this.Subject.WriteAbsoluteY(address, value);
        var result = this.Subject.ReadAbsoluteY(address);

        this.RegisterMock.Verify(state => state.IndexY, Times.Exactly(2));
        Assert.True(result.Item1);
        Assert.Equal(value, result.Item2);
    }

    [Fact]
    public void WriteRead_Indirect_Successful()
    {
        const ushort absoluteAddress = 1;
        const byte absoluteValue = 2;

        const ushort address = 2;
        const byte value = 3;

        this.Subject.WriteAbsolute(absoluteAddress, absoluteValue);

        this.Subject.WriteIndirect(address, value);
        var result = this.Subject.ReadIndirect(address);

        Assert.Equal(value, result);
    }

    [Fact]
    public void WriteRead_IndirectX_Successful()
    {
        const ushort address = 0;
        const byte registerX = 2;

        const byte value = 1;

        _ = this.RegisterMock
            .SetupSequence(s => s.IndexX)
            .Returns(registerX)
            .Returns(registerX);

        this.Subject.WriteIndirectX(address, value);
        var result = this.Subject.ReadIndirectX(address);

        this.RegisterMock.Verify(state => state.IndexX, Times.Exactly(2));
        Assert.Equal(value, result);
    }

    [Fact]
    public void WriteRead_IndirectY_Successful()
    {
        const ushort address = 0;
        const byte registerY = 2;

        const byte value = 1;

        _ = this.RegisterMock
            .SetupSequence(s => s.IndexY)
            .Returns(registerY)
            .Returns(registerY);

        this.Subject.WriteIndirectY(address, value);
        var result = this.Subject.ReadIndirectY(address);

        this.RegisterMock.Verify(state => state.IndexY, Times.Exactly(2));
        Assert.False(result.Item1);
        Assert.Equal(value, result.Item2);
    }

    [Fact]
    public void WriteRead_IndirectY_CrossesPages()
    {
        const ushort indirectAddress = 2;
        const ushort finalAddress = 0x0100;

        const byte registerY = 2;

        const byte indirectValue = 0xFE;
        const byte finalValue = 0xFE;

        _ = this.RegisterMock
            .SetupSequence(s => s.IndexY)
            .Returns(registerY)
            .Returns(registerY);

        this.Subject.WriteIndirectY(indirectAddress - 1, indirectValue);
        this.Subject.WriteAbsolute(finalAddress, finalValue);

        var result = this.Subject.ReadIndirectY(indirectAddress);

        this.RegisterMock.Verify(state => state.IndexY, Times.Exactly(2));
        Assert.True(result.Item1);
        Assert.Equal(finalValue, result.Item2);
    }

    [Fact]
    public void WriteRead_ZeroPage_Successful()
    {
        const ushort address = 0;
        const byte value = 1;

        this.Subject.WriteZeroPage(address, value);
        var result = this.Subject.ReadZeroPage(address);

        Assert.Equal(value, result);
    }

    [Fact]
    public void WriteRead_ZeroPageX_Successful()
    {
        const ushort address = 0;
        const byte registerX = 2;

        const byte value = 1;

        _ = this.RegisterMock
            .Setup(s => s.IndexX)
            .Returns(registerX);

        this.Subject.WriteZeroPageX(address, value);
        var result = this.Subject.ReadZeroPageX(address);

        this.RegisterMock.Verify(state => state.IndexX, Times.Exactly(2));
        Assert.Equal(value, result);
    }

    [Fact]
    public void WriteRead_ZeroPageY_Successful()
    {
        const ushort address = 0;
        const byte registerY = 2;

        const byte value = 1;

        _ = this.RegisterMock
            .Setup(s => s.IndexY)
            .Returns(registerY);

        this.Subject.WriteZeroPageY(address, value);
        var result = this.Subject.ReadZeroPageY(address);

        this.RegisterMock.Verify(state => state.IndexY, Times.Exactly(2));
        Assert.Equal(value, result);
    }

    [Fact]
    public void WriteRead_ZeroPageX_HigherAddress_WrapsAround()
    {
        const byte value = 1;
        const byte registerX = 0;
        const ushort higherAddress = 0x100;

        _ = this.RegisterMock
            .Setup(s => s.IndexX)
            .Returns(registerX);

        this.Subject.WriteZeroPageX(higherAddress, value);
        var result = this.Subject.ReadZeroPageX(higherAddress);

        Assert.Equal(value, result);
    }

    [Fact]
    public void WriteRead_ZeroPageY_HigherAddress_WrapsAround()
    {
        const byte value = 1;
        const byte registerY = 0;
        const ushort higherAddress = 0x100;

        _ = this.RegisterMock
            .Setup(s => s.IndexY)
            .Returns(registerY);

        this.Subject.WriteZeroPageY(higherAddress, value);
        var result = this.Subject.ReadZeroPageY(higherAddress);

        Assert.Equal(value, result);
    }

    #region Save/Load
    [Fact]
    public void Save_Returns_CurrentState()
    {
        const ushort address = 0;
        const byte value = 1;

        var memory = new byte[ushort.MaxValue + 1];
        memory[0] = value;

        this.Subject.WriteAbsolute(address, value);
        var result = this.Subject.ReadAbsolute(address);

        Assert.Equal(value, result);

        var saved = this.Subject.Save();
        Assert.Equal(memory, saved);
    }

    [Fact]
    public void Load_WritesMemory_NewState()
    {
        const ushort address = 0;
        const byte value = 1;

        var memory = new byte[ushort.MaxValue + 1];
        memory[0] = value;

        this.Subject.Load(memory);
        var result = this.Subject.ReadAbsolute(address);

        Assert.Equal(value, result);
    }

    [Fact]
    public void Load_Null_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(() => this.Subject.Load(null));
    }

    [Theory]
    [InlineData(ushort.MaxValue + 2)]
    [InlineData(ushort.MaxValue)]
    public void Load_WrongLength_Throws(int length)
    {
        var memory = new byte[length];

        _ = Assert.Throws<ArgumentOutOfRangeException>(() => this.Subject.Load(memory));
    }
    #endregion
}
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
