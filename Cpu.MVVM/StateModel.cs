using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Cpu.Execution;
using Cpu.Extensions;
using Cpu.States;

namespace Cpu.MVVM;

/// <summary>
/// View Model representation of a <see cref="ICpuState"/>
/// </summary>
public partial class StateModel : ObservableObject
{
    #region Constants
    public const string DefaultOpcode = "-";
    #endregion

    #region Attributes
    /// <inheritdoc cref="ICpuState.IsSoftwareInterrupt"/>
    [ObservableProperty]
    private bool _isSoftwareInterrupt;

    /// <inheritdoc cref="ICpuState.IsHardwareInterrupt"/>
    [ObservableProperty]
    private bool _isHardwareInterrupt;

    /// <inheritdoc cref="ICpuState.ExecutingOpcode"/>
    [ObservableProperty]
    private string _executingOpcode = string.Empty;

    /// <inheritdoc cref="ICpuState.CyclesLeft"/>
    [ObservableProperty]
    private int _cyclesLeft;

    /// <inheritdoc cref="ICpuState.DecodedInstruction"/>
    [ObservableProperty]
    private DecodedInstruction? _decodedInstruction;
    #endregion

    #region Properties
    private ICpuState? LastState { get; set; }

    /// <summary>
    /// <see cref="FlagModel"/> handling flag changes
    /// </summary>
    public FlagModel Flags { get; }

    /// <summary>
    /// <see cref="RegisterModel"/> handling register changes
    /// </summary>
    public RegisterModel Registers { get; }
    #endregion

    #region Constructors
    /// <summary>
    /// Instantiates a new view model
    /// </summary>
    public StateModel()
    {
        this.Flags = new FlagModel();
        this.Registers = new RegisterModel();

        this.ExecutingOpcode = DefaultOpcode;
    }
    #endregion

    /// <summary>
    /// Updates the model based on the source
    /// </summary>
    /// <param name="source"><see cref="ICpuState"/> with the values to update from</param>
    [RelayCommand]
    protected void Update(ICpuState source)
    {
        this.LastState = source;
        this.ExecutingOpcode = source.ExecutingOpcode.AsHex();

        this.CyclesLeft = source.CyclesLeft;
        this.DecodedInstruction = source.DecodedInstruction;
        this.IsHardwareInterrupt = source.IsHardwareInterrupt;
        this.IsSoftwareInterrupt = source.IsSoftwareInterrupt;

        this.Flags.UpdateCommand.Execute(source.Flags);
        this.Registers.UpdateCommand.Execute(source.Registers);
    }

    /// <summary>
    /// Triggers a Hardware Interrupt in the CPU
    /// </summary>
    /// <param name="source"><see cref="ICpuState"/> to trigger the interrupt</param>
    [RelayCommand(CanExecute = nameof(CanTriggerHardwareInterrupt))]
    protected void TriggerHardwareInterrupt()
    {
        if (this.LastState is not null)
        {
            this.LastState.IsHardwareInterrupt = true;
            this.Update(this.LastState);
        }
    }

    protected bool CanTriggerHardwareInterrupt()
    {
        return this.LastState is not null
            && !this.LastState.IsHardwareInterrupt;
    }
}
