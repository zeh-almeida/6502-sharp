using Cpu.Execution;
using Cpu.Extensions;
using Cpu.Maui.Models;
using Cpu.Maui.Serialization;
using Cpu.States;
using System.Text;

namespace Cpu.Maui
{
    public partial class MainPage : ContentPage
    {
        #region Constants
        private static FilePickerFileType ProgramFileType { get; } = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.Android, new[] { "application/octet-stream" } }, // MIME type
                    { DevicePlatform.WinUI, new[] { ".bin", ".prg" } }, // file extension
                });

        private static FilePickerFileType StateFileType { get; } = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.Android, new[] { "application/octet-stream" } }, // MIME type
                    { DevicePlatform.WinUI, new[] { ".state" } }, // file extension
                });
        #endregion

        #region Properties
        private IMachine Machine { get; }

        private string CurrentProgram { get; set; }
        #endregion

        #region Constructors
        public MainPage(IMachine machine)
        {
            this.Machine = machine;
            this.InitializeComponent();
        }
        #endregion

        #region Menu Events
        private async void OnOpen(object sender, EventArgs e)
        {
            PickOptions options = new()
            {
                PickerTitle = "Please select a program file",
                FileTypes = ProgramFileType,
            };

            var result = await FilePicker.PickAsync(options);

            if (result is not null)
            {
                await this.LoadProgram(result.FullPath);
            }
            else
            {
                await this.DisplayAlert("Alert",
                    "Could not load desired program",
                    "OK");
            }
        }

        private async void OnSaveState(object sender, EventArgs e)
        {
            await this.DisplayAlert("Alert", "OnSaveState", "OK");
        }

        private async void OnLoadState(object sender, EventArgs e)
        {
            PickOptions options = new()
            {
                PickerTitle = "Please select a State file",
                FileTypes = StateFileType,
            };

            var result = await FilePicker.PickAsync(options);
            if (result is not null)
            {
                try
                {
                    var state = await Serializer.LoadState(result.FullPath);
                    this.EnableProgramExecution(state.State, state.ProgramPath);

                    await this.DisplayAlert("State Loaded",
                        $"Loaded state from '{result.FullPath}'",
                        "OK");
                }
                catch (Exception _ex)
                {
                    await this.DisplayAlert("State Error",
                        "Could not load state, check log for more information",
                        "OK");
                }
            }
            else
            {
                await this.DisplayAlert("Alert",
                    "Could not load desired program state",
                    "OK");
            }
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
                //this.triggerInterruptButton.Enabled = true;
            }
        }

        private void UpdateFlags()
        {
            if (this.BindingContext is FlagModel model)
            {
                model.Update(this.Machine);
            }
        }

        private void UpdateRegisters()
        {
            //var registers = this.Machine.State.Registers;

            //this.xRegisterInput.Text = registers.IndexX.AsHex();
            //this.yRegisterInput.Text = registers.IndexY.AsHex();
            //this.accumulatorInput.Text = registers.Accumulator.AsHex();
            //this.stackPointerInput.Text = registers.StackPointer.AsHex();
            //this.programCounterInput.Text = registers.ProgramCounter.AsHex();
        }

        private void UpdateState()
        {
            //var state = this.Machine.State;

            //this.cyclesInput.Text = state.CyclesLeft.ToString();
            //this.opcodeInput.Text = state.ExecutingOpcode.AsHex();

            //this.hardwareInterruptFlag.Checked = state.IsHardwareInterrupt;
            //this.softwareInterruptFlag.Checked = state.IsSoftwareInterrupt;
        }

        private void UpdateExecution()
        {
            if (0.Equals(this.Machine.State.CyclesLeft))
            {
                //this.executionContent.AppendText($"Decoded: {this.Machine.State.DecodedInstruction}{Environment.NewLine}");
            }
        }
        #endregion

        #region Machine Actions
        private bool PerformMachineCycle()
        {
            return this.Machine.Cycle(_ =>
            {
                //this.UpdateControls();
                //this.UpdateExecution();
            });
        }

        private async Task LoadProgram(string programName)
        {
            var bytes = await Serializer.LoadProgram(programName);
            this.EnableProgramExecution(bytes, programName);

            await this.DisplayAlert("Open program",
                $"Program '{programName}' loaded",
                "OK");
        }

        private void EnableProgramExecution(IEnumerable<byte> bytes, string programName)
        {
            this.CurrentProgram = programName;
            _ = Application.Current.MainPage.Dispatcher.Dispatch(() =>
            {
                //this.ProgramNameLabel.Text = this.CurrentProgram;

                this.ShowProgramContent(bytes);
                //this.executionContent.Text = string.Empty;

                //this.clockButton.Enabled = true;
                //this.resetButton.Enabled = true;
                //this.instructionButton.Enabled = true;
                this.SaveStateButton.IsEnabled = true;

                this.Machine.Load(bytes);
                this.UpdateControls();
            });
        }

        private void ShowProgramContent(IEnumerable<byte> program)
        {
            var builder = new StringBuilder(program.Count() * 4);

            foreach (var value in program.Skip(ICpuState.MemoryStateOffset))
            {
                _ = builder.AppendLine(value.AsHex());
            }

            //this.programText.Text = builder.ToString();
            _ = builder.Clear();
        }
        #endregion
    }
}