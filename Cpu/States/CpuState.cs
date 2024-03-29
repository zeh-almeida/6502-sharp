﻿using CommunityToolkit.Diagnostics;
using Cpu.Execution;
using Cpu.Flags;
using Cpu.Memory;
using Cpu.Registers;

namespace Cpu.States;

/// <summary>
/// Implements <see cref="ICpuState"/> to keep track of the CPU state
/// </summary>
public sealed record CpuState : ICpuState
{
    #region Properties
    /// <inheritdoc/>
    public int CyclesLeft { get; private set; }

    /// <inheritdoc/>
    public byte ExecutingOpcode { get; set; }

    /// <inheritdoc/>
    public bool IsHardwareInterrupt { get; set; }

    /// <inheritdoc/>
    public bool IsSoftwareInterrupt { get; set; }

    /// <inheritdoc/>
    public DecodedInstruction? DecodedInstruction { get; private set; }

    /// <inheritdoc/>
    public IRegisterManager Registers { get; }

    /// <inheritdoc/>
    public IFlagManager Flags { get; }

    /// <inheritdoc/>
    public IMemoryManager Memory { get; }

    /// <inheritdoc/>
    public IStackManager Stack { get; }
    #endregion

    #region Constructors
    /// <summary>
    /// Instantiates a new <see cref="CpuState"/> manager
    /// </summary>
    /// <param name="flagManager">Allows manipulating the CPU flags</param>
    /// <param name="stackManager">Allows manipulating the CPU stack</param>
    /// <param name="memoryManager">Allows manipulating the CPU memory</param>
    /// <param name="registerManager">Allows manipulating the CPU registers</param>
    public CpuState(
        IFlagManager flagManager,
        IStackManager stackManager,
        IMemoryManager memoryManager,
        IRegisterManager registerManager)
    {
        this.Flags = flagManager;
        this.Stack = stackManager;
        this.Memory = memoryManager;
        this.Registers = registerManager;
    }
    #endregion

    #region Save/Load
    /// <inheritdoc/>
    public ReadOnlyMemory<byte> Save()
    {
        var registerState = this.Registers.Save();
        var memoryState = this.Memory.Save();

        var flagState = new byte[]
        {
            this.Flags.Save()
        };

        var currentState = new byte[]
        {
            (byte)this.CyclesLeft,
            this.ExecutingOpcode,
        };

        var index = 0;
        var status = new Memory<byte>(new byte[ICpuState.Length]);

        currentState.CopyTo(status.Slice(index, currentState.Length));
        index += currentState.Length;

        flagState.CopyTo(status.Slice(index, flagState.Length));
        index += flagState.Length;

        registerState.CopyTo(status.Slice(index, registerState.Length));
        index += registerState.Length;

        memoryState.CopyTo(status.Slice(index, memoryState.Length));
        // index += memoryState.Length;

        return status;
    }

    /// <inheritdoc/>
    public void Load(in ReadOnlyMemory<byte> data)
    {
        Guard.IsEqualTo(data.Length, ICpuState.Length, nameof(data));

        var flagState = data.Slice(ICpuState.FlagOffset, 1).Span[0];

        var memoryState = data[ICpuState.MemoryStateOffset..];

        var registerState = data.Slice(ICpuState.RegisterOffset,
                                       IRegisterManager.RegisterLengthBytes);

        this.Flags.Load(flagState);
        this.Memory.Load(memoryState);
        this.Registers.Load(registerState);

        this.CyclesLeft = data.Span[0];
        this.ExecutingOpcode = data.Span[1];

        this.IsHardwareInterrupt = false;
        this.IsSoftwareInterrupt = false;

        this.DecodedInstruction = null;
    }
    #endregion

    #region Cycles
    /// <inheritdoc/>
    public void PrepareCycle()
    {
        this.ExecutingOpcode = 0;
        this.CyclesLeft = 0;
    }

    /// <inheritdoc/>
    public void DecrementCycle()
    {
        this.CyclesLeft--;
    }

    /// <inheritdoc/>
    public void IncrementCycles(in int amount)
    {
        Guard.IsGreaterThan(amount, 0);
        this.CyclesLeft += amount;
    }

    /// <inheritdoc/>
    public void SetCycleInterrupt()
    {
        this.CyclesLeft += ICpuState.InterruptCycleCount;
    }
    #endregion

    /// <inheritdoc/>
    public void SetExecutingInstruction(in DecodedInstruction decoded)
    {
        Guard.IsNotNull(decoded);

        this.DecodedInstruction = decoded;
        this.ExecutingOpcode = decoded.Information.Opcode;

        this.IncrementCycles(decoded.Information.MinimumCycles);
        this.DecrementCycle();
    }

    /// <inheritdoc/>
    public bool IsProgramRunning()
    {
        return !ushort.MaxValue.Equals(this.Registers.ProgramCounter);
    }

    /// <inheritdoc/>
    public void AdvanceProgramCount(in DecodedInstruction decoded)
    {
        Guard.IsNotNull(decoded);
        this.Registers.ProgramCounter += decoded.Information.Bytes;
    }
}
