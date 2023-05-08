using CommunityToolkit.Mvvm.Messaging.Messages;
using Cpu.States;

namespace Cpu.MVVM.Messages;

/// <summary>
/// Definition of the Message used when the <see cref="ICpuState"/> is updated
/// </summary>
public sealed class StateUpdateMessage : ValueChangedMessage<ICpuState>
{
    #region Constructors
    /// <summary>
    /// Instantiates a new StateUpdateMessage
    /// </summary>
    /// <param name="state">State which triggered the message</param>
    public StateUpdateMessage(ICpuState state)
        : base(state)
    {
    }
    #endregion
}
