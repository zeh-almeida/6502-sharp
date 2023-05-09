using Cpu.States;

namespace Cpu.Instructions.Branches;

/// <summary>
/// <para>Branch if Not Equal instruction (BNE)</para>
/// <para>If the zero flag is clear then add the relative displacement to the program counter to cause a branch to a new location.</para>
/// <para>
/// Executes the following opcodes:
/// <c>0xD0</c>
/// </para>
/// </summary>
/// <see href="https://masswerk.at/6502/6502_instruction_set.html#BNE"/>
public sealed class BranchNotEqual : BaseInstruction
{
    #region Constructors
    /// <summary>
    /// Instantiates a new <see cref="BranchNotEqual"/> instruction
    /// </summary>
    public BranchNotEqual()
        : base(0xD0)
    { }
    #endregion

    /// <inheritdoc/>
    public override void Execute(ICpuState currentState, ushort value)
    {
        if (!currentState.Flags.IsZero)
        {
            ExecuteBranch(currentState, value);
        }
    }
}
