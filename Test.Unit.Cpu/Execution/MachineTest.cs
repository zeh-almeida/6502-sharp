﻿using Cpu.Execution;
using Cpu.Instructions.Jumps;
using Cpu.Opcodes;
using Cpu.States;
using Microsoft.Extensions.Logging;
using Moq;
using Test.Unit.Cpu.Utils;
using Xunit;

namespace Test.Unit.Cpu.Execution;

public sealed record MachineTest
{
    #region Constants
    private const int StreamByte = 0x20;
    #endregion

    #region Properties
    private Mock<ICpuState> StateMock { get; }

    private Mock<IDecoder> DecoderMock { get; }

    private Mock<ILogger<Machine>> LoggerMock { get; }

    private DecodedInstruction Decoded { get; }

    private Machine Subject { get; }
    #endregion

    #region Constructors
    public MachineTest()
    {
        const int cycles = 6;
        const int bytes = 3;

        var opcodeMock = new Mock<IOpcodeInformation>();

        _ = opcodeMock.Setup(m => m.Opcode)
            .Returns(StreamByte);

        _ = opcodeMock.Setup(m => m.Bytes)
            .Returns(bytes);

        _ = opcodeMock.Setup(m => m.MinimumCycles)
            .Returns(cycles);

        _ = opcodeMock.Setup(m => m.MaximumCycles)
            .Returns(cycles);

        this.StateMock = TestUtils.GenerateStateMock();

        this.DecoderMock = new Mock<IDecoder>();
        this.LoggerMock = new Mock<ILogger<Machine>>();

        this.Decoded = new DecodedInstruction(opcodeMock.Object, new JumpToSubroutine(), 65535);
        this.Subject = new Machine(this.LoggerMock.Object, this.StateMock.Object, this.DecoderMock.Object);
    }
    #endregion

    [Fact]
    public void Data_Load_Successful()
    {
        var data = new byte[10];

        this.Subject.Load(data);

        this.StateMock.Verify(mock => mock.Load(data), Times.Exactly(1));
    }

    [Fact]
    public void Data_Save_Successful()
    {
        var expected = new byte[10];

        _ = this.StateMock
            .Setup(mock => mock.Save())
            .Returns(expected);

        var result = this.Subject.Save();

        Assert.Equal(expected, result);
    }

    [Fact]
    public void ProgramFinished_Cycle_IsFalse()
    {
        _ = this.StateMock
            .Setup(mock => mock.Registers.ProgramCounter)
            .Returns(ushort.MaxValue);

        var result = this.Subject.Cycle();

        Assert.False(result);
    }

    [Fact]
    public void DecodeStream_Cycle_Successful()
    {
        _ = this.StateMock
            .SetupSequence(mock => mock.IsProgramRunning())
            .Returns(true)
            .Returns(true)
            .Returns(false);

        _ = this.DecoderMock
            .Setup(mock => mock.Decode(It.Ref<ICpuState>.IsAny))
            .Returns(this.Decoded);

        var result = this.Subject.Cycle();

        Assert.True(result);
    }

    [Fact]
    public void DecodeStream_Fails_Catch()
    {
        _ = this.StateMock
            .Setup(mock => mock.CyclesLeft)
            .Returns(0);

        _ = this.StateMock
            .Setup(mock => mock.IsProgramRunning())
            .Returns(true);

        _ = this.DecoderMock
            .Setup(mock => mock.Decode(It.Ref<ICpuState>.IsAny))
            .Throws<Exception>();

        var result = this.Subject.Cycle();

        Assert.False(result);
    }

    [Fact]
    public void ExecuteDecoded_Fails_Catch()
    {
        _ = this.StateMock
            .Setup(mock => mock.CyclesLeft)
            .Returns(0);

        _ = this.StateMock
            .Setup(mock => mock.IsProgramRunning())
            .Returns(true);

        _ = this.StateMock
            .Setup(mock => mock.SetExecutingInstruction(It.Ref<DecodedInstruction>.IsAny))
            .Throws<Exception>();

        _ = this.DecoderMock
            .Setup(mock => mock.Decode(It.Ref<ICpuState>.IsAny))
            .Returns(this.Decoded);

        var result = this.Subject.Cycle();

        Assert.False(result);
    }

