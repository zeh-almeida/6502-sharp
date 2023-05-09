using Cpu.States;

namespace Cpu.Instructions.Branches;

/// <summary>
/// <para>Branch if Positive instruction (BPL)</para>
/// <para>If the negative flag is clear then add the relative displacement to the program counter to cause a branch to a new location.</para>
/// <para>
/// Executes the following opcodes:
/// <c>0x10</c>
/// </para>
/// </summary>
/// <see href="https://masswerk.at/6502/6502_instruction_set.html#BPL"/>
public sealed class BranchPositive : BaseInstruction
{
    #region Constructors
    /// <summary>
    /// Instantiates a new <see cref="BranchPositive"/> instruction
    /// </summary>
    public BranchPositive()
        : base(0x10)
    { }
    #endregion

    /// <inheritdoc/>
    public override void Execute(ICpuState currentState, ushort value)
    {
        if (!currentState.Flags.IsNegative)
        {
            ExecuteBranch(currentState, value);
        }
    }
}
