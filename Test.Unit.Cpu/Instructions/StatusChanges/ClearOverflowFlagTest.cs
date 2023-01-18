using Cpu.Instructions.StatusChanges;
using Moq;
using Test.Unit.Cpu.Utils;
using Xunit;

namespace Test.Unit.Cpu.Instructions.StatusChanges;

public sealed record ClearOverflowFlagTest : IClassFixture<ClearOverflowFlag>
{
    #region Properties
    private ClearOverflowFlag Subject { get; }
    #endregion

    #region Constructors
    public ClearOverflowFlagTest(ClearOverflowFlag subject)
    {
        this.Subject = subject;
    }
    #endregion

    [Theory]
    [InlineData(0xB8)]
    public void HasOpcode_Matches_True(byte opcode)
    {
        Assert.True(this.Subject.HasOpcode(opcode));
    }

    [Fact]
    public void HashCode_Matches_True()
    {
        Assert.Equal(this.Subject.GetHashCode(), this.Subject.Opcodes.GetHashCode());
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
        Assert.False(this.Subject.Equals(1));
    }

    [Fact]
    public void Execute_SetsFlag_False()
    {
        var stateMock = TestUtils.GenerateStateMock();
        this.Subject.Execute(stateMock.Object, 0);

        stateMock.VerifySet(state => state.Flags.IsOverflow = false, Times.Once());
    }
}
