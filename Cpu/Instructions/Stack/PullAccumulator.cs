﻿using Cpu.Extensions;
using Cpu.States;

namespace Cpu.Instructions.Stack;

/// <summary>
/// <para>Pull Accumulator instruction (PLA)</para>
/// <para>Pulls an 8 bit value from the stack and into the accumulator. The zero and negative flags are set as appropriate.</para>
/// <para>
/// Executes the following opcodes:
/// <c>0x68</c>
/// </para>
/// </summary>
/// <see href="https://masswerk.at/6502/6502_instruction_set.html#PLA"/>
public sealed class PullAccumulator : BaseInstruction
{
    #region Constructors
    /// <summary>
    /// Instantiates a new <see cref="PullAccumulator"/>
    /// </summary>
    public PullAccumulator()
        : base(0x68)
    { }
    #endregion

    /// <inheritdoc/>
    public override void Execute(in ICpuState currentState, in ushort value)
    {
        var stackValue = currentState.Stack.Pull();

        currentState.Flags.IsZero = stackValue.IsZero();
        currentState.Flags.IsNegative = stackValue.IsLastBitSet();

        currentState.Registers.Accumulator = stackValue;
    }
}
