using Cpu.Forms.Serialization;
using Cpu.Forms.Utils;
using Cpu.MVVM;
using Microsoft.Extensions.Logging;

namespace Cpu.Forms;

/// <summary>
/// <see cref="Form"/> definition to display <see cref="Cpu.MVVM.CpuModel"/> data
/// </summary>
public partial class CpuView : Form
{
    #region Properties
    private ILogger<CpuModel> Logger { get; }

    private CpuModel CpuModel { get; }
    #endregion

    #region Constructors
    /// <summary>
    /// Instantiates a new CpuView
    /// </summary>
    /// <param name="logger">Logger for debugging</param>
    /// <param name="cpuModel">Model to be displayed in the form</param>
    public CpuView(
        ILogger<CpuModel> logger,
        CpuModel cpuModel)
    {
        this.Logger = logger;
        this.CpuModel = cpuModel;

        this.InitializeComponent();

        this.BindState();
        this.BindFlags();
        this.BindProgram();
        this.BindRegisters();
    }
    #endregion

    #region Bindings
    private void BindProgram()
    {
        this.programText.BindTo(
                this.CpuModel.Program,
                nameof(RunningProgramModel.Bytes));

        this.executionContent.BindTo(
                this.CpuModel.Program,
                nameof(RunningProgramModel.Execution));

        this.clockButton.BindTo(
                this.CpuModel.Program,
                nameof(RunningProgramModel.ProgramLoaded));

        this.resetButton.BindTo(
                this.CpuModel.Program,
                nameof(RunningProgramModel.ProgramLoaded));

        this.instructionButton.BindTo(
                this.CpuModel.Program,
                nameof(RunningProgramModel.ProgramLoaded));

        this.saveStateToolStripMenuItem.BindTo(
                this.CpuModel.Program,
                nameof(RunningProgramModel.ProgramLoaded));
    }

    private void BindState()
    {
        this.triggerInterruptButton.BindTo(
                this.CpuModel.State,
                nameof(StateModel.IsHardwareInterrupt));

        this.cyclesInput.BindTo(
                this.CpuModel.State,
                nameof(StateModel.CyclesLeft));

        this.opcodeInput.BindTo(
                this.CpuModel.State,
                nameof(StateModel.ExecutingOpcode));

        this.hardwareInterruptFlag.BindTo(
                this.CpuModel.State,
                nameof(StateModel.IsHardwareInterrupt));

        this.softwareInterruptFlag.BindTo(
                this.CpuModel.State,
                nameof(StateModel.IsSoftwareInterrupt));
    }

    private void BindFlags()
    {
        this.zeroFlag.BindTo(
            this.CpuModel.Flags,
            nameof(FlagModel.IsZero));

        this.carryFlag.BindTo(
            this.CpuModel.Flags,
            nameof(FlagModel.IsCarry));

        this.overflowFlag.BindTo(
            this.CpuModel.Flags,
            nameof(FlagModel.IsOverflow));

        this.negativeFlag.BindTo(
            this.CpuModel.Flags,
            nameof(FlagModel.IsNegative));

        this.breakFlag.BindTo(
            this.CpuModel.Flags,
            nameof(FlagModel.IsBreakCommand));

        this.decimalFlag.BindTo(
            this.CpuModel.Flags,
            nameof(FlagModel.IsDecimalMode));

        this.interruptFlag.BindTo(
            this.CpuModel.Flags,
            nameof(FlagModel.IsInterruptDisable));
    }

    private void BindRegisters()
    {
        this.xRegisterInput.BindTo(
            this.CpuModel.Registers,
            nameof(RegisterModel.IndexX));

        this.yRegisterInput.BindTo(
            this.CpuModel.Registers,
            nameof(RegisterModel.IndexY));

        this.accumulatorInput.BindTo(
            this.CpuModel.Registers,
            nameof(RegisterModel.Accumulator));

        this.stackPointerInput.BindTo(
            this.CpuModel.Registers,
            nameof(RegisterModel.StackPointer));

        this.programCounterInput.BindTo(
            this.CpuModel.Registers,
            nameof(RegisterModel.ProgramCounter));
    }
    #endregion

