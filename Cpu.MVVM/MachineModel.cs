using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Cpu.Execution;
using Cpu.MVVM.Messages;
using Cpu.States;

namespace Cpu.MVVM;

/// <summary>
/// View Model representation of a <see cref="IMachine"/>
/// </summary>
public partial class MachineModel : ObservableObject
{
    #region Attributes
    /// <summary>
    /// Indicates if the last cycle was a success
    /// </summary>
    [ObservableProperty]
    private bool _cycleSuccessful;
    #endregion

    #region Properties
    private IMachine Machine { get; }

    /// <summary>
    /// <see cref="StateModel"/> handling state changes
    /// </summary>
    public StateModel State { get; }
    #endregion

    #region Constructors
    /// <summary>
    /// Instantiates the View Model
    /// </summary>
    /// <param name="machine"><see cref="IMachine"/> to be represented</param>
    public MachineModel(IMachine machine)
    {
        this.Machine = machine;
        this.State = new StateModel();
    }
    #endregion

    /// <summary>
    /// Performs a CPU Cycle
    /// </summary>
    [RelayCommand]
    protected void PerformCycle()
    {
        this.CycleSuccessful = this.Machine.Cycle(SendUpdateMessage);
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
            execute = !0.Equals(this.State.CyclesLeft);
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
        SendUpdateMessage(this.Machine.State);
    }

    private static void SendUpdateMessage(ICpuState state)
    {
        _ = WeakReferenceMessenger.Default.Send(new StateUpdateMessage(state));
    }
}
