using Cpu.Execution;
using Cpu.Forms.Serialization;
using Cpu.Forms.Utils;
using Cpu.MVVM;
using Microsoft.Extensions.Logging;
using System.ComponentModel;

namespace Cpu.Forms;

public partial class CpuView : Form
{
    #region Properties
    private ILogger<CpuView> Logger { get; }

    private IMachine Machine { get; }

    private string CurrentProgram { get; set; }

    private MachineModel MachineView { get; }

    private RunningProgramModel ProgramView { get; }
    #endregion

    #region Constructors
    public CpuView(
        ILogger<CpuView> logger,
        IMachine machine)
    {
        this.Logger = logger;
        this.Machine = machine;

        this.MachineView = new MachineModel(machine);
        this.ProgramView = new RunningProgramModel();

        this.MachineView.State.PropertyChanged += this.OnStateUpdate;

        this.InitializeComponent();
        this.BindState();
        this.BindFlags();
        this.BindProgram();
        this.BindRegisters();
    }
    #endregion

    #region Updates
    private void BindProgram()
    {
        this.programText.BindTo(
                this.ProgramView,
                nameof(RunningProgramModel.Bytes));

        this.executionContent.BindTo(
                this.ProgramView,
                nameof(RunningProgramModel.Execution));
    }

    private void BindState()
    {
        this.triggerInterruptButton.BindTo(
                this.MachineView.State,
                nameof(StateModel.IsHardwareInterrupt));

        this.cyclesInput.BindTo(
                this.MachineView.State,
                nameof(StateModel.CyclesLeft));

        this.opcodeInput.BindTo(
                this.MachineView.State,
                nameof(StateModel.ExecutingOpcode));

        this.hardwareInterruptFlag.BindTo(
                this.MachineView.State,
                nameof(StateModel.IsHardwareInterrupt));

        this.softwareInterruptFlag.BindTo(
                this.MachineView.State,
                nameof(StateModel.IsSoftwareInterrupt));
    }

    private void BindFlags()
    {
        this.zeroFlag.BindTo(
            this.MachineView.State.Flags,
            nameof(FlagModel.IsZero));

        this.carryFlag.BindTo(
            this.MachineView.State.Flags,
            nameof(FlagModel.IsCarry));

        this.overflowFlag.BindTo(
            this.MachineView.State.Flags,
            nameof(FlagModel.IsOverflow));

        this.negativeFlag.BindTo(
            this.MachineView.State.Flags,
            nameof(FlagModel.IsNegative));

        this.breakFlag.BindTo(
            this.MachineView.State.Flags,
            nameof(FlagModel.IsBreakCommand));

        this.decimalFlag.BindTo(
            this.MachineView.State.Flags,
            nameof(FlagModel.IsDecimalMode));

        this.interruptFlag.BindTo(
            this.MachineView.State.Flags,
            nameof(FlagModel.IsInterruptDisable));
    }

    private void BindRegisters()
    {
        this.xRegisterInput.BindTo(
            this.MachineView.State.Registers,
            nameof(RegisterModel.IndexX));

        this.yRegisterInput.BindTo(
            this.MachineView.State.Registers,
            nameof(RegisterModel.IndexY));

        this.accumulatorInput.BindTo(
            this.MachineView.State.Registers,
            nameof(RegisterModel.Accumulator));

        this.stackPointerInput.BindTo(
            this.MachineView.State.Registers,
            nameof(RegisterModel.StackPointer));

        this.programCounterInput.BindTo(
            this.MachineView.State.Registers,
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
                    this.ProgramView.ProgramName,
                    this.Machine,
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
                this.EnableProgramExecution(state.State, state.ProgramPath);

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
        this.MachineView.PerformCycleCommand.Execute(null);
    }

    private void InstructionButton_Click(object sender, EventArgs e)
    {
        this.MachineView.PerformInstructionCommand.Execute(null);
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
            : this.LoadProgram(this.ProgramView.ProgramName));
    }

    private void TriggerInterruptButton_Click(object sender, EventArgs e)
    {
        this.MachineView.State.TriggerHardwareInterruptCommand.Execute(null);
    }
    #endregion

    #region View Model Events
    private void OnStateUpdate(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is StateModel model)
        {
            if (nameof(StateModel.DecodedInstruction).Equals(e.PropertyName))
            {
                if (model.DecodedInstruction is not null)
                {
                    this.ProgramView.AddInstructionCommand.Execute(model.DecodedInstruction);
                }
            }
        }
    }
    #endregion

    private async Task LoadProgram(string programName)
    {
        var bytes = await Serializer.LoadProgram(programName);
        this.EnableProgramExecution(bytes, programName);

        _ = MessageBox.Show(
            $"Program '{programName}' loaded",
            "Open program",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }

    private void EnableProgramExecution(ReadOnlyMemory<byte> bytes, string programName)
    {
        this.ProgramView.SetProgramNameCommand.Execute(programName);

        this.MachineView.LoadProgramCommand.Execute(bytes);
        this.ProgramView.LoadProgramCommand.Execute(bytes);

        this.clockButton.Enabled = true;
        this.resetButton.Enabled = true;
        this.instructionButton.Enabled = true;
        this.saveStateToolStripMenuItem.Enabled = true;
    }
}