using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Cpu.MVVM.Messages;

namespace Cpu.MVVM;

/// <summary>
/// MVVM Definition of the full CPU and Machine
/// </summary>
public partial class CpuModel
{
    #region Properties
    /// <summary>
    /// MVVM Model for the <see cref="Execution.IMachine"/>
    /// </summary>
    public MachineModel Machine { get; }

    /// <summary>
    /// MVVM Model for the <see cref="States.ICpuState"/>
    /// </summary>
    public StateModel State { get; }

    /// <summary>
    /// MVVM Model for the <see cref="Flags.IFlagManager"/>
    /// </summary>
    public FlagModel Flags { get; }

    /// <summary>
    /// MVVM Model for the <see cref="Registers.IRegisterManager"/>
    /// </summary>
    public RegisterModel Registers { get; }

    /// <summary>
    /// MVVM Model for the executing program state
    /// </summary>
    public RunningProgramModel Program { get; }

    /// <summary>
    /// Allows sending and receiving messages for MVVM
    /// </summary>
    private IMessenger Messenger { get; }
    #endregion

    #region Constructors
    /// <summary>
    /// Instantiates a new CpuModel
    /// </summary>
    /// <param name="messenger">Messenger for communications</param>
    /// <param name="machine"><see cref="MachineModel"/> view model</param>
    /// <param name="state"><see cref="StateModel"/> view model</param>
    /// <param name="flags"><see cref="FlagModel"/> view model</param>
    /// <param name="registers"><see cref="RegisterModel"/> view model</param>
    /// <param name="program"><see cref="RunningProgramModel"/> view model</param>
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

        messenger.RegisterAll(this.Flags);
        messenger.RegisterAll(this.Machine);
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
