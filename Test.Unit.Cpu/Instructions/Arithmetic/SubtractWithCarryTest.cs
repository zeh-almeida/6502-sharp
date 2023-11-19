using Cpu.Instructions.Arithmetic;
using Cpu.Instructions.Exceptions;
using Cpu.States;
using Moq;
using Test.Unit.Cpu.Utils;
using Xunit;

namespace Test.Unit.Cpu.Instructions.Arithmetic;

public sealed record SubtractWithCarryTest : IClassFixture<SubtractWithCarry>
{
    #region Properties
    private SubtractWithCarry Subject { get; }
    #endregion

    #region Constructors
    public SubtractWithCarryTest(SubtractWithCarry subject)
    {
        this.Subject = subject;
    }
    #endregion

    [Theory]
    [InlineData(0xE9)]
    [InlineData(0xE5)]
    [InlineData(0xF5)]
    [InlineData(0xED)]
    [InlineData(0xFD)]
    [InlineData(0xF9)]
    [InlineData(0xE1)]
    [InlineData(0xF1)]
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
        const byte accumulator = 0b_0000_0000;
        const byte value = 0b_0000_0000;
        const byte result = 0b_0000_0000;

        var stateMock = SetupMock(0xE9, accumulator);

        this.Subject.Execute(stateMock.Object, value);

        stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());

        stateMock.VerifySet(state => state.Flags.IsZero = true, Times.Once());
        stateMock.VerifySet(state => state.Flags.IsNegative = false, Times.Once());
        stateMock.VerifySet(state => state.Flags.IsOverflow = false, Times.Once());
        stateMock.VerifySet(state => state.Flags.IsCarry = true, Times.Once());
    }

    [Fact]
    public void Execute_ResultSeventhBitSet_WritesNegativeFlag()
    {
        const byte accumulator = 0b_0000_0101;
        const byte value = 0b_000_0110;
        const byte result = 0b_1111_1111;

        var stateMock = SetupMock(0xE9, accumulator);

        this.Subject.Execute(stateMock.Object, value);

        stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());

        stateMock.VerifySet(state => state.Flags.IsZero = false, Times.Once());
        stateMock.VerifySet(state => state.Flags.IsNegative = true, Times.Once());
        stateMock.VerifySet(state => state.Flags.IsOverflow = false, Times.Once());
        stateMock.VerifySet(state => state.Flags.IsCarry = false, Times.Once());
    }

    [Fact]
    public void Execute_ResultOverflows_WritesOverflowFlag()
    {
        const byte accumulator = 0b_0000_0101;
        const byte value = 0b_000_0001;
        const byte result = 0b_0000_0100;

        var stateMock = SetupMock(0xE9, accumulator);

        this.Subject.Execute(stateMock.Object, value);

        stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());

        stateMock.VerifySet(state => state.Flags.IsZero = false, Times.Once());
        stateMock.VerifySet(state => state.Flags.IsNegative = false, Times.Once());
        stateMock.VerifySet(state => state.Flags.IsOverflow = true, Times.Once());
        stateMock.VerifySet(state => state.Flags.IsCarry = true, Times.Once());
    }

    [Fact]
    public void Execute_ResultPositiveBit_WritesCarryFlag()
    {
        const byte accumulator = 0b_0000_0101;
        const byte value = 0b_0000_0001;
        const byte result = 0b_0000_0100;

        var stateMock = SetupMock(0xE9, accumulator);

        this.Subject.Execute(stateMock.Object, value);

        stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());

        stateMock.VerifySet(state => state.Flags.IsZero = false, Times.Once());
        stateMock.VerifySet(state => state.Flags.IsNegative = false, Times.Once());
        stateMock.VerifySet(state => state.Flags.IsOverflow = true, Times.Once());
        stateMock.VerifySet(state => state.Flags.IsCarry = true, Times.Once());
    }

    [Fact]
    public void Execute_NoCarry_Subtracts()
    {
        const byte accumulator = 0b_0000_0101;
        const byte value = 0b_000_0001;
        const byte result = 0b_0000_0011;

        var stateMock = SetupMock(0xE9, accumulator);

        _ = stateMock
            .Setup(s => s.Flags.IsCarry)
            .Returns(false);

        this.Subject.Execute(stateMock.Object, value);

        stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());
    }

    [Fact]
    public void Execute_BCD_WritesNegativeFlag()
    {
        const byte value = 0b_0000_0001;
        const byte accumulator = 0b_0000_0000;
        const byte result = 0b_1001_1001;

        var stateMock = SetupMock(0xE9, accumulator);

        _ = stateMock
            .Setup(s => s.Flags.IsDecimalMode)
            .Returns(true);

        this.Subject.Execute(stateMock.Object, value);

        stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());
        stateMock.VerifySet(state => state.Flags.IsNegative = true, Times.Once());
    }

    [Fact]
    public void Execute_BCD_WritesCarryFlag()
    {
        const byte value = 0b_0001_0010;
        const byte accumulator = 0b_0100_0110;
        const byte result = 0b_0011_0100;

        var stateMock = SetupMock(0xE9, accumulator);

        _ = stateMock
            .Setup(s => s.Flags.IsDecimalMode)
            .Returns(true);

        _ = stateMock
            .Setup(s => s.Flags.IsCarry)
            .Returns(true);

        this.Subject.Execute(stateMock.Object, value);

        stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());
        stateMock.VerifySet(state => state.Flags.IsCarry = true, Times.Once());
    }

    [Fact]
    public void Execute_BCD_WritesZeroFlag()
    {
        const byte value = 0b_0000_0010;
        const byte accumulator = 0b_0000_0010;
        const byte result = 0b_0000_0000;

        var stateMock = SetupMock(0xE9, accumulator);

        _ = stateMock
            .Setup(s => s.Flags.IsDecimalMode)
            .Returns(true);

        _ = stateMock
            .Setup(s => s.Flags.IsCarry)
            .Returns(true);

        this.Subject.Execute(stateMock.Object, value);

        stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());
        stateMock.VerifySet(state => state.Flags.IsZero = true, Times.Once());
    }

    [Fact]
    public void Execute_BCD_NoCarry_Subtracts()
    {
        const byte value = 0b_0000_0001;
        const byte accumulator = 0b_0000_0011;
        const byte result = 0b_0000_0001;

        var stateMock = SetupMock(0xE9, accumulator);

        _ = stateMock
            .Setup(s => s.Flags.IsDecimalMode)
            .Returns(true);

        _ = stateMock
            .Setup(s => s.Flags.IsCarry)
            .Returns(false);

        this.Subject.Execute(stateMock.Object, value);

        stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());
    }

    [Fact]
    public void Execute_Immediate_Subtracts()
    {
        const byte accumulator = 0b_0000_0101;
        const byte value = 0b_000_0001;
        const byte result = 0b_0000_0100;

        var stateMock = SetupMock(0xE9, accumulator);

        this.Subject.Execute(stateMock.Object, value);

        stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());
    }

    [Fact]
    public void Execute_ZeroPage_Subtracts()
    {
        const ushort address = 2;

        const byte accumulator = 0b_0000_0101;
        const byte value = 0b_000_0001;
        const byte result = 0b_0000_0100;

        var stateMock = SetupMock(0xE5, accumulator);

        _ = stateMock
            .Setup(s => s.Memory.ReadZeroPage(address))
            .Returns(value);

        this.Subject.Execute(stateMock.Object, address);

        stateMock.Verify(state => state.Memory.ReadZeroPage(address), Times.Once());

        stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());
    }

    [Fact]
    public void Execute_ZeroPageX_Subtracts()
    {
        const ushort address = 2;

        const byte accumulator = 0b_0000_0101;
        const byte value = 0b_000_0001;
        const byte result = 0b_0000_0100;

        var stateMock = SetupMock(0xF5, accumulator);

        _ = stateMock
            .Setup(s => s.Memory.ReadZeroPageX(address))
            .Returns(value);

        this.Subject.Execute(stateMock.Object, address);

        stateMock.Verify(state => state.Memory.ReadZeroPageX(address), Times.Once());
        stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());
    }

    [Fact]
    public void Execute_Absolute_Subtracts()
    {
        const ushort address = 2;

        const byte accumulator = 0b_0000_0101;
        const byte value = 0b_000_0001;
        const byte result = 0b_0000_0100;

        var stateMock = SetupMock(0xED, accumulator);

        _ = stateMock
            .Setup(s => s.Memory.ReadAbsolute(address))
            .Returns(value);

        this.Subject.Execute(stateMock.Object, address);

        stateMock.Verify(state => state.Memory.ReadAbsolute(address), Times.Once());
        stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());
    }

    [Fact]
    public void Execute_AbsoluteX_Subtracts()
    {
        const ushort address = 2;

        const byte accumulator = 0b_0000_0101;
        const byte value = 0b_000_0001;
        const byte result = 0b_0000_0100;

        var stateMock = SetupMock(0xFD, accumulator);

        _ = stateMock
            .Setup(s => s.Memory.ReadAbsoluteX(address))
            .Returns((false, value));

        this.Subject.Execute(stateMock.Object, address);

        stateMock.Verify(state => state.IncrementCycles(It.Ref<int>.IsAny), Times.Never());

        stateMock.Verify(state => state.Memory.ReadAbsoluteX(address), Times.Once());
        stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());
    }

    [Fact]
    public void Execute_AbsoluteX_AdditionalCycles()
    {
        const ushort address = 2;

        const byte accumulator = 0b_0000_0101;
        const byte value = 0b_000_0001;
        const byte result = 0b_0000_0100;

        var stateMock = SetupMock(0xFD, accumulator);

        _ = stateMock
            .Setup(s => s.Memory.ReadAbsoluteX(address))
            .Returns((true, value));

        this.Subject.Execute(stateMock.Object, address);

        stateMock.Verify(state => state.IncrementCycles(It.Ref<int>.IsAny), Times.Once());

        stateMock.Verify(state => state.Memory.ReadAbsoluteX(address), Times.Once());
        stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());
    }

    [Fact]
    public void Execute_AbsoluteY_Subtracts()
    {
        const ushort address = 2;

        const byte accumulator = 0b_0000_0101;
        const byte value = 0b_000_0001;
        const byte result = 0b_0000_0100;

        var stateMock = SetupMock(0xF9, accumulator);

        _ = stateMock
            .Setup(s => s.Memory.ReadAbsoluteY(address))
            .Returns((false, value));

        this.Subject.Execute(stateMock.Object, address);

        stateMock.Verify(state => state.IncrementCycles(It.Ref<int>.IsAny), Times.Never());

        stateMock.Verify(state => state.Memory.ReadAbsoluteY(address), Times.Once());
        stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());
    }

    [Fact]
    public void Execute_AbsoluteY_AdditionalCycles()
    {
        const ushort address = 2;

        const byte accumulator = 0b_0000_0101;
        const byte value = 0b_000_0001;
        const byte result = 0b_0000_0100;

        var stateMock = SetupMock(0xF9, accumulator);

        _ = stateMock
            .Setup(s => s.Memory.ReadAbsoluteY(address))
            .Returns((true, value));

        this.Subject.Execute(stateMock.Object, address);

        stateMock.Verify(state => state.IncrementCycles(It.Ref<int>.IsAny), Times.Once());

        stateMock.Verify(state => state.Memory.ReadAbsoluteY(address), Times.Once());
        stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());
    }

    [Fact]
    public void Execute_IndirectX_Subtracts()
    {
        const ushort address = 1;

        const byte accumulator = 0b_0000_0101;
        const byte value = 0b_000_0001;
        const byte result = 0b_0000_0100;

        var stateMock = SetupMock(0xE1, accumulator);

        _ = stateMock
            .Setup(s => s.Memory.ReadIndirectX(address))
            .Returns(value);

        this.Subject.Execute(stateMock.Object, address);

        stateMock.Verify(state => state.Memory.ReadIndirectX(address), Times.Once());
        stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());
    }

    [Fact]
    public void Execute_IndirectY_Subtracts()
    {
        const ushort address = 0;

        const byte accumulator = 0b_0000_0101;
        const byte value = 0b_000_0001;
        const byte result = 0b_0000_0100;

        var stateMock = SetupMock(0xF1, accumulator);

        _ = stateMock
            .Setup(s => s.Memory.ReadIndirectY(address))
            .Returns((false, value));

        this.Subject.Execute(stateMock.Object, address);

        stateMock.Verify(state => state.IncrementCycles(It.Ref<int>.IsAny), Times.Never());

        stateMock.Verify(state => state.Memory.ReadIndirectY(address), Times.Once());
        stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());
    }

    [Fact]
    public void Execute_IndirectY_AdditionalCycles()
    {
        const ushort address = 0;

        const byte accumulator = 0b_0000_0101;
        const byte value = 0b_000_0001;
        const byte result = 0b_0000_0100;

        var stateMock = SetupMock(0xF1, accumulator);

        _ = stateMock
            .Setup(s => s.Memory.ReadIndirectY(address))
            .Returns((true, value));

        this.Subject.Execute(stateMock.Object, address);

        stateMock.Verify(state => state.IncrementCycles(It.Ref<int>.IsAny), Times.Once());

        stateMock.Verify(state => state.Memory.ReadIndirectY(address), Times.Once());
        stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());
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

        _ = stateMock
            .Setup(s => s.Flags.IsCarry)
            .Returns(true);

        return stateMock;
    }
}
