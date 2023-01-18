
using Cpu.Instructions.Exceptions;
using Cpu.Instructions.Store;
using Cpu.States;
using Moq;
using Test.Unit.Cpu.Utils;
using Xunit;

namespace Test.Unit.Cpu.Instructions.Store;

public sealed record StoreRegisterYTest
{
    #region Properties
    private StoreRegisterY Subject { get; }
    #endregion

    #region Constructors
    public StoreRegisterYTest()
    {
        this.Subject = new StoreRegisterY();
    }
    #endregion

    [Theory]
    [InlineData(0x84)]
    [InlineData(0x94)]
    [InlineData(0x8C)]
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

        var stateMock = SetupMock(0x84, value);

        this.Subject.Execute(stateMock.Object, address);

        stateMock.Verify(state => state.Registers.IndexY, Times.Once());
        stateMock.Verify(state => state.Memory.WriteZeroPage(address, value), Times.Once());
    }

    [Fact]
    public void Execute_ZeroPageXAddress_ReadWritesValue()
    {
        const byte value = 1;
        const ushort address = 2;

        var stateMock = SetupMock(0x94, value);

        this.Subject.Execute(stateMock.Object, address);

        stateMock.Verify(state => state.Registers.IndexY, Times.Once());
        stateMock.Verify(state => state.Memory.WriteZeroPageX(address, value), Times.Once());
    }

    [Fact]
    public void Execute_AbsoluteAddress_ReadWritesValue()
    {
        const byte value = 0;
        const ushort address = 0;

        var stateMock = SetupMock(0x8C, value);

        this.Subject.Execute(stateMock.Object, address);

        stateMock.Verify(state => state.Registers.IndexY, Times.Once());
        stateMock.Verify(state => state.Memory.WriteAbsolute(address, value), Times.Once());
    }

    private static Mock<ICpuState> SetupMock(byte opcode, byte registerY)
    {
        var stateMock = TestUtils.GenerateStateMock();

        _ = stateMock
            .Setup(s => s.ExecutingOpcode)
            .Returns(opcode);

        _ = stateMock
            .Setup(s => s.Registers.IndexY)
            .Returns(registerY);

        return stateMock;
    }
}
