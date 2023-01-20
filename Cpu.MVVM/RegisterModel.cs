using CommunityToolkit.Mvvm.ComponentModel;
using Cpu.Extensions;
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
    private string _programCounter = string.Empty;

    /// <inheritdoc cref="IRegisterManager.StackPointer"/>
    [ObservableProperty]
    private string _stackPointer = string.Empty;

    /// <inheritdoc cref="IRegisterManager.Accumulator"/>
    [ObservableProperty]
    private string _accumulator = string.Empty;

    /// <inheritdoc cref="IRegisterManager.IndexX"/>
    [ObservableProperty]
    private string _indexX = string.Empty;

    /// <inheritdoc cref="IRegisterManager.IndexY"/>
    [ObservableProperty]
    private string _indexY = string.Empty;
    #endregion

    #region Constructors
    public RegisterModel()
    {
        const byte value = 0;
        var hex = value.AsHex();

        this.IndexX = hex;
        this.IndexY = hex;
        this.Accumulator = hex;
        this.StackPointer = hex;
        this.ProgramCounter = hex;
    }
    #endregion

    /// <summary>
    /// Updates the model based on the source
    /// </summary>
    /// <param name="source"><see cref="IRegisterManager"/> with the values to update from</param>
    public void Update(IRegisterManager source)
    {
        this.IndexX = source.IndexX.AsHex();
        this.IndexY = source.IndexY.AsHex();
        this.Accumulator = source.Accumulator.AsHex();
        this.StackPointer = source.StackPointer.AsHex();
        this.ProgramCounter = source.ProgramCounter.AsHex();
    }
}
