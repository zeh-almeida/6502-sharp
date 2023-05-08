using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Cpu.Extensions;
using Cpu.MVVM.Messages;
using Cpu.Registers;

namespace Cpu.MVVM;

/// <summary>
/// View Model representation of a <see cref="IRegisterManager"/>
/// </summary>
public partial class RegisterModel
    : ObservableRecipient,
    IRecipient<StateUpdateMessage>
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
    /// <summary>
    /// Instantiates a new view model
    /// </summary>
    public RegisterModel(IMessenger messenger)
        : base(messenger)
    {
        const byte value = 0;
        var hex = value.AsHex();

        this.IndexX = hex;
        this.IndexY = hex;
        this.Accumulator = hex;
        this.StackPointer = hex;
        this.ProgramCounter = hex;

        this.HandleProgramLoadedMessage();
    }
    #endregion

    #region Handlers
    private void HandleProgramLoadedMessage()
    {
        this.Messenger.Register<RegisterModel, StateUpdateMessage>(this, static (r, m) => r.Receive(m));
    }
    #endregion

    #region Messages
    /// <summary>
    /// Receives a <see cref="StateUpdateMessage"/> message
    /// </summary>
    /// <param name="message">Message sent</param>
    public void Receive(StateUpdateMessage message)
    {
        this.UpdateCommand.Execute(message.Value.Registers);
    }
    #endregion

    #region Commands
    /// <summary>
    /// Updates the model based on the source
    /// </summary>
    /// <param name="source"><see cref="IRegisterManager"/> with the values to update from</param>
    [RelayCommand]
    protected void Update(IRegisterManager source)
    {
        this.IndexX = source.IndexX.AsHex();
        this.IndexY = source.IndexY.AsHex();
        this.Accumulator = source.Accumulator.AsHex();
        this.StackPointer = source.StackPointer.AsHex();
        this.ProgramCounter = source.ProgramCounter.AsHex();
    }
    #endregion
}
