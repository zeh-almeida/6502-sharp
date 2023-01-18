using Cpu.Instructions.Arithmetic;
using Cpu.Instructions.Exceptions;
using Cpu.States;
using Moq;
using Test.Unit.Cpu.Utils;
using Xunit;

namespace Test.Unit.Cpu.Instructions.Arithmetic;

public sealed record CompareAccumulatorTest : IClassFixture<CompareAccumulator>
{
    #region Properties
    private CompareAccumulator Subject { get; }
    #endregion

    #region Constructors
    public CompareAccumulatorTest(CompareAccumulator subject)
    {
        this.Subject = subject;
    }
    #endregion

    [Theory]
    [InlineData(0xC9)]
    [InlineData(0xC5)]
    [InlineData(0xD5)]
    [InlineData(0xCD)]
    [InlineData(0xDD)]
    [InlineData(0xD9)]
    [InlineData(0xC1)]
    [InlineData(0xD1)]
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
    public void Execute_UnknownOpcode_Throws()
    {
        var stateMock = SetupMock(0x00, 0);
        _ = Assert.Throws<UnknownOpcodeException>(() => this.Subject.Execute(stateMock.Object, 0));
    }

    [Fact]
    public void Execute_Equals_WritesZeroFlag()
    {
        const byte value = 0b_0000_0000;
        const byte accumulator = 0b_0000_0000;

        var stateMock = SetupMock(0xC9, accumulator);

        this.Subject.Execute(stateMock.Object, value);

        stateMock.VerifySet(state => state.Flags.IsZero = true, Times.Once());
        stateMock.VerifySet(state => state.Flags.IsNegative = false, Times.Once());
        stateMock.VerifySet(state => state.Flags.IsCarry = true, Times.Once());
    }

    [Fact]
    public void Execute_ResultSeventhBitSet_WritesNegativeFlag()
    {
        const byte value = 0b_0000_0001;
        const byte accumulator = 0b_1000_0001;

        var stateMock = SetupMock(0xC9, accumulator);

        this.Subject.Execute(stateMock.Object, value);

        stateMock.VerifySet(state => state.Flags.IsZero = false, Times.Once());
        stateMock.VerifySet(state => state.Flags.IsNegative = true, Times.Once());
        stateMock.VerifySet(state => state.Flags.IsCarry = true, Times.Once());
    }

    [Fact]
    public void Execute_ValueLessEqualToAccumulator_WritesCarryFlag()
    {
        const byte value = 0b_0000_0001;
        const byte accumulator = 0b_0000_0001;

        var stateMock = SetupMock(0xC9, accumulator);

        this.Subject.Execute(stateMock.Object, value);

        stateMock.VerifySet(state => state.Flags.IsZero = false, Times.Once());
        stateMock.VerifySet(state => state.Flags.IsNegative = false, Times.Once());
        stateMock.VerifySet(state => state.Flags.IsCarry = true, Times.Once());
    }

    [Fact]
    public void Execute_Immediate_Compares()
    {
        const byte value = 0b_0000_0011;
        const byte accumulator = 0b_0000_0001;

        var stateMock = SetupMock(0xC9, accumulator);

        this.Subject.Execute(stateMock.Object, value);

        stateMock.Verify(state => state.Memory.ReadZeroPage(It.IsAny<ushort>()), Times.Never());
        stateMock.Verify(state => state.Memory.ReadZeroPageX(It.IsAny<ushort>()), Times.Never());
        stateMock.Verify(state => state.Memory.ReadAbsolute(It.IsAny<ushort>()), Times.Never());
        stateMock.Verify(state => state.Memory.ReadAbsoluteX(It.IsAny<ushort>()), Times.Never());
        stateMock.Verify(state => state.Memory.ReadAbsoluteY(It.IsAny<ushort>()), Times.Never());
        stateMock.Verify(state => state.Memory.ReadIndirectX(It.IsAny<ushort>()), Times.Never());
        stateMock.Verify(state => state.Memory.ReadIndirectY(It.IsAny<ushort>()), Times.Never());
    }

    [Fact]
    public void Execute_ZeroPage_Compares()
    {
        const ushort address = 2;

        const byte value = 0b_0000_0011;
        const byte accumulator = 0b_0000_0001;

        var stateMock = SetupMock(0xC5, accumulator);

        _ = stateMock
            .Setup(s => s.Memory.ReadZeroPage(address))
            .Returns(value);

        this.Subject.Execute(stateMock.Object, address);

        stateMock.Verify(state => state.Memory.ReadZeroPage(address), Times.Once());
    }

    [Fact]
    public void Execute_ZeroPageX_Compares()
    {
        const ushort address = 2;
        const byte registerX = 2;
        const ushort finalAddress = address + registerX;

        const byte value = 0b_0000_0011;
        const byte accumulator = 0b_0000_0001;

        var stateMock = SetupMock(0xD5, accumulator);

        _ = stateMock
            .Setup(s => s.Memory.ReadZeroPageX(finalAddress))
            .Returns(value);

        this.Subject.Execute(stateMock.Object, address);

        stateMock.Verify(state => state.Memory.ReadZeroPageX(address), Times.Once());
    }