    [Fact]
    public void AdvanceProgramCount_Cycle_Successful()
    {
        const ushort initialProgramCounter = 65532;
        const ushort finalProgramCounter = 65535;

        _ = this.StateMock
            .SetupSequence(mock => mock.Registers.ProgramCounter)
            .Returns(initialProgramCounter)
            .Returns(initialProgramCounter)
            .Returns(finalProgramCounter);

        _ = this.StateMock
            .SetupSequence(mock => mock.IsProgramRunning())
            .Returns(true)
            .Returns(true)
            .Returns(false);

        _ = this.StateMock
            .Setup(mock => mock.ExecutingOpcode)
            .Returns(StreamByte);

        _ = this.DecoderMock
            .Setup(mock => mock.Decode(It.Ref<ICpuState>.IsAny))
            .Returns(this.Decoded);

        var result = this.Subject.Cycle();

        Assert.True(result);

        this.StateMock.Verify(mock => mock.Registers.ProgramCounter, Times.AtLeast(2));
        this.StateMock.VerifySet(mock => mock.Registers.ProgramCounter = finalProgramCounter, Times.AtLeast(1));
    }

    [Fact]
    public void RemainingCycles_Count_Successful()
    {
        var totalCycleCount = this.Decoded.Information.MaximumCycles;

        _ = this.StateMock
            .SetupSequence(mock => mock.IsProgramRunning())
            .Returns(true)
            .Returns(false);

        _ = this.StateMock
            .Setup(mock => mock.ExecutingOpcode)
            .Returns(StreamByte);

        // Counts from 4 because it takes two cycles to read the opcode
        _ = this.StateMock
            .SetupSequence(mock => mock.CyclesLeft)
            .Returns(4)
            .Returns(3)
            .Returns(2)
            .Returns(1)
            .Returns(0);

        _ = this.DecoderMock
            .Setup(mock => mock.Decode(It.Ref<ICpuState>.IsAny))
            .Returns(this.Decoded);

        var cycleCount = 0;
        var result = false;

        do
        {
            result = this.Subject.Cycle();
            cycleCount++;

            Assert.True(totalCycleCount >= cycleCount, $"{cycleCount} > {totalCycleCount}");
        } while (result);

        Assert.Equal(totalCycleCount, cycleCount);
    }

    [Fact]
    public void DecodeStream_Action_Executes()
    {
        _ = this.StateMock
            .SetupSequence(mock => mock.IsProgramRunning())
            .Returns(true)
            .Returns(true)
            .Returns(false);

        _ = this.DecoderMock
            .Setup(mock => mock.Decode(It.Ref<ICpuState>.IsAny))
            .Returns(this.Decoded);

        var executionCount = 0;
        void action(ICpuState _)
        {
            executionCount++;
        }

        var result = this.Subject.Cycle(action);

        Assert.True(result);
        Assert.Equal(1, executionCount);
    }

    [Fact]
    public void DecodeStream_Throws_Fails()
    {
        _ = this.StateMock
            .Setup(mock => mock.Registers.ProgramCounter)
            .Returns(1);

        _ = this.DecoderMock
            .Setup(mock => mock.Decode(It.Ref<ICpuState>.IsAny))
            .Throws<Exception>();

        var result = this.Subject.Cycle();

        Assert.False(result);
    }

    [Fact]
    public void ExecuteDecoded_Throws_Fails()
    {
        const ushort initialProgramCounter = 65532;
        const ushort finalProgramCounter = 65535;

        _ = this.StateMock
            .SetupSequence(mock => mock.Registers.ProgramCounter)
            .Returns(initialProgramCounter)
            .Returns(initialProgramCounter)
            .Returns(finalProgramCounter);

        _ = this.StateMock
            .Setup(mock => mock.SetExecutingInstruction(It.IsAny<DecodedInstruction>()))
            .Throws<Exception>();

        _ = this.DecoderMock
            .Setup(mock => mock.Decode(It.Ref<ICpuState>.IsAny))
            .Returns(this.Decoded);

        var result = this.Subject.Cycle();

        Assert.False(result);
    }

