using Cpu.States;

namespace Cpu.Instructions.Jumps;

/// <summary>
/// <para>Jump To Subroutine instruction (JSR)</para>
/// <para>Used at the end of a subroutine to return to the calling routine. It pulls the program counter (minus one) from the stack.</para>
/// <para>
/// Executes the following opcodes:
/// <c>0x20</c>
/// </para>
/// </summary>
/// <see href="https://masswerk.at/6502/6502_instruction_set.html#JSR"/>
public sealed class JumpToSubroutine : BaseInstruction
{
    #region Constructors
    /// <summary>
    /// Instantiates a new <see cref="JumpToSubroutine"/>
    /// </summary>
    public JumpToSubroutine()
        : base(0x20)
    { }
    #endregion

    /// <inheritdoc/>
    public override void Execute(in ICpuState currentState, in ushort value)
    {
        currentState.Stack.Push16(currentState.Registers.ProgramCounter);
        currentState.Registers.ProgramCounter = value;
    }
}
