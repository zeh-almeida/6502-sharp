using Cpu.Instructions.Logic;
using Cpu.Opcodes;
using Xunit;

namespace Test.Unit.Cpu.Opcodes;
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type. - Necessary for null tests

public sealed record OpcodeInformationTest
{
    #region Constants
    private const byte Opcode = 0x01;

    private const int MinimumCycles = 1;

    private const int MaximumCycles = 2;

    private const int Bytes = 2;
    #endregion

    #region Properties
    private OpcodeInformation Subject { get; }
    #endregion

    #region Constructors
    public OpcodeInformationTest()
    {
#pragma warning disable CS8601 // Possible null reference assignment.
        this.Subject = new OpcodeInformation(Opcode, MinimumCycles, Bytes)
            .SetInstruction(new InclusiveOr()) as OpcodeInformation;

        if (this.Subject is null)
        {
            throw new Exception("Should never happen");
        }
#pragma warning restore CS8601 // Possible null reference assignment.
    }
    #endregion

    [Fact]
    public void Different_Cycles_Constructor_Instantiates()
    {
        var target = typeof(InclusiveOr).FullName ?? throw new Exception("Error qualifying instruction");

        var subject = new OpcodeInformation(
            Opcode,
            Bytes,
            target,
            MinimumCycles,
            MaximumCycles);

        Assert.Equal(MinimumCycles, subject.MinimumCycles);
        Assert.Equal(MaximumCycles, subject.MaximumCycles);

        Assert.NotNull(subject.Instruction);
        Assert.Equal(target, subject.Instruction.GetType().FullName);
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
        Assert.Equal(MinimumCycles, this.Subject.MaximumCycles);
    }

    [Fact]
    public void Bytes_Equals_Defined()
    {
        Assert.Equal(Bytes, this.Subject.Bytes);
    }

    [Fact]
    public void Instruction_Is_Set()
    {
        Assert.NotNull(this.Subject.Instruction);
    }

    [Fact]
    public void Instruction_SetNull_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(() => this.Subject.SetInstruction(null));
    }

    [Fact]
    public void Instruction_AlreadySet_Throws()
    {
        _ = Assert.Throws<ArgumentException>(() => this.Subject.SetInstruction(new InclusiveOr()));
    }
}
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
