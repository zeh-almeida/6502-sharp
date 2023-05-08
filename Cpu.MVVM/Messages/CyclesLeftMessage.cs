using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Cpu.MVVM.Messages;

/// <summary>
/// Definition of a CPU cycle message.
/// Displays the current amount of cycles left.
/// </summary>
public sealed class CyclesLeftMessage : RequestMessage<int>
{
}
