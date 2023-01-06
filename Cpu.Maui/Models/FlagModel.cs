using Cpu.Execution;
using System.ComponentModel;

namespace Cpu.Maui.Models
{
    public sealed class FlagModel : INotifyPropertyChanged
    {
        #region Properties
        public PropertyHandler<bool> IsCarry => new(nameof(this.IsCarry), this.PropertyChanged);

        public PropertyHandler<bool> IsZero => new(nameof(this.IsZero), this.PropertyChanged);

        public PropertyHandler<bool> IsInterruptDisable => new(nameof(this.IsInterruptDisable), this.PropertyChanged);

        public PropertyHandler<bool> IsDecimalMode => new(nameof(this.IsDecimalMode), this.PropertyChanged);

        public PropertyHandler<bool> IsBreakCommand => new(nameof(this.IsBreakCommand), this.PropertyChanged);

        public PropertyHandler<bool> IsOverflow => new(nameof(this.IsOverflow), this.PropertyChanged);

        public PropertyHandler<bool> IsNegative => new(nameof(this.IsNegative), this.PropertyChanged);
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        public void Update(IMachine machine)
        {
            var currentState = machine.State.Flags;

            this.IsZero.Value = currentState.IsZero;
            this.IsCarry.Value = currentState.IsCarry;
            this.IsOverflow.Value = currentState.IsOverflow;
            this.IsNegative.Value = currentState.IsNegative;
            this.IsDecimalMode.Value = currentState.IsDecimalMode;
            this.IsBreakCommand.Value = currentState.IsBreakCommand;
            this.IsInterruptDisable.Value = currentState.IsInterruptDisable;
        }
    }
}
