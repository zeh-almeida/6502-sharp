using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;

namespace Cpu.Forms.Utils;

/// <summary>
/// Extends the <see cref="Control"/> definition with binds for MVVM async usage
/// </summary>
public static class ControlExtensions
{
    /// <summary>
    /// Binds a generic <see cref="Control"/>
    /// </summary>
    /// <param name="control">Control object</param>
    /// <param name="controlPropName">Property of the control to bind to</param>
    /// <param name="source">Source object</param>
    /// <param name="sourcePropName">Property of the source to bind to</param>
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

    /// <summary>
    /// Binds the <see cref="CheckBox.Checked"/> property of the <see cref="CheckBox"/>
    /// </summary>
    /// <param name="control">Control object</param>
    /// <param name="source">Source object</param>
    /// <param name="sourcePropName">Property of the source to bind to</param>
    public static void BindTo(this CheckBox control, ObservableObject source, string sourcePropName)
    {
        control.BindTo(nameof(CheckBox.Checked), source, sourcePropName);
    }

    /// <summary>
    /// Binds the <see cref="TextBox.Text"/> property of the <see cref="TextBox"/>
    /// </summary>
    /// <param name="control">Control object</param>
    /// <param name="source">Source object</param>
    /// <param name="sourcePropName">Property of the source to bind to</param>
    public static void BindTo(this TextBox control, ObservableObject source, string sourcePropName)
    {
        control.BindTo(nameof(TextBox.Text), source, sourcePropName);
    }

    /// <summary>
    /// Binds the <see cref="Control.Enabled"/> property of the <see cref="Button"/>
    /// </summary>
    /// <param name="control">Control object</param>
    /// <param name="source">Source object</param>
    /// <param name="sourcePropName">Property of the source to bind to</param>
    public static void BindTo(this Button control, ObservableObject source, string sourcePropName)
    {
        control.BindTo(nameof(Button.Enabled), source, sourcePropName);
    }

    /// <summary>
    /// Binds the <see cref="Control.Enabled"/> property of the <see cref="ToolStripMenuItem"/>
    /// </summary>
    /// <param name="control">Control object</param>
    /// <param name="source">Source object</param>
    /// <param name="sourcePropName">Property of the source to bind to</param>
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
