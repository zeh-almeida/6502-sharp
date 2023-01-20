using Cpu.Execution;
using Cpu.Extensions;
using Cpu.Forms.Serialization;
using Cpu.Forms.Utils;
using Cpu.MVVM;
using Cpu.States;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Text;

namespace Cpu.Forms;

public partial class CpuView : Form
{
    #region Properties
    private ILogger<CpuView> Logger { get; }

    private IMachine Machine { get; }

    private string CurrentProgram { get; set; }

    private MachineModel MachineView { get; }

    private StateModel StateView { get; }

    private FlagModel FlagView { get; }
    #endregion

    #region Constructors
    public CpuView(
        ILogger<CpuView> logger,
        IMachine machine)
    {
        this.Logger = logger;
        this.Machine = machine;
        this.CurrentProgram = string.Empty;

        this.FlagView = new FlagModel();
        this.StateView = new StateModel();

        this.MachineView = new MachineModel(machine);

        this.StateView.PropertyChanged += this.OnStateUpdate;
        this.MachineView.PropertyChanged += this.OnMachineUpdate;

        this.InitializeComponent();
        this.BindState();
        this.BindFlags();
    }
    #endregion

    #region Updates
    private void BindState()
    {
        this.cyclesInput.BindTo(
                this.StateView,
                nameof(StateModel.CyclesLeft));

        this.opcodeInput.BindTo(
                this.StateView,
                nameof(StateModel.ExecutingOpcode));

        this.hardwareInterruptFlag.BindTo(
                this.StateView,
                nameof(StateModel.IsHardwareInterrupt));

        this.softwareInterruptFlag.BindTo(
                this.StateView,
                nameof(StateModel.IsSoftwareInterrupt));
    }

    private void UpdateControls()
    {
        this.UpdateRegisters();
    }

    private void BindFlags()
    {
        this.zeroFlag.BindTo(this.FlagView, nameof(FlagModel.IsZero));
        this.carryFlag.BindTo(this.FlagView, nameof(FlagModel.IsCarry));
        this.overflowFlag.BindTo(this.FlagView, nameof(FlagModel.IsOverflow));
        this.negativeFlag.BindTo(this.FlagView, nameof(FlagModel.IsNegative));
        this.breakFlag.BindTo(this.FlagView, nameof(FlagModel.IsBreakCommand));
        this.decimalFlag.BindTo(this.FlagView, nameof(FlagModel.IsDecimalMode));
        this.interruptFlag.BindTo(this.FlagView, nameof(FlagModel.IsInterruptDisable));
    }

    private void UpdateRegisters()
    {
        var registers = this.Machine.State.Registers;

        this.xRegisterInput.Text = registers.IndexX.AsHex();
        this.yRegisterInput.Text = registers.IndexY.AsHex();
        this.accumulatorInput.Text = registers.Accumulator.AsHex();
        this.stackPointerInput.Text = registers.StackPointer.AsHex();
        this.programCounterInput.Text = registers.ProgramCounter.AsHex();
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
                await Serializer.SaveState(this.CurrentProgram, this.Machine, programDialog.SelectedPath)
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
            finally
            {
                this.UpdateControls();
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
            : this.LoadProgram(this.CurrentProgram));
    }

    private void TriggerInterruptButton_Click(object sender, EventArgs e)
    {
        this.StateView.TriggerHardwareInterruptCommand.Execute(this.MachineView.State);
    }
    #endregion

    #region View Model Events
    private void OnMachineUpdate(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is MachineModel model)
        {
            this.StateView.Update(model.State);
            this.FlagView.Update(model.State.Flags);
        }

        this.UpdateControls();
    }

    private void OnStateUpdate(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is StateModel model)
        {
            if (0.Equals(model.CyclesLeft))
            {
                this.executionContent.AppendText($"{model.DecodedInstruction}{Environment.NewLine}");
                this.triggerInterruptButton.Enabled = !model.IsHardwareInterrupt;
            }
        }

        this.UpdateControls();
    }
    #endregion

    private async Task LoadProgram(string programName)
    {
        var bytes = await Serializer.LoadProgram(programName);
        this.EnableProgramExecution(bytes, programName);
        this.UpdateControls();

        _ = MessageBox.Show(
            $"Program '{programName}' loaded",
            "Open program",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }

    private void EnableProgramExecution(ReadOnlyMemory<byte> bytes, string programName)
    {
        this.CurrentProgram = programName;

        this.ShowProgramContent(bytes.Span);
        this.executionContent.Text = string.Empty;

        this.clockButton.Enabled = true;
        this.resetButton.Enabled = true;
        this.instructionButton.Enabled = true;
        this.saveStateToolStripMenuItem.Enabled = true;

        this.MachineView.LoadProgramCommand.Execute(bytes);
    }

    private void ShowProgramContent(ReadOnlySpan<byte> program)
    {
        var builder = new StringBuilder(program.Length * 4);

        foreach (var value in program[ICpuState.MemoryStateOffset..])
        {
            _ = builder.AppendLine(value.AsHex());
        }

        this.programText.Text = builder.ToString();
        _ = builder.Clear();
    }
}