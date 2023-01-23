namespace Cpu.Forms.Utils;

public static class ControlExtensions
{
    public static void BindTo(
        this Control control, string controlPropName,
        object source, string sourcePropName)
    {
        _ = control.DataBindings.Add(
                controlPropName,
                source,
                sourcePropName,
                false,
                DataSourceUpdateMode.OnPropertyChanged);
    }

    public static void BindTo(this CheckBox control, object source, string sourcePropName)
    {
        control.BindTo(nameof(CheckBox.Checked), source, sourcePropName);
    }

    public static void BindTo(this TextBox control, object source, string sourcePropName)
    {
        control.BindTo(nameof(TextBox.Text), source, sourcePropName);
    }

    public static void BindTo(this Button control, object source, string sourcePropName)
    {
        control.BindTo(nameof(Button.Enabled), source, sourcePropName);
    }

    public static void BindTo(this ToolStripMenuItem control, object source, string sourcePropName)
    {
        _ = control.DataBindings.Add(
                nameof(Button.Enabled),
                source,
                sourcePropName,
                false,
                DataSourceUpdateMode.OnPropertyChanged);
    }
}
