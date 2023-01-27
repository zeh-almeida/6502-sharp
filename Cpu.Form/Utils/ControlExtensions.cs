using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;

namespace Cpu.Forms.Utils;

public static class ControlExtensions
{
    public static void BindTo(
        this Control control, string controlPropName,
        ObservableObject source, string sourcePropName)
    {
        source.PropertyChanged += (object? sender, PropertyChangedEventArgs eventArgs) =>
        {
            if (sender is not null
                && sourcePropName.Equals(eventArgs.PropertyName))
            {
                control.Invoke(() =>
                {
                    var newValue = sender
                        .GetType()?
                        .GetProperty(sourcePropName)?
                        .GetValue(sender) ?? string.Empty;

                    var targetProp =
                    control
                        .GetType()?
                        .GetProperty(controlPropName);

                    var finalValue = typeof(string).Equals(targetProp?.PropertyType)
                                   ? newValue.ToString()
                                   : newValue;

                    targetProp?.SetValue(control, finalValue);
                });
            }
        };
    }

    public static void BindTo(this CheckBox control, ObservableObject source, string sourcePropName)
    {
        control.BindTo(nameof(CheckBox.Checked), source, sourcePropName);
    }

    public static void BindTo(this TextBox control, ObservableObject source, string sourcePropName)
    {
        control.BindTo(nameof(TextBox.Text), source, sourcePropName);
    }

    public static void BindTo(this Button control, ObservableObject source, string sourcePropName)
    {
        control.BindTo(nameof(Button.Enabled), source, sourcePropName);
    }

    public static void BindTo(this ToolStripMenuItem control, ObservableObject source, string sourcePropName)
    {
        _ = control.DataBindings.Add(
                nameof(ToolStripMenuItem.Enabled),
                source,
                sourcePropName,
                false,
                DataSourceUpdateMode.OnPropertyChanged);
    }
}
