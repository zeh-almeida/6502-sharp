using Cpu.Instructions.Increments;
using Cpu.States;
using Moq;
using Test.Unit.Cpu.Utils;
using Xunit;

namespace Test.Unit.Cpu.Instructions.Increments;

public sealed record IncrementRegisterXTest : IClassFixture<IncrementRegisterX>
{
    #region Properties
    private IncrementRegisterX Subject { get; }
    #endregion

    #region Constructors
    public IncrementRegisterXTest(IncrementRegisterX subject)
    {
        this.Subject = subject;
    }
    #endregion

    [Theory]
    [InlineData(0xE8)]
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
    public void Execute_IncrementWraps_WritesZeroFlag()
    {
        const byte value = 0b_1111_1111;
        const byte result = 0b_0000_0000;

        var stateMock = SetupMock(0xE8, value);

        this.Subject.Execute(stateMock.Object, 0);

        stateMock.Verify(state => state.Registers.IndexX, Times.Once());
        stateMock.VerifySet(state => state.Registers.IndexX = result, Times.Once());

        stateMock.VerifySet(state => state.Flags.IsZero = true, Times.Once());
        stateMock.VerifySet(state => state.Flags.IsNegative = false, Times.Once());
    }

    [Fact]
    public void Execute_ResultSeventhBitSet_WritesNegativeFlag()
    {
        const byte value = 0b_0111_1111;
        const byte result = 0b_1000_0000;

        var stateMock = SetupMock(0xE8, value);

        this.Subject.Execute(stateMock.Object, 0);

        stateMock.Verify(state => state.Registers.IndexX, Times.Once());
        stateMock.VerifySet(state => state.Registers.IndexX = result, Times.Once());

        stateMock.VerifySet(state => state.Flags.IsZero = false, Times.Once());
        stateMock.VerifySet(state => state.Flags.IsNegative = true, Times.Once());
    }

    [Fact]
    public void Execute_Increments_ChangesRegister()
    {
        const byte value = 0b_0000_0000;
        const byte result = 0b_0000_0001;

        var stateMock = SetupMock(0xE8, value);

        this.Subject.Execute(stateMock.Object, 0);

        stateMock.Verify(state => state.Registers.IndexX, Times.Once());
        stateMock.VerifySet(state => state.Registers.IndexX = result, Times.Once());
    }

    private static Mock<ICpuState> SetupMock(byte opcode, byte registerX)
    {
        var stateMock = TestUtils.GenerateStateMock();

        _ = stateMock
            .Setup(s => s.ExecutingOpcode)
            .Returns(opcode);

        _ = stateMock
            .Setup(state => state.Registers.IndexX)
            .Returns(registerX);

        return stateMock;
    }
}
