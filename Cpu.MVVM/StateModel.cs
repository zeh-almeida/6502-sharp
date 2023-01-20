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
    #region Attributes
    /// <inheritdoc cref="ICpuState.IsSoftwareInterrupt"/>
    [ObservableProperty]
    private bool _isSoftwareInterrupt;

    /// <inheritdoc cref="ICpuState.IsHardwareInterrupt"/>
    [ObservableProperty]
    private bool _isHardwareInterrupt;

    /// <inheritdoc cref="ICpuState.ExecutingOpcode"/>
    [ObservableProperty]
    private string _executingOpcode = "";

    /// <inheritdoc cref="ICpuState.CyclesLeft"/>
    [ObservableProperty]
    private int _cyclesLeft;

    /// <inheritdoc cref="ICpuState.DecodedInstruction"/>
    [ObservableProperty]
    private DecodedInstruction? _decodedInstruction;
    #endregion

    /// <summary>
    /// Updates the model based on the source
    /// </summary>
    /// <param name="source"><see cref="ICpuState"/> with the values to update from</param>
    public void Update(ICpuState source)
    {
        this.ExecutingOpcode = source.ExecutingOpcode.AsHex();

        this.CyclesLeft = source.CyclesLeft;
        this.DecodedInstruction = source.DecodedInstruction;
        this.IsHardwareInterrupt = source.IsHardwareInterrupt;
        this.IsSoftwareInterrupt = source.IsSoftwareInterrupt;
    }

    /// <summary>
    /// Triggers a Hardware Interrupt in the CPU
    /// </summary>
    /// <param name="source"><see cref="ICpuState"/> to trigger the interrupt</param>
    [RelayCommand(CanExecute = nameof(CanTriggerHardwareInterrupt))]
    public void TriggerHardwareInterrupt(ICpuState source)
    {
        source.IsHardwareInterrupt = true;
        this.Update(source);
    }

    public static bool CanTriggerHardwareInterrupt(ICpuState source)
    {
        return !source.IsHardwareInterrupt;
    }
}
