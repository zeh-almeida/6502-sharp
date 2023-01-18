using Cpu.Instructions.Exceptions;
using Cpu.Instructions.Illegal;
using Cpu.States;
using Moq;
using Test.Unit.Cpu.Utils;
using Xunit;

namespace Test.Unit.Cpu.Instructions.Illegal;

public sealed record SubtractMemoryAccumulatorTest : IClassFixture<SubtractMemoryAccumulator>
{
    #region Properties
    private SubtractMemoryAccumulator Subject { get; }
    #endregion

    #region Constructors
    public SubtractMemoryAccumulatorTest(SubtractMemoryAccumulator subject)
    {
        this.Subject = subject;
    }
    #endregion

    [Theory]
    [InlineData(0xE7)]
    [InlineData(0xF7)]
    [InlineData(0xEF)]
    [InlineData(0xFF)]
    [InlineData(0xFB)]
    [InlineData(0xE3)]
    [InlineData(0xF3)]
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
        var stateMock = SetupMock(0x00, 0x00, false);
        _ = Assert.Throws<UnknownOpcodeException>(() => this.Subject.Execute(stateMock.Object, 0));
    }

    [Fact]
    public void Execution_WritesZeroFlag()
    {
        const ushort address = 0b_0000_0001;

        const byte value = 0b_1111_1111;
        const byte accumulator = 0b_0000_0000;

        var stateMock = SetupMock(0xEF, accumulator, true);

        _ = stateMock
            .Setup(s => s.Memory.ReadAbsolute(address))
            .Returns(value);

        this.Subject.Execute(stateMock.Object, address);

        stateMock.Verify(state => state.Registers.Accumulator, Times.Once());
        stateMock.VerifySet(state => state.Flags.IsZero = true, Times.Once());
    }

    [Fact]
    public void Execution_WritesCarryFlag()
    {
        const ushort address = 0b_0000_0001;

        const byte value = 0b_000_0001;
        const byte accumulator = 0b_0000_0100;

        var stateMock = SetupMock(0xEF, accumulator, false);

        _ = stateMock
            .Setup(s => s.Memory.ReadAbsolute(address))
            .Returns(value);

        this.Subject.Execute(stateMock.Object, address);

        stateMock.Verify(state => state.Registers.Accumulator, Times.Once());
        stateMock.VerifySet(state => state.Flags.IsCarry = true, Times.Once());
    }

    [Fact]
    public void Execution_WritesNegativeFlag()
    {
        const ushort address = 0b_0000_0001;

        const byte value = 0b_000_0110;
        const byte accumulator = 0b_0000_0101;

        var stateMock = SetupMock(0xEF, accumulator, false);

        _ = stateMock
            .Setup(s => s.Memory.ReadAbsolute(address))
            .Returns(value);

        this.Subject.Execute(stateMock.Object, address);

        stateMock.Verify(state => state.Registers.Accumulator, Times.Once());
        stateMock.VerifySet(state => state.Flags.IsNegative = true, Times.Once());
    }

    [Fact]
    public void Execution_ReadZeroPage()
    {
        const ushort address = 0b_0000_0001;

        const byte value = 0b_0000_0010;
        const byte accumulator = 0b_0000_0010;

        var stateMock = SetupMock(0xE7, accumulator, false);

        _ = stateMock
            .Setup(s => s.Memory.ReadZeroPage(address))
            .Returns(value);

        this.Subject.Execute(stateMock.Object, address);

        stateMock.Verify(state => state.Memory.ReadZeroPage(address), Times.Once());
    }

    [Fact]
    public void Execution_ReadZeroPageX()
    {
        const ushort address = 0b_0000_0001;

        const byte value = 0b_0000_0010;
        const byte accumulator = 0b_0000_0010;

        var stateMock = SetupMock(0xF7, accumulator, false);

        _ = stateMock
            .Setup(s => s.Memory.ReadZeroPageX(address))
            .Returns(value);

        this.Subject.Execute(stateMock.Object, address);

        stateMock.Verify(state => state.Memory.ReadZeroPageX(address), Times.Once());
    }

    [Fact]
    public void Execution_ReadIndirectX()
    {
        const ushort address = 0b_0000_0001;

        const byte value = 0b_0000_0010;
        const byte accumulator = 0b_0000_0010;

        var stateMock = SetupMock(0xE3, accumulator, false);

        _ = stateMock
            .Setup(s => s.Memory.ReadIndirectX(address))
            .Returns(value);

        this.Subject.Execute(stateMock.Object, address);

        stateMock.Verify(state => state.Memory.ReadIndirectX(address), Times.Once());
    }

    [Fact]
    public void Execution_ReadIndirectY()
    {
        const ushort address = 0b_0000_0001;

        const byte value = 0b_0000_0010;
        const byte accumulator = 0b_0000_0010;

        var stateMock = SetupMock(0xF3, accumulator, false);

        _ = stateMock
            .Setup(s => s.Memory.ReadIndirectY(address))
            .Returns((false, value));

        this.Subject.Execute(stateMock.Object, address);

        stateMock.Verify(state => state.Memory.ReadIndirectY(address), Times.Once());
    }

    [Fact]
    public void Execution_ReadAbsolute()
    {
        const ushort address = 0b_0000_0001;

        const byte value = 0b_0000_0010;
        const byte accumulator = 0b_0000_0010;

        var stateMock = SetupMock(0xEF, accumulator, false);

        _ = stateMock
            .Setup(s => s.Memory.ReadAbsolute(address))
            .Returns(value);

        this.Subject.Execute(stateMock.Object, address);

        stateMock.Verify(state => state.Memory.ReadAbsolute(address), Times.Once());
    }

    [Fact]
    public void Execution_ReadAbsoluteX()
    {
        const ushort address = 0b_0000_0001;

        const byte value = 0b_0000_0010;
        const byte accumulator = 0b_0000_0010;

        var stateMock = SetupMock(0xFF, accumulator, false);

        _ = stateMock
            .Setup(s => s.Memory.ReadAbsoluteX(address))
            .Returns((false, value));

        this.Subject.Execute(stateMock.Object, address);

        stateMock.Verify(state => state.Memory.ReadAbsoluteX(address), Times.Once());
    }

    [Fact]
    public void Execution_ReadAbsoluteY()
    {
        const ushort address = 0b_0000_0001;

        const byte value = 0b_0000_0010;
        const byte accumulator = 0b_0000_0010;

        var stateMock = SetupMock(0xFB, accumulator, false);

        _ = stateMock
            .Setup(s => s.Memory.ReadAbsoluteY(address))
            .Returns((false, value));

        this.Subject.Execute(stateMock.Object, address);

        stateMock.Verify(state => state.Memory.ReadAbsoluteY(address), Times.Once());
    }

    private static Mock<ICpuState> SetupMock(byte opcode, byte accumulator, bool carry)
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
            .Returns(carry);

        return stateMock;
    }
}