    [Fact]
    public void Execute_Absolute_Compares()
    {
        const ushort address = 2;

        const byte value = 0b_0000_0011;
        const byte accumulator = 0b_0000_0001;

        var stateMock = SetupMock(0xCD, accumulator);

        _ = stateMock
            .Setup(s => s.Memory.ReadAbsolute(address))
            .Returns(value);

        this.Subject.Execute(stateMock.Object, address);

        stateMock.Verify(state => state.Memory.ReadAbsolute(address), Times.Once());
    }

    [Fact]
    public void Execute_AbsoluteX_Compares()
    {
        const ushort address = 2;

        const byte value = 0b_0000_0011;
        const byte accumulator = 0b_0000_0001;

        var stateMock = SetupMock(0xDD, accumulator);

        _ = stateMock
            .Setup(s => s.Memory.ReadAbsoluteX(address))
            .Returns((false, value));

        this.Subject.Execute(stateMock.Object, address);

        stateMock.Verify(state => state.IncrementCycles(It.IsAny<int>()), Times.Never());

        stateMock.Verify(state => state.Memory.ReadAbsoluteX(address), Times.Once());
    }

    [Fact]
    public void Execute_AbsoluteX_AdditionalCycles()
    {
        const ushort address = 2;

        const byte value = 0b_0000_0011;
        const byte accumulator = 0b_0000_0001;

        var stateMock = SetupMock(0xDD, accumulator);

        _ = stateMock
            .Setup(s => s.Memory.ReadAbsoluteX(address))
            .Returns((true, value));

        this.Subject.Execute(stateMock.Object, address);

        stateMock.Verify(state => state.IncrementCycles(It.IsAny<int>()), Times.Once());

        stateMock.Verify(state => state.Memory.ReadAbsoluteX(address), Times.Once());
    }

    [Fact]
    public void Execute_AbsoluteY_Compares()
    {
        const ushort address = 2;

        const byte value = 0b_0000_0011;
        const byte accumulator = 0b_0000_0001;

        var stateMock = SetupMock(0xD9, accumulator);

        _ = stateMock
            .Setup(s => s.Memory.ReadAbsoluteY(address))
            .Returns((false, value));

        this.Subject.Execute(stateMock.Object, address);

        stateMock.Verify(state => state.IncrementCycles(It.IsAny<int>()), Times.Never());

        stateMock.Verify(state => state.Memory.ReadAbsoluteY(address), Times.Once());
    }

    [Fact]
    public void Execute_AbsoluteY_AdditionalCycles()
    {
        const ushort address = 2;

        const byte value = 0b_0000_0011;
        const byte accumulator = 0b_0000_0001;

        var stateMock = SetupMock(0xD9, accumulator);

        _ = stateMock
            .Setup(s => s.Memory.ReadAbsoluteY(address))
            .Returns((true, value));

        this.Subject.Execute(stateMock.Object, address);

        stateMock.Verify(state => state.IncrementCycles(It.IsAny<int>()), Times.Once());

        stateMock.Verify(state => state.Memory.ReadAbsoluteY(address), Times.Once());
    }

    [Fact]
    public void Execute_IndirectX_Compares()
    {
        const ushort address = 1;

        const byte value = 0b_0000_0011;
        const byte accumulator = 0b_0000_0001;

        var stateMock = SetupMock(0xC1, accumulator);

        _ = stateMock
            .Setup(s => s.Memory.ReadIndirectX(address))
            .Returns(value);

        this.Subject.Execute(stateMock.Object, address);

        stateMock.Verify(state => state.Memory.ReadIndirectX(address), Times.Once());
    }

    [Fact]
    public void Execute_IndirectY_Compares()
    {
        const ushort address = 0;

        const byte value = 0b_0000_0011;
        const byte accumulator = 0b_0000_0001;

        var stateMock = SetupMock(0xD1, accumulator);

        _ = stateMock
            .Setup(s => s.Memory.ReadIndirectY(address))
            .Returns((false, value));

        this.Subject.Execute(stateMock.Object, address);

        stateMock.Verify(state => state.IncrementCycles(It.IsAny<int>()), Times.Never());

        stateMock.Verify(state => state.Memory.ReadIndirectY(address), Times.Once());
    }

    [Fact]
    public void Execute_IndirectY_AdditionalCycles()
    {
        const ushort address = 0;

        const byte value = 0b_0000_0011;
        const byte accumulator = 0b_0000_0001;

        var stateMock = SetupMock(0xD1, accumulator);

        _ = stateMock
            .Setup(s => s.Memory.ReadIndirectY(address))
            .Returns((true, value));

        this.Subject.Execute(stateMock.Object, address);

        stateMock.Verify(state => state.IncrementCycles(It.IsAny<int>()), Times.Once());

        stateMock.Verify(state => state.Memory.ReadIndirectY(address), Times.Once());
    }

    private static Mock<ICpuState> SetupMock(byte opcode, byte accumulator)
    {
        var stateMock = TestUtils.GenerateStateMock();

        _ = stateMock
            .Setup(s => s.ExecutingOpcode)
            .Returns(opcode);

        _ = stateMock
            .Setup(s => s.Registers.Accumulator)
            .Returns(accumulator);

        return stateMock;
    }
}
