using Cpu.Execution;
using Cpu.Flags;
using Cpu.Instructions.StatusChanges;
using Cpu.Memory;
using Cpu.Opcodes;
using Cpu.Registers;
using Cpu.States;
using Moq;
using Xunit;

namespace Test.Unit.Cpu.States;
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type. - Necessary for null tests

public sealed record CpuStateTest
{
    #region Properties
    private CpuState Subject { get; }

    private Mock<IFlagManager> FlagMock { get; }

    private Mock<IStackManager> StackMock { get; }

    private Mock<IMemoryManager> MemoryMock { get; }

    private Mock<IRegisterManager> RegisterMock { get; }
    #endregion

    #region Constructors
    public CpuStateTest()
    {
        this.FlagMock = new Mock<IFlagManager>();
        this.StackMock = new Mock<IStackManager>();
        this.MemoryMock = new Mock<IMemoryManager>();
        this.RegisterMock = new Mock<IRegisterManager>();

        this.Subject = new CpuState(
            this.FlagMock.Object,
            this.StackMock.Object,
            this.MemoryMock.Object,
            this.RegisterMock.Object);
    }
    #endregion

    #region Properties
    [Fact]
    public void Property_RegisterManager_Exists()
    {
        Assert.NotNull(this.Subject.Registers);
    }

    [Fact]
    public void Property_MemoryManager_Exists()
    {
        Assert.NotNull(this.Subject.Memory);
    }

    [Fact]
    public void Property_StackManager_Exists()
    {
        Assert.NotNull(this.Subject.Stack);
    }

    [Fact]
    public void Property_FlagManager_Exists()
    {
        Assert.NotNull(this.Subject.Flags);
    }
    #endregion

    #region Cycles
    [Fact]
    public void PrepareCycle_Executes()
    {
        this.Subject.PrepareCycle();

        Assert.Equal(0, this.Subject.CyclesLeft);
        Assert.Equal(0, this.Subject.ExecutingOpcode);
    }

    [Fact]
    public void CountCycle_Executes()
    {
        this.Subject.DecrementCycle();

        Assert.Equal(-1, this.Subject.CyclesLeft);
    }

    [Fact]
    public void SetCycleInterrupt_Executes()
    {
        this.Subject.SetCycleInterrupt();

        Assert.Equal(6, this.Subject.CyclesLeft);
    }

    [Fact]
    public void SetExecutingInstruction_Executes()
    {
        const int streamByte = 0x38;
        const int cycles = 2;
        const int bytes = 1;

        var opcodeMock = new Mock<IOpcodeInformation>();

        _ = opcodeMock.Setup(m => m.Instruction)
            .Returns(new SetCarryFlag());

        _ = opcodeMock.Setup(m => m.Opcode)
            .Returns(streamByte);

        _ = opcodeMock.Setup(m => m.Bytes)
            .Returns(bytes);

        _ = opcodeMock.Setup(m => m.MinimumCycles)
            .Returns(cycles);

        _ = opcodeMock.Setup(m => m.MaximumCycles)
            .Returns(cycles);

        var decoded = new DecodedInstruction(opcodeMock.Object, 0x00);
        this.Subject.SetExecutingInstruction(decoded);

        Assert.Equal(decoded.Cycles - 1, this.Subject.CyclesLeft);
        Assert.Equal(decoded.Opcode, this.Subject.ExecutingOpcode);
    }
    #endregion

    #region Save/Load
    [Fact]
    public void Save_Returns_CurrentState()
    {
        const byte flagState = 0b_0101_0101;

        var registerState = new byte[] {
                0b_0000_0001,
                0b_0000_0010,
                0b_0000_0100,
                0b_0000_1000,
                0b_0001_0000,
                0b_0010_0000,
            };

        var memoryState = new byte[ushort.MaxValue];
        memoryState[0] = 0b_0100_0000;

        var expected = new byte[ushort.MaxValue + 9];
        expected[2] = 0b_0101_0101;
        expected[3] = 0b_0000_0001;
        expected[4] = 0b_0000_0010;
        expected[5] = 0b_0000_0100;
        expected[6] = 0b_0000_1000;
        expected[7] = 0b_0001_0000;
        expected[8] = 0b_0010_0000;
        expected[9] = 0b_0100_0000;

        _ = this.FlagMock
            .Setup(mock => mock.Save())
            .Returns(flagState);

        _ = this.RegisterMock
            .Setup(mock => mock.Save())
            .Returns(registerState);

        _ = this.MemoryMock
            .Setup(mock => mock.Save())
            .Returns(memoryState);

        var result = this.Subject.Save();

        Assert.Equal(expected, result);
    }

    [Fact]
    public void Load_WritesMemory_NewState()
    {
        const byte flagState = 0b_0101_0101;

        var registerState = new byte[] {
                0b_0000_0001,
                0b_0000_0010,
                0b_0000_0100,
                0b_0000_1000,
                0b_0001_0000,
                0b_0010_0000,
            };

        var memoryState = new byte[ushort.MaxValue + 1];
        memoryState[0] = 0b_0100_0000;

        var expected = new byte[ICpuState.Length];
        expected[0] = 0x10;
        expected[1] = 0x12;

        expected[2] = 0b_0101_0101;
        expected[3] = 0b_0000_0001;
        expected[4] = 0b_0000_0010;
        expected[5] = 0b_0000_0100;
        expected[6] = 0b_0000_1000;
        expected[7] = 0b_0001_0000;
        expected[8] = 0b_0010_0000;
        expected[9] = 0b_0100_0000;

        this.Subject.Load(expected);

        this.FlagMock.Verify(mock => mock.Load(flagState), Times.Exactly(1));
        this.RegisterMock.Verify(mock => mock.Load(registerState), Times.Exactly(1));
        this.MemoryMock.Verify(mock => mock.Load(memoryState), Times.Exactly(1));

        Assert.Equal(0x10, this.Subject.CyclesLeft);
        Assert.Equal(0x12, this.Subject.ExecutingOpcode);
    }

    [Fact]
    public void Load_Null_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(() => this.Subject.Load(null));
    }

    [Theory]
    [InlineData(ICpuState.Length + 1)]
    [InlineData(ICpuState.Length - 1)]
    public void Load_WrongLength_Throws(int length)
    {
        var memory = new byte[length];

        _ = Assert.Throws<ArgumentOutOfRangeException>(() => this.Subject.Load(memory));
    }
    #endregion

    [Fact]
    public void GetSet_IsHardwareInterrupt_Executes()
    {
        this.Subject.IsHardwareInterrupt = true;
        Assert.True(this.Subject.IsHardwareInterrupt);

        this.Subject.IsHardwareInterrupt = false;
        Assert.False(this.Subject.IsHardwareInterrupt);
    }

    [Fact]
    public void GetSet_IsSoftwareInterrupt_Executes()
    {
        this.Subject.IsSoftwareInterrupt = true;
        Assert.True(this.Subject.IsSoftwareInterrupt);

        this.Subject.IsSoftwareInterrupt = false;
        Assert.False(this.Subject.IsSoftwareInterrupt);
    }
}
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
