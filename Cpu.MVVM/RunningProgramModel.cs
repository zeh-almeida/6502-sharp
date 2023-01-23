using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Cpu.Execution;
using Cpu.Extensions;
using Cpu.States;
using System.Text;

namespace Cpu.MVVM;

/// <summary>
/// Represents the executed instructions in a program
/// </summary>
public partial class RunningProgramModel : ObservableObject
{
    #region Constants
    public const int CharsPer8Bit = 4;
    #endregion

    #region Attributes
    /// <summary>
    /// Name of the program loaded
    /// </summary>
    [ObservableProperty]
    private string _programName = string.Empty;

    /// <summary>
    /// Representation of the current loaded program
    /// </summary>
    [ObservableProperty]
    private string _bytes = string.Empty;

    /// <summary>
    /// Representation of the instructions executed so far
    /// </summary>
    [ObservableProperty]
    private string _execution = string.Empty;

    /// <summary>
    /// Checks if the program is loaded and ready to use
    /// </summary>
    [ObservableProperty]
    private bool _programLoaded = false;
    #endregion

    /// <summary>
    /// Adds a new executed instruction to the running program
    /// </summary>
    /// <param name="instruction">Instruction to add</param>
    [RelayCommand]
    protected void AddInstruction(DecodedInstruction instruction)
    {
        this.Execution += $"{instruction}{Environment.NewLine}";
    }

    /// <summary>
    /// Clears the execution list
    /// </summary>
    [RelayCommand]
    protected void ClearExecution()
    {
        this.Execution = string.Empty;
    }

    /// <summary>
    /// Loads the program bytes
    /// </summary>
    [RelayCommand]
    protected void LoadProgram(ReadOnlyMemory<byte> program)
    {
        var builder = new StringBuilder(1 + (program.Length * CharsPer8Bit));
        var data = program.Span;

        foreach (var value in data[ICpuState.MemoryStateOffset..])
        {
            _ = builder.AppendLine(value.AsHex());
        }

        this.Bytes = builder.ToString();
        this.ProgramLoaded = true;
    }

    /// <summary>
    /// Clears the bytes list
    /// </summary>
    [RelayCommand]
    protected void ClearProgram()
    {
        this.Bytes = string.Empty;
        this.ProgramLoaded = false;
    }

    /// <summary>
    /// Sets the current program name
    /// </summary>
    /// <param name="name">Program name</param>
    [RelayCommand]
    protected void SetProgramName(string name)
    {
        this.ProgramName = name;

        this.ClearProgramCommand.Execute(null);
        this.ClearExecutionCommand.Execute(null);
    }
}