    #region Interrupts
    [Fact]
    public void ProcessInterrupts_AllClear_DoesNothing()
    {
        _ = this.StateMock
            .Setup(mock => mock.IsHardwareInterrupt)
            .Returns(false);

        _ = this.StateMock
            .Setup(mock => mock.IsSoftwareInterrupt)
            .Returns(false);

        this.Subject.ProcessInterrupts();

        this.StateMock.Verify(m => m.Stack.Push(It.IsAny<byte>()), Times.Never());
        this.StateMock.Verify(m => m.Stack.Push16(It.IsAny<byte>()), Times.Never());

        this.StateMock.VerifySet(m => m.Registers.ProgramCounter = It.IsAny<ushort>(), Times.Never());
    }

    [Fact]
    public void ProcessInterrupts_Hardware_Executes()
    {
        const byte flagState = 0b_1111_1111;
        const ushort programCounter = 0b_1111_0000_1111_0000;

        const byte interruptMsb = 0x11;
        const byte interruptLsb = 0x22;

        const ushort interruptPC = 0x1122;

        _ = this.StateMock
            .Setup(mock => mock.IsHardwareInterrupt)
            .Returns(true);

        _ = this.StateMock
            .Setup(mock => mock.IsSoftwareInterrupt)
            .Returns(false);

        _ = this.StateMock
            .Setup(m => m.Flags.Save())
            .Returns(flagState);

        _ = this.StateMock
            .Setup(m => m.Registers.ProgramCounter)
            .Returns(programCounter);

        _ = this.StateMock
            .Setup(m => m.Memory.ReadAbsolute(0xFFFA))
            .Returns(interruptLsb);

        _ = this.StateMock
            .Setup(m => m.Memory.ReadAbsolute(0xFFFB))
            .Returns(interruptMsb);

        this.Subject.ProcessInterrupts();

        this.StateMock.Verify(m => m.Stack.Push(flagState), Times.Once());
        this.StateMock.Verify(m => m.Stack.Push16(programCounter), Times.Once());

        this.StateMock.VerifySet(m => m.Flags.IsInterruptDisable = true, Times.Once());

        this.StateMock.VerifySet(m => m.IsHardwareInterrupt = false, Times.Once());
        this.StateMock.VerifySet(m => m.IsSoftwareInterrupt = false, Times.Once());
        this.StateMock.VerifySet(m => m.Registers.ProgramCounter = interruptPC, Times.Once());

        this.StateMock.Verify(m => m.SetCycleInterrupt(), Times.Once());
    }

    [Fact]
    public void ProcessInterrupts_Software_Executes()
    {
        const byte flagState = 0b_1111_1111;
        const ushort programCounter = 0b_1111_0000_1111_0000;

        const byte interruptMsb = 0x11;
        const byte interruptLsb = 0x22;

        const ushort interruptPC = 0x1122;

        _ = this.StateMock
            .Setup(mock => mock.IsHardwareInterrupt)
            .Returns(false);

        _ = this.StateMock
            .Setup(mock => mock.IsSoftwareInterrupt)
            .Returns(true);

        _ = this.StateMock
            .Setup(m => m.Flags.Save())
            .Returns(flagState);

        _ = this.StateMock
            .Setup(m => m.Registers.ProgramCounter)
            .Returns(programCounter);

        _ = this.StateMock
            .Setup(m => m.Memory.ReadAbsolute(0xFFFE))
            .Returns(interruptLsb);

        _ = this.StateMock
            .Setup(m => m.Memory.ReadAbsolute(0xFFFF))
            .Returns(interruptMsb);

        this.Subject.ProcessInterrupts();

        this.StateMock.Verify(m => m.Stack.Push(flagState), Times.Once());
        this.StateMock.Verify(m => m.Stack.Push16(programCounter), Times.Once());

        this.StateMock.VerifySet(m => m.Flags.IsInterruptDisable = true, Times.Once());
        this.StateMock.VerifySet(m => m.Flags.IsBreakCommand = true, Times.Once());

        this.StateMock.VerifySet(m => m.IsHardwareInterrupt = It.IsAny<bool>(), Times.Never());
        this.StateMock.VerifySet(m => m.IsSoftwareInterrupt = false, Times.Once());
        this.StateMock.VerifySet(m => m.Registers.ProgramCounter = interruptPC, Times.Once());

        this.StateMock.Verify(m => m.SetCycleInterrupt(), Times.Once());
    }
    #endregion
}
