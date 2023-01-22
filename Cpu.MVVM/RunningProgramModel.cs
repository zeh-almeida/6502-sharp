using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Cpu.Execution;
using System.Collections.ObjectModel;

namespace Cpu.MVVM;

/// <summary>
/// Represents the executed instructions in a program
/// </summary>
public partial class RunningProgramModel : ObservableObject
{
    #region Properties
    /// <summary>
    /// Instructions previously executed
    /// </summary>
    public ObservableCollection<DecodedInstruction> Instructions { get; }
    #endregion

    #region Constructors
    /// <summary>
    /// Instantiates a new view model
    /// </summary>
    public RunningProgramModel()
    {
        this.Instructions = new ObservableCollection<DecodedInstruction>();
    }
    #endregion

    /// <summary>
    /// Adds a new executed instruction to the running program
    /// </summary>
    /// <param name="instruction">Instruction to add</param>
    [RelayCommand]
    protected void AddInstruction(DecodedInstruction instruction)
    {
        this.Instructions.Add(instruction);
    }

    /// <summary>
    /// Clears the execution list
    /// </summary>
    [RelayCommand]
    protected void ClearExecution()
    {
        this.Instructions.Clear();
    }
}
