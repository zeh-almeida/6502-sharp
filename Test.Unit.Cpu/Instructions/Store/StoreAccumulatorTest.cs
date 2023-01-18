
using Cpu.Extensions;
using Cpu.Instructions.Exceptions;
using Cpu.Instructions.Store;
using Cpu.States;
using Moq;
using Test.Unit.Cpu.Utils;
using Xunit;

namespace Test.Unit.Cpu.Instructions.Store;

public sealed record StoreAccumulatorTest
{
    #region Properties
    private StoreAccumulator Subject { get; }
    #endregion

    #region Constructors
    public StoreAccumulatorTest()
    {
        this.Subject = new StoreAccumulator();
    }
    #endregion

    [Theory]
    [InlineData(0x85)]
    [InlineData(0x95)]
    [InlineData(0x8D)]
    [InlineData(0x9D)]
    [InlineData(0x99)]
    [InlineData(0x81)]
    [InlineData(0x91)]
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
    public void Execute_ZeroPageAddress_ReadWritesValue()
    {
        const byte value = 1;
        const ushort address = 2;

        var stateMock = SetupMock(0x85, value);

        this.Subject.Execute(stateMock.Object, address);

        stateMock.Verify(state => state.Registers.Accumulator, Times.Once());

        stateMock.Verify(state => state.Memory.WriteZeroPage(address, value), Times.Once());
    }

    [Fact]
    public void Execute_ZeroPageXAddress_ReadWritesValue()
    {
        const byte value = 1;
        const ushort address = 2;
        const byte registerX = 3;
        (var valueLsb, var valueMsb) = value.SignificantBits();

        var stateMock = SetupMock(0x95, value);

        _ = stateMock
            .Setup(s => s.Registers.IndexX)
            .Returns(registerX);

        this.Subject.Execute(stateMock.Object, address);

        stateMock.Verify(state => state.Registers.Accumulator, Times.Once());
        stateMock.Verify(state => state.Memory.WriteZeroPageX(address, value), Times.Once());
    }

    [Fact]
    public void Execute_AbsoluteAddress_ReadWritesValue()
    {
        const byte value = 0;
        const ushort address = 0;

        var stateMock = SetupMock(0x8D, value);

        this.Subject.Execute(stateMock.Object, address);

        stateMock.Verify(state => state.Registers.Accumulator, Times.Once());
        stateMock.Verify(state => state.Memory.WriteAbsolute(address, value), Times.Once());
    }

    [Fact]
    public void Execute_AbsoluteXAddress_ReadWritesValue()
    {
        const byte value = 1;
        const ushort address = 2;
        const byte registerX = 3;

        var stateMock = SetupMock(0x9D, value);

        _ = stateMock
            .Setup(s => s.Registers.IndexX)
            .Returns(registerX);

        this.Subject.Execute(stateMock.Object, address);

        stateMock.Verify(state => state.Registers.Accumulator, Times.Once());
        stateMock.Verify(state => state.Memory.WriteAbsoluteX(address, value), Times.Once());
    }

    [Fact]
    public void Execute_AbsoluteYAddress_ReadWritesValue()
    {
        const byte value = 1;

        var address = ((ushort)2).MostSignificantBits();

        var stateMock = SetupMock(0x99, value);

        this.Subject.Execute(stateMock.Object, address);

        stateMock.Verify(state => state.Registers.Accumulator, Times.Once());
        stateMock.Verify(state => state.Memory.WriteAbsoluteY(address, value), Times.Once());
    }

    [Fact]
    public void Execute_IndirectXAddress_ReadWritesValue()
    {
        const byte value = 0;
        const ushort address = 1;
        const byte registerX = 0xFF;

        var stateMock = SetupMock(0x81, value);

        _ = stateMock
            .Setup(s => s.Registers.IndexX)
            .Returns(registerX);

        this.Subject.Execute(stateMock.Object, address);

        stateMock.Verify(state => state.Registers.Accumulator, Times.Once());
        stateMock.Verify(state => state.Memory.WriteIndirectX(address, value), Times.Once());
    }

    [Fact]
    public void Execute_IndirectYAddress_ReadWritesValue()
    {
        const byte value = 0;
        const ushort address = 0;
        const byte registerY = 1;

        var stateMock = SetupMock(0x91, value);

        _ = stateMock
            .Setup(s => s.Registers.IndexY)
            .Returns(registerY);

        this.Subject.Execute(stateMock.Object, address);

        stateMock.Verify(state => state.Registers.Accumulator, Times.Once());
    }

    private static Mock<ICpuState> SetupMock(byte opcode, byte value)
    {
        var stateMock = TestUtils.GenerateStateMock();

        _ = stateMock
            .Setup(s => s.ExecutingOpcode)
            .Returns(opcode);

        _ = stateMock
            .Setup(s => s.Registers.Accumulator)
            .Returns(value);

        return stateMock;
    }
}
