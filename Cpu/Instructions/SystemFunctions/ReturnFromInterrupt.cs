using Cpu.States;

namespace Cpu.Instructions.SystemFunctions;

/// <summary>
/// <para>Return from Interrupt instruction (RTI)</para>
/// <para>
/// Used at the end of an interrupt processing routine.
/// It pulls the processor flags from the stack followed by the program counter
/// </para>
/// <para>
/// Executes the following opcodes:
/// <c>0x40</c>
/// </para>
/// </summary>
/// <see href="https://masswerk.at/6502/6502_instruction_set.html#NOP"/>
public sealed class ReturnFromInterrupt : BaseInstruction
{
    #region Constructors
    /// <summary>
    /// Instantiates a new <see cref="ReturnFromInterrupt"/>
    /// </summary>
    public ReturnFromInterrupt()
        : base(0x40)
    { }
    #endregion

    /// <inheritdoc/>
    public override void Execute(ICpuState currentState, ushort _)
    {
        LoadProgramCounter(currentState);
        LoadProcessorStatus(currentState);
    }

    private static void LoadProcessorStatus(ICpuState currentState)
    {
        var bits = currentState.Stack.Pull();
        currentState.Flags.Load(bits);
    }

    private static void LoadProgramCounter(ICpuState currentState)
    {
        currentState.Registers.ProgramCounter = currentState.Stack.Pull16();
    }
}
