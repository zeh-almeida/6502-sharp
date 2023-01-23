using CommunityToolkit.Mvvm.Messaging.Messages;
using Cpu.States;

namespace Cpu.MVVM.Messages;

public sealed class StateUpdateMessage : ValueChangedMessage<ICpuState>
{
    #region Constructors
    public StateUpdateMessage(ICpuState state)
        : base(state)
    {
    }
    #endregion
}
