using Cpu.Instructions.Illegal;
using Cpu.States;
using Moq;
using Test.Unit.Cpu.Utils;
using Xunit;

namespace Test.Unit.Cpu.Instructions.Illegal;

public sealed record AndAccumulatorXTest : IClassFixture<AndAccumulatorX>
{
    #region Properties
    private AndAccumulatorX Subject { get; }
    #endregion

    #region Constructors
    public AndAccumulatorXTest(AndAccumulatorX subject)
    {
        this.Subject = subject;
    }
    #endregion

    [Theory]
    [InlineData(0x8B)]
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
    public void ZeroValue_ZeroAccumulator_ZeroRegisterX_WritesZeroFlag()
    {
        const ushort value = 0b_0000_0000;
        const byte registerX = 0b_0000_0000;
        const byte accumulator = 0b_0000_0000;

        const byte result = 0b_0000_0000;

        var stateMock = SetupMock(accumulator, registerX);

        this.Subject.Execute(stateMock.Object, value);

        stateMock.Verify(state => state.Registers.Accumulator, Times.Once());
        stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());
        stateMock.VerifySet(state => state.Flags.IsZero = true, Times.Once());
    }

    [Fact]
    public void Value_Accumulator_RegisterX_WritesNegativeFlag()
    {
        const ushort value = 0b_1000_0001;
        const byte registerX = 0b_1000_0001;
        const byte accumulator = 0b_1000_0101;

        const byte result = 0b_1000_0001;

        var stateMock = SetupMock(accumulator, registerX);

        this.Subject.Execute(stateMock.Object, value);

        stateMock.Verify(state => state.Registers.Accumulator, Times.Once());
        stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());
        stateMock.VerifySet(state => state.Flags.IsNegative = true, Times.Once());
    }

    private static Mock<ICpuState> SetupMock(byte accumulator, byte registerX)
    {
        var stateMock = TestUtils.GenerateStateMock();

        _ = stateMock
            .Setup(s => s.ExecutingOpcode)
            .Returns(0x0B);

        _ = stateMock
            .Setup(s => s.Registers.Accumulator)
            .Returns(accumulator);

        _ = stateMock
            .Setup(s => s.Registers.IndexX)
            .Returns(registerX);

        return stateMock;
    }
}
