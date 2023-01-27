using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Cpu.MVVM.Messages;

public sealed class ProgramLoadedMessage : ValueChangedMessage<ReadOnlyMemory<byte>>
{
    #region Constructors
    public ProgramLoadedMessage(ReadOnlyMemory<byte> program)
        : base(program)
    {
    }
    #endregion
}
