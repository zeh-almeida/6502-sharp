using Cpu.Registers;
using Xunit;

namespace Test.Unit.Cpu.Registers;

public sealed record RegisterManagerTest
{
    #region Properties
    private RegisterManager Subject { get; }
    #endregion

    #region Constructors
    public RegisterManagerTest()
    {
        this.Subject = new RegisterManager();
    }
    #endregion

    [Fact]
    public void WriteRead_Accumulator_Works()
    {
        const byte value = 0b_0000_1111;

        this.Subject.Accumulator = value;
        var result = this.Subject.Accumulator;

        Assert.Equal(value, result);
    }

    [Fact]
    public void WriteRead_IndexX_Works()
    {
        const byte value = 0b_0000_1111;

        this.Subject.IndexX = value;
        var result = this.Subject.IndexX;

        Assert.Equal(value, result);
    }

    [Fact]
    public void WriteRead_IndexY_Works()
    {
        const byte value = 0b_0000_1111;

        this.Subject.IndexY = value;
        var result = this.Subject.IndexY;

        Assert.Equal(value, result);
    }

    [Fact]
    public void WriteRead_ProgramCounter_Works()
    {
        const ushort value = 0b_0000_1111_0000_1111;

        this.Subject.ProgramCounter = value;
        var result = this.Subject.ProgramCounter;

        Assert.Equal(value, result);
    }

    [Fact]
    public void WriteRead_StackPointer_Works()
    {
        const byte value = 0b_0000_1111;

        this.Subject.StackPointer = value;
        var result = this.Subject.StackPointer;

        Assert.Equal(value, result);
    }

    [Fact]
    public void Save_Returns_CurrentState()
    {
        const ushort programCounter = 0b_1010_1010_0101_0101;
        const byte stackPointer = 0b_0000_0001;
        const byte accumulator = 0b_0000_0010;
        const byte indexX = 0b_0000_0100;
        const byte indexY = 0b_0000_1000;

        var expected = new byte[]
        {
                0b_0101_0101,
                0b_1010_1010,
                0b_0000_0001,
                0b_0000_0010,
                0b_0000_0100,
                0b_0000_1000,
        };

        this.Subject.ProgramCounter = programCounter;
        this.Subject.StackPointer = stackPointer;
        this.Subject.Accumulator = accumulator;
        this.Subject.IndexX = indexX;
        this.Subject.IndexY = indexY;

        var result = this.Subject.Save();

        Assert.Equal(expected, result.ToArray());
    }

    [Fact]
    public void Load_WritesMemory_NewState()
    {
        const ushort programCounter = 0b_1010_1010_0101_0101;
        const byte stackPointer = 0b_0000_0001;
        const byte accumulator = 0b_0000_0010;
        const byte indexX = 0b_0000_0100;
        const byte indexY = 0b_0000_1000;

        var expected = new byte[]
        {
                0b_0101_0101,
                0b_1010_1010,
                0b_0000_0001,
                0b_0000_0010,
                0b_0000_0100,
                0b_0000_1000,
        };

        this.Subject.Load(expected);

        Assert.Equal(programCounter, this.Subject.ProgramCounter);
        Assert.Equal(stackPointer, this.Subject.StackPointer);
        Assert.Equal(accumulator, this.Subject.Accumulator);
        Assert.Equal(indexX, this.Subject.IndexX);
        Assert.Equal(indexY, this.Subject.IndexY);
    }

    [Fact]
    public void Load_Null_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(() => this.Subject.Load(null));
    }

    [Fact]
    public void ToString_Executes()
    {
        var expected = new byte[]
        {
                0x10,
                0x10,
                0x10,
                0x10,
                0x10,
                0x10,
        };

        this.Subject.Load(expected);

        Assert.Equal("PC:0x1010;SP:0x10;A:0x10;X:0x10;Y:0x10", this.Subject.ToString());
    }

    [Theory]
    [InlineData(6 + 1)]
    [InlineData(6 - 1)]
    public void Load_WrongLength_Throws(int length)
    {
        var memory = new byte[length];

        _ = Assert.Throws<ArgumentOutOfRangeException>(() => this.Subject.Load(memory));
    }
}