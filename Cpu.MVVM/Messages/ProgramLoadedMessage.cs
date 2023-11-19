using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Cpu.MVVM.Messages;

/// <summary>
/// Defines a message containing the values of a loaded program
/// </summary>
/// <remarks>
/// Instantiates a new ProgramLoadedMessage
/// </remarks>
public sealed class ProgramLoadedMessage(ReadOnlyMemory<byte> program) : ValueChangedMessage<ReadOnlyMemory<byte>>(program)
{
}
