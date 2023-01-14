using Cpu.Opcodes;
using Xunit;

namespace Test.Unit.Cpu.Opcodes;

public sealed record OpcodeInformationTest
{
    #region Constants
    private const byte Opcode = 0x01;

    private const int MinimumCycles = 1;

    private const int MaximumCycles = 2;

    private const int Bytes = 2;

    private const string Mnemonic = "Mnemonic";
    #endregion

    #region Properties
    private OpcodeInformation Subject { get; }
    #endregion

    #region Constructors
    public OpcodeInformationTest()
    {
        this.Subject = new OpcodeInformation(
            Opcode,
            Bytes,
            MinimumCycles,
            MaximumCycles,
            Mnemonic);
    }
    #endregion

    [Fact]
    public void Different_Cycles_Constructor_Instantiates()
    {
        Assert.Equal(MinimumCycles, this.Subject.MinimumCycles);
        Assert.Equal(MaximumCycles, this.Subject.MaximumCycles);
    }

    [Fact]
    public void HashCode_Matches_True()
    {
        Assert.Equal(Opcode.GetHashCode(), this.Subject.GetHashCode());
    }

    [Fact]
    public void Equals_Object_IsTrueForInstruction()
    {
        Assert.True(this.Subject.Equals(this.Subject));
        Assert.True(this.Subject.Equals(this.Subject as object));
    }

    [Fact]
    public void Equals_Object_IsFalseForNonInstructions()
    {
        OpcodeInformation? test = null;

        Assert.False(this.Subject.Equals(test));
        Assert.False(this.Subject.Equals(1));
    }

    [Fact]
    public void Opcode_Equals_Defined()
    {
        Assert.Equal(Opcode, this.Subject.Opcode);
    }

    [Fact]
    public void Cycles_Equals_Defined()
    {
        Assert.Equal(MinimumCycles, this.Subject.MinimumCycles);
        Assert.Equal(MaximumCycles, this.Subject.MaximumCycles);
    }

    [Fact]
    public void Bytes_Equals_Defined()
    {
        Assert.Equal(Bytes, this.Subject.Bytes);
    }

    [Fact]
    public void Mnemonic_Equals_Defined()
    {
        Assert.Equal(Mnemonic, this.Subject.Mnemonic);
    }
}
