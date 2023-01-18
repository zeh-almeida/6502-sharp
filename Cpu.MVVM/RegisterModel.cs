using CommunityToolkit.Mvvm.ComponentModel;
using Cpu.Registers;

namespace Cpu.MVVM;

/// <summary>
/// View Model representation of a <see cref="IRegisterManager"/>
/// </summary>
public sealed class RegisterModel : ObservableObject
{
    #region Attributes
    private ushort _programCounter;

    private byte _stackPointer;

    private byte _accumulator;

    private byte _indexX;

    private byte _indexY;
    #endregion

    #region Properties
    /// <inheritdoc cref="IRegisterManager.ProgramCounter"/>
    public ushort ProgramCounter
    {
        get => this._programCounter;
        set => this.SetProperty(ref this._programCounter, value);
    }

    /// <inheritdoc cref="IRegisterManager.StackPointer"/>
    public byte StackPointer
    {
        get => this._stackPointer;
        set => this.SetProperty(ref this._stackPointer, value);
    }

    /// <inheritdoc cref="IRegisterManager.Accumulator"/>
    public byte Accumulator
    {
        get => this._accumulator;
        set => this.SetProperty(ref this._accumulator, value);
    }

    /// <inheritdoc cref="IRegisterManager.IndexX"/>
    public byte IndexX
    {
        get => this._indexX;
        set => this.SetProperty(ref this._indexX, value);
    }

    /// <inheritdoc cref="IRegisterManager.IndexY"/>
    public byte IndexY
    {
        get => this._indexY;
        set => this.SetProperty(ref this._indexY, value);
    }
    #endregion

    /// <summary>
    /// Updates the model based on the source
    /// </summary>
    /// <param name="source"><see cref="IRegisterManager"/> with the values to update from</param>
    public void Update(IRegisterManager source)
    {
        this.IndexX = source.IndexX;
        this.IndexY = source.IndexY;
        this.Accumulator = source.Accumulator;
        this.StackPointer = source.StackPointer;
        this.ProgramCounter = source.ProgramCounter;
    }
}
