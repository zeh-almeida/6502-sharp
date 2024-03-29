﻿using Cpu.Instructions.Stack;
using Cpu.States;
using Moq;
using Test.Unit.Cpu.Utils;
using Xunit;

namespace Test.Unit.Cpu.Instructions.Stack;

public sealed record TransferStackXTest : IClassFixture<TransferPointerX>
{
    #region Properties
    private TransferPointerX Subject { get; }
    #endregion

    #region Constructors
    public TransferStackXTest(TransferPointerX subject)
    {
        this.Subject = subject;
    }
    #endregion

    [Theory]
    [InlineData(0xBA)]
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
    public void Execute_ZeroPointer_ChangesZeroFlag()
    {
        const byte value = 0;
        var stateMock = SetupMock(value);

        this.Subject.Execute(stateMock.Object, 0);

        BasicExecutionAssertion(stateMock, value);

        stateMock.VerifySet(state => state.Flags.IsZero = true, Times.Once());
    }

    [Fact]
    public void Execute_NegativePointer_ChangesNegativeFlag()
    {
        const byte value = 0b_1000_0000;
        var stateMock = SetupMock(value);

        this.Subject.Execute(stateMock.Object, 0);

        BasicExecutionAssertion(stateMock, value);

        stateMock.VerifySet(state => state.Flags.IsNegative = true, Times.Once());
    }

    [Fact]
    public void Execute_PositivePointer_WritesRegister()
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
            .Setup(s => s.Registers.StackPointer)
            .Returns(value);

        return stateMock;
    }

    private static void BasicExecutionAssertion(Mock<ICpuState> stateMock, byte value)
    {
        stateMock.VerifySet(state => state.Registers.IndexX = value, Times.Once());
    }
}
