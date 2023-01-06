using System.ComponentModel;

namespace Cpu.Maui.Models
{
    public sealed record PropertyHandler<T>
    {
        #region Properties
        public T Value
        {
            get => this._value;

            set
            {
                if (Equals(this._value, value))
                {
                    return;
                }

                this._value = value;
                this.OnPropertyChanged();
            }
        }

        private PropertyChangedEventHandler Handler { get; }

        private string PropertyName { get; }
        #endregion

        #region Variables
        private T _value;
        #endregion

        #region Constructors
        public PropertyHandler(string name, PropertyChangedEventHandler handler)
        {
            this.PropertyName = name;
            this.Handler = handler;
        }
        #endregion

        private void OnPropertyChanged()
        {
            this.Handler.Invoke(this, new PropertyChangedEventArgs(this.PropertyName));
        }
    }
}
