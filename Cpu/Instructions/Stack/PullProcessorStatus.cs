using Cpu.States;

namespace Cpu.Instructions.Stack;

/// <summary>
/// <para>Pull Processor Status instruction (PLP)</para>
/// <para>
/// Pulls an 8 bit value from the stack and into the processor flags.
/// The flags will take on new states as determined by the value pulled.
/// </para>
/// <para>
/// Executes the following opcodes:
/// <c>0x28</c>
/// </para>
/// </summary>
/// <see href="https://masswerk.at/6502/6502_instruction_set.html#PLP"/>
public sealed class PullProcessorStatus : BaseInstruction
{
    #region Constructors
    /// <summary>
    /// Instantiates a new <see cref="PullProcessorStatus"/>
    /// </summary>
    public PullProcessorStatus()
        : base(0x28)
    { }
    #endregion

    /// <inheritdoc/>
    public override void Execute(ICpuState currentState, ushort _)
    {
        var stackValue = currentState.Stack.Pull();
        currentState.Flags.Load(stackValue);
    }
}
