using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Cpu.Execution;
using Cpu.Extensions;
using Cpu.MVVM.Messages;
using Cpu.States;
using System.Text;

namespace Cpu.MVVM;

/// <summary>
/// Represents the executed instructions in a program
/// </summary>
public partial class RunningProgramModel
    : ObservableRecipient,
      IRecipient<ProgramLoadedMessage>,
      IRecipient<PropertyChangedMessage<DecodedInstruction>>
{
    #region Constants
    /// <summary>
    /// Amount of characters used to represent 8-bit values in hexadecimal strings
    /// </summary>
    /// <example>1111_1111b == 0xFF</example>
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

    #region Properties
    private object ByteLock { get; } = new();
    #endregion

    #region Constructors
    /// <summary>
    /// Instantiates a new RunningProgramModel
    /// </summary>
    /// <param name="messenger">used to send and receive messages</param>
    public RunningProgramModel(IMessenger messenger)
        : base(messenger)
    {
        this.HandleProgramLoadedMessage();
        this.HandleDecodedInstructionMessage();
    }
    #endregion

    #region Handlers
    private void HandleProgramLoadedMessage()
    {
        this.Messenger.Register<RunningProgramModel, ProgramLoadedMessage>(this, static (r, m) => r.Receive(m));
    }

    private void HandleDecodedInstructionMessage()
    {
        this.Messenger.Register<RunningProgramModel, PropertyChangedMessage<DecodedInstruction>>(this, static (r, m) => r.Receive(m));
    }
    #endregion

    #region Messages
    /// <summary>
    /// receives a message when a program is decoded
    /// </summary>
    /// <param name="message">Loaded program data</param>
    public void Receive(ProgramLoadedMessage message)
    {
        ArgumentNullException.ThrowIfNull(message, nameof(message));
        this.LoadProgramCommand.Execute(message.Value);
    }

    /// <summary>
    /// receives a message when a instruction is decoded
    /// </summary>
    /// <param name="message">Decoded instruction data</param>
    public void Receive(PropertyChangedMessage<DecodedInstruction> message)
    {
        ArgumentNullException.ThrowIfNull(message, nameof(message));

        if (message.NewValue is not null)
        {
            this.AddInstructionCommand.Execute(message.NewValue);
        }
    }
    #endregion

    #region Commands
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
        lock (this.ByteLock)
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
    }

    /// <summary>
    /// Clears the bytes list
    /// </summary>
    [RelayCommand]
    protected void ClearProgram()
    {
        lock (this.ByteLock)
        {
            this.Bytes = string.Empty;
            this.ProgramLoaded = false;
        }
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
    #endregion
}
