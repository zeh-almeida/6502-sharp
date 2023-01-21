using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Cpu.Execution;
using Cpu.States;

namespace Cpu.MVVM;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
/// <summary>
/// View Model representation of a <see cref="IMachine"/>
/// </summary>
public partial class MachineModel : ObservableObject
{
    #region Attributes
    /// <inheritdoc cref="IMachine.State"/>
    [ObservableProperty]
    private ICpuState _state;

    /// <summary>
    /// Indicates if the last cycle was a success
    /// </summary>
    [ObservableProperty]
    private bool _cycleSuccessful;
    #endregion

    #region Properties
    private IMachine Machine { get; }
    #endregion

    #region Constructors
    /// <summary>
    /// Instantiates the View Model
    /// </summary>
    /// <param name="machine"><see cref="IMachine"/> to be represented</param>
    public MachineModel(IMachine machine)
    {
        this.Machine = machine;
        this.State = this.Machine.State;
    }
    #endregion

    /// <summary>
    /// Performs a CPU Cycle
    /// </summary>
    [RelayCommand]
    public void PerformCycle()
    {
        this.CycleSuccessful = this.Machine.Cycle(state => this.State = state);
    }

    /// <summary>
    /// Performs all the cycles of a single instruction execution
    /// </summary>
    public void PerformInstruction()
    {
        do
        {
            this.PerformCycleCommand.Execute(null);

            if (0.Equals(this.State.CyclesLeft))
            {
                this.CycleSuccessful = false;
            }
        } while (this.CycleSuccessful);
    }

    /// <summary>
    /// Loads a new program into the machine
    /// </summary>
    /// <param name="data">Program to be loaded</param>
    /// <see cref="IMachine.Load(ReadOnlyMemory{byte})"/>
    [RelayCommand]
    public void LoadProgram(ReadOnlyMemory<byte> data)
    {
        this.Machine.Load(data);
        this.State = this.Machine.State;
    }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
