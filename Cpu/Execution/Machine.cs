﻿using Cpu.Execution.Exceptions;
using Cpu.Extensions;
using Cpu.States;
using Microsoft.Extensions.Logging;

namespace Cpu.Execution;

/// <summary>
/// Implements the <see cref="IMachine"/> interface
/// </summary>
public sealed record Machine : IMachine
{
    #region Constants
    private const ushort HardwareInterruptAddress = 0xFFFA;

    private const ushort SoftwareInterruptAddress = 0xFFFE;
    #endregion

    #region Properties
    /// <inheritdoc/>
    public ICpuState State { get; }

    private IDecoder Decoder { get; }

    private ILogger<Machine> Logger { get; }
    #endregion

    #region Constructors
    /// <summary>
    /// Instantiates a new machine
    /// </summary>
    /// <param name="logger"><see cref="ILogger{TCategoryName}"/> to log machine operations</param>
    /// <param name="state"><see cref="ICpuState"/> to maintain the execution state</param>
    /// <param name="decoder"><see cref="IDecoder"/> to decode instructions</param>
    public Machine(
        ILogger<Machine> logger,
        ICpuState state,
        IDecoder decoder)
    {
        this.State = state;
        this.Logger = logger;
        this.Decoder = decoder;
    }
    #endregion

    /// <inheritdoc/>
    public bool Cycle(Action<ICpuState> afterCycle)
    {
        var result = this.Cycle();
        afterCycle(this.State);

        return result;
    }

    /// <inheritdoc/>
    public bool Cycle()
    {
        if (this.State.CyclesLeft > 0)
        {
            this.State.DecrementCycle();
            return true;
        }
        else
        {
            return this.State.IsProgramRunning()
                && this.Execute();
        }
    }

    /// <inheritdoc/>
    public void Load(ReadOnlyMemory<byte> data)
    {
        this.Logger.LogInformation(MachineEvents.OnLoadData, "{dataLength}", data.Length);
        this.State.Load(data);
    }

    /// <inheritdoc/>
    public ReadOnlyMemory<byte> Save()
    {
        this.Logger.LogInformation(MachineEvents.OnSaveData, "Save state");
        return this.State.Save();
    }

    #region Interrupts
    /// <inheritdoc/>
    public void ProcessInterrupts()
    {
        this.ProcessHardwareInterrupt();
        this.ProcessSoftwareInterrupt();
    }

    private void ProcessHardwareInterrupt()
    {
        if (this.State.IsHardwareInterrupt)
        {
            this.Logger.LogInformation(MachineEvents.OnInterrupt, "{InterruptType}", "Hardware");
            this.CommonInterrupt(false, HardwareInterruptAddress);

            this.State.IsHardwareInterrupt = false;
            this.State.IsSoftwareInterrupt = false;
        }
    }

    private void ProcessSoftwareInterrupt()
    {
        if (this.State.IsSoftwareInterrupt && !this.State.Flags.IsInterruptDisable)
        {
            this.Logger.LogInformation(MachineEvents.OnInterrupt, "{InterruptType}", "Software");
            this.CommonInterrupt(true, SoftwareInterruptAddress);

            this.State.IsSoftwareInterrupt = false;
        }
    }

    private void CommonInterrupt(bool isBreak, ushort address)
    {
        this.State.Flags.IsBreakCommand = isBreak;
        var bits = this.State.Flags.Save();

        this.State.Stack.Push(bits);
        this.State.Stack.Push16(this.State.Registers.ProgramCounter);

        // Adds cycles of fetching and pushing values.
        // It is the same amount of cycles used by the 0x00 BRK Instruction,
        // without the single decode cycle
        this.State.SetCycleInterrupt();
        this.State.Flags.IsInterruptDisable = true;

        this.LoadInterruptProgramAddress(address);
    }

    private void LoadInterruptProgramAddress(ushort address)
    {
        var upperAddress = (ushort)(address + 1);

        var lsb = this.State.Memory.ReadAbsolute(address);
        var msb = this.State.Memory.ReadAbsolute(upperAddress);

        this.State.Registers.ProgramCounter = lsb.CombineBytes(msb);
    }
    #endregion

    private bool Execute()
    {
        var result = true;

        try
        {
            this.State.PrepareCycle();
            this.ProcessInterrupts();

            var decoded = this.DecodeStream();

            this.State.AdvanceProgramCount(decoded);
            this.ExecuteDecoded(decoded);
        }
        catch (ProgramExecutionExeption ex)
        {
            this.Logger.LogError(MachineEvents.OnExecute, ex, "Failed to execute clock");
            result = false;
        }

        return result;
    }

    private DecodedInstruction DecodeStream()
    {
        try
        {
            var result = this.Decoder.Decode(this.State);

            this.Logger.LogInformation(MachineEvents.OnDecode, "{Instruction} @ {ProgramCounter}",
                result, this.State.Registers.ProgramCounter.AsHex());

            return result;
        }
        catch (Exception ex)
        {
            throw new ProgramExecutionExeption("Failed to decode stream", ex);
        }
    }

    private void ExecuteDecoded(DecodedInstruction decoded)
    {
        try
        {
            this.State.SetExecutingInstruction(decoded);

            decoded.Instruction.Execute(this.State, decoded.ValueParameter);
            this.State.DecrementCycle();

            this.Logger.LogInformation(MachineEvents.OnFlags, "{flagState}", this.State.Flags.ToString());
            this.Logger.LogInformation(MachineEvents.OnRegisters, "{registerState}", this.State.Registers.ToString());
        }
        catch (Exception ex)
        {
            throw new ProgramExecutionExeption("Failed to execute instruction", ex);
        }
    }
}
