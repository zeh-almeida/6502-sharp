using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Cpu.MVVM.Messages;

/// <summary>
/// Defines a message containing the values of a loaded program
/// </summary>
public sealed class ProgramLoadedMessage : ValueChangedMessage<ReadOnlyMemory<byte>>
{
    #region Constructors
    /// <summary>
    /// Instantiates a new ProgramLoadedMessage
    /// </summary>
    /// <param name="program">loaded program data</param>
    public ProgramLoadedMessage(ReadOnlyMemory<byte> program)
        : base(program)
    {
    }
    #endregion
}
