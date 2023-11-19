using CommunityToolkit.Mvvm.Messaging.Messages;
using Cpu.States;

namespace Cpu.MVVM.Messages;

/// <summary>
/// Definition of the Message used when the <see cref="ICpuState"/> is updated
/// </summary>
/// <remarks>
/// Instantiates a new StateUpdateMessage
/// </remarks>
public sealed class StateUpdateMessage(ICpuState state) : ValueChangedMessage<ICpuState>(state)
{
}