    #region Events
    private async void OpenToolStripMenuItem_Click(object sender, EventArgs e)
    {
        using var programDialog = new OpenFileDialog()
        {
            Title = "Open program",
            Filter = "program files (*.prg)|*.prg|binary files (*.bin)|*.bin",
        };

        var result = programDialog.ShowDialog();

        if (!DialogResult.OK.Equals(result))
        {
            _ = MessageBox.Show(
                "No program was loaded, keeping current machine state",
                "Open program",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            return;
        }

        try
        {
            await this.LoadProgram(programDialog.FileName);
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, "{programPath}", programDialog.SafeFileName);

            _ = MessageBox.Show(
                "Could not load program, check log for more information",
                "Open program",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private async void SaveStateToolStripMenuItem_Click(object sender, EventArgs e)
    {
        using var programDialog = new FolderBrowserDialog()
        {
            Description = "Save state",
            UseDescriptionForTitle = true,
        };

        var result = programDialog.ShowDialog();

        if (DialogResult.OK.Equals(result))
        {
            try
            {
                await Serializer.SaveState(
                    this.CpuModel.Program.ProgramName,
                    this.CpuModel.Machine.SaveState(),
                    programDialog.SelectedPath)
                    .ConfigureAwait(false);

                _ = MessageBox.Show(
                    $"State saved at '{programDialog.SelectedPath}'",
                    "Save state",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex, "{programPath}", programDialog.SelectedPath);

                _ = MessageBox.Show(
                    "Could not save state, check log for more information",
                    "Open program",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }

    private async void LoadStateToolStripMenuItem_Click(object sender, EventArgs e)
    {
        using var programDialog = new OpenFileDialog()
        {
            Title = "Load state",
            Filter = "6502 Emulator State files (*.state)|*.state",
        };

        var result = programDialog.ShowDialog();

        if (DialogResult.OK.Equals(result))
        {
            try
            {
                var state = await Serializer.LoadState(programDialog.FileName);
                await this.EnableProgramExecution(state.State, state.ProgramPath);

                _ = MessageBox.Show(
                    $"Loaded state from '{programDialog.FileName}'",
                    "Load state",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex, "{programPath}", programDialog.FileName);

                _ = MessageBox.Show(
                    "Could not load state, check log for more information",
                    "Open program",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }

    private void ClockButton_Click(object sender, EventArgs e)
    {
        this.CpuModel.Machine.PerformCycleCommand.Execute(null);
    }

    private void InstructionButton_Click(object sender, EventArgs e)
    {
        this.CpuModel.Machine.PerformInstructionCommand.Execute(null);
    }

    private async void ResetButton_Click(object sender, EventArgs e)
    {
        var result = MessageBox.Show(
                       "Do you want to reset the machine?",
                       "Reset machine",
                       MessageBoxButtons.OKCancel,
                       MessageBoxIcon.Question);

        await (!DialogResult.OK.Equals(result)
            ? Task.CompletedTask
            : this.LoadProgram(this.CpuModel.Program.ProgramName));
    }

    private void TriggerInterruptButton_Click(object sender, EventArgs e)
    {
        this.CpuModel.State.TriggerHardwareInterruptCommand.Execute(null);
    }
    #endregion

    private async Task LoadProgram(string programName)
    {
        var bytes = await Serializer.LoadProgram(programName);
        await this.EnableProgramExecution(bytes, programName);

        _ = MessageBox.Show(
            $"Program '{programName}' loaded",
            "Open program",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }

    private async Task EnableProgramExecution(ReadOnlyMemory<byte> bytes, string programName)
    {
        this.CpuModel.Program.SetProgramNameCommand.Execute(programName);
        await this.CpuModel.LoadProgramCommand.ExecuteAsync(bytes);
    }
}