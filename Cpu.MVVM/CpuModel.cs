using CommunityToolkit.Mvvm.Messaging;
using Cpu.Execution;

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
    public CpuModel(IMessenger messenger, IMachine machine)
    {
        this.Machine = new MachineModel(messenger, machine);
        this.State = new StateModel(messenger);

        this.Flags = new FlagModel();
        this.Registers = new RegisterModel();
        this.Program = new RunningProgramModel();

        messenger.RegisterAll(this.State);
        messenger.RegisterAll(this.Flags);
        messenger.RegisterAll(this.Program);
        messenger.RegisterAll(this.Registers);
    }
    #endregion
}
