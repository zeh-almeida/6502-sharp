using Cpu.States;

namespace Cpu.Instructions.Branches;

/// <summary>
/// <para>Branch if Minus instruction (BMI)</para>
/// <para>If the negative flag is set then add the relative displacement to the program counter to cause a branch to a new location.</para>
/// <para>
/// Executes the following opcodes:
/// <c>0x30</c>
/// </para>
/// </summary>
/// <see href="https://masswerk.at/6502/6502_instruction_set.html#BMI"/>
public sealed class BranchNegative : BaseInstruction
{
    #region Constructors
    /// <summary>
    /// Instantiates a new <see cref="BranchNegative"/> instruction
    /// </summary>
    public BranchNegative()
        : base(0x30)
    { }
    #endregion

    /// <inheritdoc/>
    public override void Execute(in ICpuState currentState, in ushort value)
    {
        if (currentState.Flags.IsNegative)
        {
            ExecuteBranch(currentState, value);
        }
    }
}
