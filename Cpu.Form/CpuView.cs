using Cpu.Execution;
using Cpu.Extensions;
using Cpu.Forms.Serialization;
using Microsoft.Extensions.Logging;

namespace Cpu.Forms
{
    public partial class CpuView : Form
    {
        #region Properties
        private ILogger<CpuView> Logger { get; }

        private IMachine Machine { get; }

        private string CurrentProgram { get; set; }
        #endregion

        #region Constructors
        public CpuView(
            ILogger<CpuView> logger,
            IMachine machine)
        {
            this.Logger = logger;
            this.Machine = machine;
            this.CurrentProgram = string.Empty;

            this.InitializeComponent();
        }
        #endregion

        #region Updates
        private void UpdateControls()
        {
            this.UpdateState();
            this.UpdateFlags();
            this.UpdateRegisters();

            if (!this.Machine.State.IsHardwareInterrupt)
            {
                this.triggerInterruptButton.Enabled = true;
            }
        }

        private void UpdateFlags()
        {
            var flags = this.Machine.State.Flags;

            this.zeroFlag.Checked = flags.IsZero;
            this.carryFlag.Checked = flags.IsCarry;
            this.overflowFlag.Checked = flags.IsOverflow;
            this.negativeFlag.Checked = flags.IsNegative;
            this.breakFlag.Checked = flags.IsBreakCommand;
            this.decimalFlag.Checked = flags.IsDecimalMode;
            this.interruptFlag.Checked = flags.IsInterruptDisable;
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

        private void UpdateState()
        {
            var state = this.Machine.State;

            this.cyclesInput.Text = state.CyclesLeft.ToString();
            this.opcodeInput.Text = state.ExecutingOpcode.AsHex();

            this.hardwareInterruptFlag.Checked = state.IsHardwareInterrupt;
            this.softwareInterruptFlag.Checked = state.IsSoftwareInterrupt;
        }

        private void UpdateExecution()
        {
            if (0.Equals(this.Machine.State.CyclesLeft))
            {
                this.executionContent.AppendText($"Decoded: {this.Machine.State.DecodedInstruction}{Environment.NewLine}");
            }
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

            if (DialogResult.OK.Equals(result))
            {
                try
                {
                    this.CurrentProgram = programDialog.FileName;

                    await this.LoadProgram()
                        .ConfigureAwait(false);
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
            else
            {
                _ = MessageBox.Show(
                    "No program was loaded, keeping current machine state",
                    "Open program",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
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
                    var state = await Serializer.LoadState(programDialog.FileName)
                        .ConfigureAwait(false);

                    this.CurrentProgram = state.ProgramPath;
                    this.Machine.Load(state.State);

                    this.Invoke(() => this.EnableProgramExecution());

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
            _ = this.PerformMachineCycle();
        }

        private void InstructionButton_Click(object sender, EventArgs e)
        {
            bool cycling;

            do
            {
                cycling = this.PerformMachineCycle();

                if (0.Equals(this.Machine.State.CyclesLeft))
                {
                    cycling = false;
                }

            } while (cycling);
        }

        private async void ResetButton_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                           "Do you want to reset the machine?",
                           "Reset machine",
                           MessageBoxButtons.OKCancel,
                           MessageBoxIcon.Question);

            if (DialogResult.OK.Equals(result))
            {
                await this.LoadProgram()
                    .ConfigureAwait(false);
            }
        }

        private void TriggerInterruptButton_Click(object sender, EventArgs e)
        {
            this.Machine.State.IsHardwareInterrupt = true;

            this.UpdateControls();
            this.triggerInterruptButton.Enabled = false;
        }
        #endregion

        private bool PerformMachineCycle()
        {
            return this.Machine.Cycle(_ =>
            {
                this.UpdateControls();
                this.UpdateExecution();
            });
        }

        private async Task LoadProgram()
        {
            await Serializer.LoadProgram(this.Machine, this.CurrentProgram);
            this.EnableProgramExecution();

            _ = MessageBox.Show(
                $"Program '{this.CurrentProgram}' loaded",
                "Open program",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void EnableProgramExecution()
        {
            this.UpdateControls();

            this.executionContent.Text = string.Empty;

            this.clockButton.Enabled = true;
            this.resetButton.Enabled = true;
            this.instructionButton.Enabled = true;

            this.saveStateToolStripMenuItem.Enabled = true;
        }
    }
}