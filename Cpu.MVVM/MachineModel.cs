﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Cpu.Execution;
using Cpu.MVVM.Messages;
using Cpu.States;

namespace Cpu.MVVM;

/// <summary>
/// View Model representation of a <see cref="IMachine"/>
/// </summary>
/// <remarks>
/// Instantiates the View Model
/// </remarks>
public partial class MachineModel(IMessenger messenger, IMachine machine)
        : ObservableRecipient(messenger),
    IRecipient<ProgramLoadedMessage>
{
    #region Attributes
    /// <summary>
    /// Indicates if the last cycle was a success
    /// </summary>
    [ObservableProperty]
    private bool _cycleSuccessful;
    #endregion

    #region Properties
    private IMachine Machine { get; } = machine;
    #endregion

    /// <summary>
    /// Saves the current state of the Machine
    /// </summary>
    /// <returns>State of the machine</returns>
    public ReadOnlyMemory<byte> SaveState()
    {
        return this.Machine.Save();
    }

    #region Messages
    /// <summary>
    /// Receives data from a new loaded program
    /// </summary>
    /// <param name="message">Contents of the loaded program</param>
    public void Receive(ProgramLoadedMessage message)
    {
        ArgumentNullException.ThrowIfNull(message, nameof(message));
        this.LoadProgramCommand.Execute(message.Value);
    }
    #endregion

    #region Commands
    /// <summary>
    /// Performs a CPU Cycle
    /// </summary>
    [RelayCommand]
    protected void PerformCycle()
    {
        this.CycleSuccessful = this.Machine.Cycle(this.SendUpdateMessage);
    }

    /// <summary>
    /// Performs all the cycles of a single instruction execution
    /// </summary>
    [RelayCommand]
    protected void PerformInstruction()
    {
        bool execute;

        do
        {
            this.PerformCycleCommand.Execute(null);
            var cyclesLeft = this.Messenger.Send<CyclesLeftMessage>();

            execute = !0.Equals(cyclesLeft);
        } while (execute);
    }

    /// <summary>
    /// Loads a new program into the machine
    /// </summary>
    /// <param name="data">Program to be loaded</param>
    /// <see cref="IMachine.Load(ReadOnlyMemory{byte})"/>
    [RelayCommand]
    protected void LoadProgram(ReadOnlyMemory<byte> data)
    {
        this.Machine.Load(data);
        this.SendUpdateMessage(this.Machine.State);
    }
    #endregion

    private void SendUpdateMessage(ICpuState state)
    {
        _ = this.Messenger.Send(new StateUpdateMessage(state));
    }
}
