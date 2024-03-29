﻿using Cpu.States;

namespace Cpu.Instructions.Branches;

/// <summary>
/// <para>Branch if Overflow Set instruction (BVS)</para>
/// <para>If the overflow flag is clear then add the relative displacement to the program counter to cause a branch to a new location.</para>
/// <para>
/// Executes the following opcodes:
/// <c>0x70</c>
/// </para>
/// </summary>
/// <see href="https://masswerk.at/6502/6502_instruction_set.html#BVS"/>
public sealed class BranchOverflowSet : BaseInstruction
{
    #region Constructors
    /// <summary>
    /// Instantiates a new <see cref="BranchOverflowSet"/> instruction
    /// </summary>
    public BranchOverflowSet()
        : base(0x70)
    { }
    #endregion

    /// <inheritdoc/>
    public override void Execute(in ICpuState currentState, in ushort value)
    {
        if (currentState.Flags.IsOverflow)
        {
            ExecuteBranch(currentState, value);
        }
    }
}
