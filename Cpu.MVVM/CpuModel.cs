using CommunityToolkit.Mvvm.Messaging;

namespace Cpu.MVVM;

public sealed class CpuModel
{
    #region Properties
    public MachineModel Machine { get; }

    public StateModel State { get; }

    public FlagModel Flags { get; }

    public RegisterModel Registers { get; }

    public RunningProgramModel Program { get; }
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
}
