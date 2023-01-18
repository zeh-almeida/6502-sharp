using Cpu.Instructions.Stack;
using Moq;
using Test.Unit.Cpu.Utils;
using Xunit;

namespace Test.Unit.Cpu.Instructions.Stack;

public sealed record PushAccumulatorTest : IClassFixture<PushAccumulator>
{
    #region Properties
    private PushAccumulator Subject { get; }
    #endregion

    #region Constructors
    public PushAccumulatorTest(PushAccumulator subject)
    {
        this.Subject = subject;
    }
    #endregion

    [Theory]
    [InlineData(0x48)]
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
    public void Execute_Accumulator_WritesRegister()
    {
        const byte value = 1;

        var stateMock = TestUtils.GenerateStateMock();

        _ = stateMock
            .Setup(s => s.Registers.Accumulator)
            .Returns(value);

        this.Subject.Execute(stateMock.Object, 0);

        stateMock.Verify(state => state.Stack.Push16(value), Times.Once());
    }
}
