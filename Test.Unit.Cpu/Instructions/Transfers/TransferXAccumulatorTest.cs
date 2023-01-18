using Cpu.Instructions.Transfers;
using Cpu.States;
using Moq;
using Test.Unit.Cpu.Utils;
using Xunit;

namespace Test.Unit.Cpu.Instructions.Transfers;

public sealed record TransferXAccumulatorTest
{
    #region Properties
    private TransferXAccumulator Subject { get; }
    #endregion

    #region Constructors
    public TransferXAccumulatorTest()
    {
        this.Subject = new TransferXAccumulator();
    }
    #endregion

    [Theory]
    [InlineData(0x8A)]
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
    public void Execute_ZeroIndex_ChangesZeroFlag()
    {
        const byte value = 0;
        var stateMock = SetupMock(value);

        this.Subject.Execute(stateMock.Object, 0);

        BasicExecutionAssertion(stateMock, value);

        stateMock.VerifySet(state => state.Flags.IsZero = true, Times.Once());
    }

    [Fact]
    public void Execute_NegativeIndex_ChangesNegativeFlag()
    {
        const byte value = 0b_1000_0000;
        var stateMock = SetupMock(value);

        this.Subject.Execute(stateMock.Object, 0);

        BasicExecutionAssertion(stateMock, value);

        stateMock.VerifySet(state => state.Flags.IsNegative = true, Times.Once());
    }

    [Fact]
    public void Execute_PositiveIndex_WritesRegister()
    {
        const byte value = 1;
        var stateMock = SetupMock(value);

        this.Subject.Execute(stateMock.Object, 0);

        BasicExecutionAssertion(stateMock, value);

        stateMock.VerifySet(state => state.Flags.IsNegative = false, Times.Once());
        stateMock.VerifySet(state => state.Flags.IsZero = false, Times.Once());
    }

    private static Mock<ICpuState> SetupMock(byte value)
    {
        var stateMock = TestUtils.GenerateStateMock();

        _ = stateMock
            .Setup(s => s.Registers.IndexX)
            .Returns(value);

        return stateMock;
    }

    private static void BasicExecutionAssertion(Mock<ICpuState> stateMock, byte value)
    {
        stateMock.Verify(state => state.Registers.IndexX, Times.Once());
        stateMock.VerifySet(state => state.Registers.Accumulator = value, Times.Once());
    }
}
