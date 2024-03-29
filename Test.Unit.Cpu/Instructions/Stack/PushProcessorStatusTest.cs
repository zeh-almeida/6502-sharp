﻿using Cpu.Instructions.Stack;
using Moq;
using Test.Unit.Cpu.Utils;
using Xunit;

namespace Test.Unit.Cpu.Instructions.Stack;

public sealed record PushProcessorStatusTest : IClassFixture<PushProcessorStatus>
{
    #region Properties
    private PushProcessorStatus Subject { get; }
    #endregion

    #region Constructors
    public PushProcessorStatusTest(PushProcessorStatus subject)
    {
        this.Subject = subject;
    }
    #endregion

    [Theory]
    [InlineData(0x08)]
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
    public void Execute_State_WritesStack()
    {
        const byte value = 0b_1001_1001;
        var stateMock = TestUtils.GenerateStateMock();

        _ = stateMock
            .Setup(s => s.Flags.Save())
            .Returns(value);

        this.Subject.Execute(stateMock.Object, 0);

        stateMock.Verify(state => state.Stack.Push(value), Times.Once());
    }
}
