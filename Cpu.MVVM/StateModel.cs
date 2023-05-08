using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Cpu.Execution;
using Cpu.Extensions;
using Cpu.MVVM.Messages;
using Cpu.States;

namespace Cpu.MVVM;

/// <summary>
/// View Model representation of a <see cref="ICpuState"/>
/// </summary>
public partial class StateModel
    : ObservableRecipient,
    IRecipient<StateUpdateMessage>,
    IRecipient<CyclesLeftMessage>
{
    #region Constants
    /// <summary>
    /// Default OPCode to show when initialized
    /// </summary>
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
    #endregion

    #region Constructors
    /// <summary>
    /// Instantiates a new view model
    /// </summary>
    public StateModel(IMessenger messenger)
        : base(messenger)
    {
        this.ExecutingOpcode = DefaultOpcode;

        this.HandleCyclesLeftMessage();
        this.HandleStateUpdateMessage();
    }
    #endregion

    #region Handlers
    private void HandleStateUpdateMessage()
    {
        this.Messenger.Register<StateModel, StateUpdateMessage>(this, static (r, m) => r.Receive(m));
    }

    private void HandleCyclesLeftMessage()
    {
        this.Messenger.Register<StateModel, CyclesLeftMessage>(this, static (r, m) => r.Receive(m));
    }
    #endregion

    #region Messages
    partial void OnDecodedInstructionChanged(DecodedInstruction? value)
    {
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        this.Broadcast<DecodedInstruction>(null, value, nameof(DecodedInstruction));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning restore CS8604 // Possible null reference argument.
    }

    /// <summary>
    /// Receives a <see cref="StateUpdateMessage"/>
    /// </summary>
    /// <param name="message">Message received</param>
    public void Receive(StateUpdateMessage message)
    {
        this.UpdateCommand.Execute(message.Value);
    }

    /// <summary>
    /// Receives a <see cref="CyclesLeftMessage"/>
    /// </summary>
    /// <param name="message">Message received</param>
    public void Receive(CyclesLeftMessage message)
    {
        message.Reply(this.CyclesLeft);
    }
    #endregion

    #region Commands
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
    }

    /// <summary>
    /// Triggers a Hardware Interrupt in the CPU
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanTriggerHardwareInterrupt))]
    protected void TriggerHardwareInterrupt()
    {
        if (this.LastState is not null)
        {
            this.LastState.IsHardwareInterrupt = true;
        }

        this.UpdateCommand.Execute(this.LastState);
    }
    #endregion

    #region Validations
    /// <summary>
    /// Checks if it is possible to trigger a hardware interrupt in the <see cref="ICpuState"/>
    /// </summary>
    /// <returns>True if possible, false otherwise</returns>
    protected bool CanTriggerHardwareInterrupt()
    {
        return this.LastState?.IsHardwareInterrupt == false;
    }
    #endregion
}
