using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Cpu.Flags;
using Cpu.MVVM.Messages;

namespace Cpu.MVVM;

/// <summary>
/// View Model representation of a <see cref="IFlagManager"/>
/// </summary>
public partial class FlagModel : ObservableObject, IRecipient<StateUpdateMessage>
{
    #region Attributes
    /// <inheritdoc cref="IFlagManager.IsCarry"/>
    [ObservableProperty]
    private bool _isCarry;

    /// <inheritdoc cref="IFlagManager.IsZero"/>
    [ObservableProperty]
    private bool _isZero;

    /// <inheritdoc cref="IFlagManager.IsInterruptDisable"/>
    [ObservableProperty]
    private bool _isInterruptDisable;

    /// <inheritdoc cref="IFlagManager.IsDecimalMode"/>
    [ObservableProperty]
    private bool _isDecimalMode;

    /// <inheritdoc cref="IFlagManager.IsBreakCommand"/>
    [ObservableProperty]
    private bool _isBreakCommand;

    /// <inheritdoc cref="IFlagManager.IsOverflow"/>
    [ObservableProperty]
    private bool _isOverflow;

    /// <inheritdoc cref="IFlagManager.IsNegative"/>
    [ObservableProperty]
    private bool _isNegative;
    #endregion

    #region Messages
    public void Receive(StateUpdateMessage message)
    {
        this.UpdateCommand.Execute(message.Value.Flags);
    }
    #endregion

    #region Commands
    /// <summary>
    /// Updates the model based on the source
    /// </summary>
    /// <param name="source"><see cref="IFlagManager"/> with the values to update from</param>
    [RelayCommand]
    protected void Update(IFlagManager source)
    {
        this.IsZero = source.IsZero;
        this.IsCarry = source.IsCarry;
        this.IsOverflow = source.IsOverflow;
        this.IsNegative = source.IsNegative;
        this.IsDecimalMode = source.IsDecimalMode;
        this.IsBreakCommand = source.IsBreakCommand;
        this.IsInterruptDisable = source.IsInterruptDisable;
    }
    #endregion
}
