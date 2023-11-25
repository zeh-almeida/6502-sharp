using CommunityToolkit.Diagnostics;
using Cpu.Execution.Exceptions;
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

    /// <inheritdoc/>
    public bool HasCycled { get; private set; }
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
    public bool Cycle(in Action<ICpuState> afterCycle)
    {
        Guard.IsNotNull(afterCycle);

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
            this.HasCycled = true;
        }
        else
        {
            this.HasCycled = this.State.IsProgramRunning()
                && this.Execute();
        }

        return this.HasCycled;
    }

    /// <inheritdoc/>
    public void Load(in ReadOnlyMemory<byte> data)
    {
        this.Logger.LogAction(MachineEvents.LoadDataAction, data.Length);
        this.State.Load(data);
    }

    /// <inheritdoc/>
    public ReadOnlyMemory<byte> Save()
    {
        this.Logger.LogAction(MachineEvents.SaveStateAction);
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
            this.Logger.LogAction(MachineEvents.InterruptAction, "Hardware");
            this.CommonInterrupt(false, HardwareInterruptAddress);

            this.State.IsHardwareInterrupt = false;
            this.State.IsSoftwareInterrupt = false;
        }
    }

    private void ProcessSoftwareInterrupt()
    {
        if (this.State.IsSoftwareInterrupt && !this.State.Flags.IsInterruptDisable)
        {
            this.Logger.LogAction(MachineEvents.InterruptAction, "Software");
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
        catch (ProgramExecutionException ex)
        {
            this.Logger.LogAction(MachineEvents.ExecutionExceptionAction, ex);
            result = false;
        }

        return result;
    }

    private DecodedInstruction DecodeStream()
    {
        try
        {
            var result = this.Decoder.Decode(this.State);

            this.Logger.LogAction(
                MachineEvents.DecodeAction,
                default, result,
                this.State.Registers.ProgramCounter.AsHex());

            return result;
        }
        catch (Exception ex)
        {
            throw new ProgramExecutionException("Failed to decode stream", ex);
        }
    }

    private void ExecuteDecoded(in DecodedInstruction decoded)
    {
        try
        {
            this.State.SetExecutingInstruction(decoded);

            decoded.Instruction.Execute(this.State, decoded.ValueParameter);
            this.State.DecrementCycle();

            this.Logger.LogAction(MachineEvents.FlagAction, this.State.Flags);
            this.Logger.LogAction(MachineEvents.RegisterAction, this.State.Registers);
        }
        catch (Exception ex)
        {
            throw new ProgramExecutionException("Failed to execute instruction", ex);
        }
    }
}
