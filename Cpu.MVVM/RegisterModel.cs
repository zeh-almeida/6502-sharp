using CommunityToolkit.Mvvm.ComponentModel;
using Cpu.Registers;

namespace Cpu.MVVM;

/// <summary>
/// View Model representation of a <see cref="IRegisterManager"/>
/// </summary>
public partial class RegisterModel : ObservableObject
{
    #region Attributes
    /// <inheritdoc cref="IRegisterManager.ProgramCounter"/>
    [ObservableProperty]
    private ushort _programCounter;

    /// <inheritdoc cref="IRegisterManager.StackPointer"/>
    [ObservableProperty]
    private byte _stackPointer;

    /// <inheritdoc cref="IRegisterManager.Accumulator"/>
    [ObservableProperty]
    private byte _accumulator;

    /// <inheritdoc cref="IRegisterManager.IndexX"/>
    [ObservableProperty]
    private byte _indexX;

    /// <inheritdoc cref="IRegisterManager.IndexY"/>
    [ObservableProperty]
    private byte _indexY;
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
