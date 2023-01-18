using Cpu.Instructions.Branches;
using Cpu.States;
using Moq;
using Test.Unit.Cpu.Utils;
using Xunit;

namespace Test.Unit.Cpu.Instructions.Branches;

public sealed record BranchPositiveTest : IClassFixture<BranchPositive>
{
    #region Properties
    private BranchPositive Subject { get; }
    #endregion

    #region Constructors
    public BranchPositiveTest(BranchPositive subject)
    {
        this.Subject = subject;
    }
    #endregion

    [Theory]
    [InlineData(0x10)]
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
    public void Execute_FlagIsTrue_NoOp()
    {
        const ushort value = 1;
        const ushort counter = 0;

        var stateMock = SetupMock(true, counter);

        this.Subject.Execute(stateMock.Object, value);

        stateMock.Verify(state => state.Registers.ProgramCounter, Times.Never());
        stateMock.Verify(state => state.IncrementCycles(It.IsAny<int>()), Times.Never());
        stateMock.VerifySet(state => state.Registers.ProgramCounter = It.IsAny<ushort>(), Times.Never());
    }

    [Theory]
    [InlineData(0x00FE, 1, 0x00FF, 1)]
    [InlineData(0x00FE, 2, 0x0100, 2)]
    public void Execute_FlagIsTrue_WritesProgramCounter(
        ushort counter,
        ushort value,
        ushort result,
        int cycles)
    {
        var stateMock = SetupMock(false, counter);

        this.Subject.Execute(stateMock.Object, value);

        stateMock.Verify(state => state.IncrementCycles(cycles), Times.Once());
        stateMock.VerifySet(state => state.Registers.ProgramCounter = result, Times.Once());
    }

    private static Mock<ICpuState> SetupMock(bool isFlag, ushort counter)
    {
        var stateMock = TestUtils.GenerateStateMock();

        _ = stateMock
            .Setup(state => state.Flags.IsNegative)
            .Returns(isFlag);

        _ = stateMock
            .Setup(state => state.Registers.ProgramCounter)
            .Returns(counter);

        return stateMock;
    }
}
