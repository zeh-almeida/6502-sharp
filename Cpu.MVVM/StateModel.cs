using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
    private byte _executingOpcode;

    /// <inheritdoc cref="ICpuState.CyclesLeft"/>
    [ObservableProperty]
    private int _cyclesLeft;
    #endregion

    /// <summary>
    /// Updates the model based on the source
    /// </summary>
    /// <param name="source"><see cref="ICpuState"/> with the values to update from</param>
    public void Update(ICpuState source)
    {
        this.CyclesLeft = source.CyclesLeft;
        this.ExecutingOpcode = source.ExecutingOpcode;
        this.IsHardwareInterrupt = source.IsHardwareInterrupt;
        this.IsSoftwareInterrupt = source.IsSoftwareInterrupt;
    }

    /// <summary>
    /// Triggers a Hardware Interrupt in the CPU
    /// </summary>
    /// <param name="source"><see cref="ICpuState"/> to trigger the interrupt</param>
    [RelayCommand]
    public void TriggerHardwareInterrupt(ICpuState source)
    {
        if (!this.IsHardwareInterrupt)
        {
            source.IsHardwareInterrupt = true;
            this.Update(source);
        }
    }
}
