using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Cpu.MVVM.Messages;

namespace Cpu.MVVM;

public partial class CpuModel
{
    #region Properties
    public MachineModel Machine { get; }

    public StateModel State { get; }

    public FlagModel Flags { get; }

    public RegisterModel Registers { get; }

    public RunningProgramModel Program { get; }

    private IMessenger Messenger { get; }
    #endregion

    #region Constructors
    public CpuModel(
        IMessenger messenger,
        MachineModel machine,
        StateModel state,
        FlagModel flags,
        RegisterModel registers,
        RunningProgramModel program)
    {
        this.Messenger = messenger;

        this.State = state;
        this.Flags = flags;
        this.Machine = machine;
        this.Program = program;
        this.Registers = registers;

        messenger.RegisterAll(this.State);
        messenger.RegisterAll(this.Flags);
        messenger.RegisterAll(this.Machine);
        messenger.RegisterAll(this.Program);
        messenger.RegisterAll(this.Registers);
    }
    #endregion

    #region Commands
    /// <summary>
    /// Sends a message for a loaded program
    /// </summary>
    [RelayCommand(AllowConcurrentExecutions = false)]
    protected async Task LoadProgram(ReadOnlyMemory<byte> data)
    {
        _ = await Task.Run(() => this.Messenger.Send(new ProgramLoadedMessage(data)));
    }
    #endregion
}
