using CommunityToolkit.Mvvm.ComponentModel;
using Cpu.Flags;

namespace Cpu.MVVM;

/// <summary>
/// View Model representation of a <see cref="IFlagManager"/>
/// </summary>
public sealed class FlagModel : ObservableObject
{
    #region Attributes
    private bool _isCarry;

    private bool _isZero;

    private bool _isInterruptDisable;

    private bool _isDecimalMode;

    private bool _isBreakCommand;

    private bool _isOverflow;

    private bool _isNegative;
    #endregion

    #region Properties
    /// <inheritdoc cref="IFlagManager.IsCarry"/>
    public bool IsCarry
    {
        get => this._isCarry;
        set => this.SetProperty(ref this._isCarry, value);
    }

    /// <inheritdoc cref="IFlagManager.IsZero"/>
    public bool IsZero
    {
        get => this._isZero;
        set => this.SetProperty(ref this._isZero, value);
    }

    /// <inheritdoc cref="IFlagManager.IsInterruptDisable"/>
    public bool IsInterruptDisable
    {
        get => this._isInterruptDisable;
        set => this.SetProperty(ref this._isInterruptDisable, value);
    }

    /// <inheritdoc cref="IFlagManager.IsDecimalMode"/>
    public bool IsDecimalMode
    {
        get => this._isDecimalMode;
        set => this.SetProperty(ref this._isDecimalMode, value);
    }

    /// <inheritdoc cref="IFlagManager.IsBreakCommand"/>
    public bool IsBreakCommand
    {
        get => this._isBreakCommand;
        set => this.SetProperty(ref this._isBreakCommand, value);
    }

    /// <inheritdoc cref="IFlagManager.IsOverflow"/>
    public bool IsOverflow
    {
        get => this._isOverflow;
        set => this.SetProperty(ref this._isOverflow, value);
    }

    /// <inheritdoc cref="IFlagManager.IsNegative"/>
    public bool IsNegative
    {
        get => this._isNegative;
        set => this.SetProperty(ref this._isNegative, value);
    }
    #endregion

    /// <summary>
    /// Updates the model based on the source
    /// </summary>
    /// <param name="source"><see cref="IFlagManager"/> with the values to update from</param>
    public void Update(IFlagManager source)
    {
        this.IsZero = source.IsZero;
        this.IsCarry = source.IsCarry;
        this.IsOverflow = source.IsOverflow;
        this.IsNegative = source.IsNegative;
        this.IsDecimalMode = source.IsDecimalMode;
        this.IsBreakCommand = source.IsBreakCommand;
        this.IsInterruptDisable = source.IsInterruptDisable;
    }
}
