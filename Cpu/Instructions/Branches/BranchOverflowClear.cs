using Cpu.States;

namespace Cpu.Instructions.Branches;

/// <summary>
/// <para>Branch if Overflow Clear instruction (BVC)</para>
/// <para>If the overflow flag is clear then add the relative displacement to the program counter to cause a branch to a new location.</para>
/// <para>
/// Executes the following opcodes:
/// <c>0x50</c>
/// </para>
/// </summary>
/// <see href="https://masswerk.at/6502/6502_instruction_set.html#BVC"/>
public sealed class BranchOverflowClear : BaseInstruction
{
    #region Constructors
    /// <summary>
    /// Instantiates a new <see cref="BranchOverflowClear"/> instruction
    /// </summary>
    public BranchOverflowClear()
        : base(0x50)
    { }
    #endregion

    /// <inheritdoc/>
    public override void Execute(ICpuState currentState, ushort value)
    {
        if (!currentState.Flags.IsOverflow)
        {
            ExecuteBranch(currentState, value);
        }
    }
}
