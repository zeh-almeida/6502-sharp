using Cpu.Maui.Utilities;
using Cpu.MVVM;

namespace Cpu.Maui;

public partial class MainPage : ContentPage
{
    #region Properties
    public CpuModel CpuModel { get; }
    #endregion

    #region Constructors
    public MainPage(CpuModel cpu)
    {
        this.InitializeComponent();
        this.CpuModel = cpu;

        this.BindingContext = this.CpuModel;

        this.BindState();
        this.BindFlags();
        this.BindProgram();
        this.BindRegisters();
    }
    #endregion

    #region Bindings
    private void BindProgram()
    {
        this.ProgramText.BindingContext = this.CpuModel.Program;
        this.ProgramText.SetBinding(Label.TextProperty, nameof(RunningProgramModel.Bytes));
    }

    private void BindState()
    {

    }

    private void BindFlags()
    {

    }

    private void BindRegisters()
    {

    }
    #endregion

    #region Events
    private async void OnLoadProgram(object sender, EventArgs e)
    {
        var bytes = await FileSelector.LoadProgram(PickOptions.Default);

        this.CpuModel.Machine.LoadProgramCommand.Execute(bytes);
        this.CpuModel.Program.LoadProgramCommand.Execute(bytes);
    }

    private void OnRunCycle(object sender, EventArgs e)
    {
        this.CpuModel.Machine.PerformCycleCommand.Execute(null);
    }

    private void OnExecuteInstruction(object sender, EventArgs e)
    {
        this.CpuModel.Machine.PerformInstructionCommand.Execute(null);
    }

    private void OnHardwareInterrupt(object sender, EventArgs e)
    {
        this.CpuModel.State.TriggerHardwareInterruptCommand.Execute(null);
    }
    #endregion
}