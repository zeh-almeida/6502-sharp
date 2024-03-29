﻿using Cpu.Extensions;
using Cpu.States;

namespace Cpu.Instructions.Transfers;

/// <summary>
/// <para>Transfer X to Accumulator instruction (TYA)</para>
/// <para>Copies the current contents of the Y register into the accumulator and sets the zero and negative flags as appropriate.</para>
/// <para>
/// Executes the following opcodes:
/// <c>0x98</c>
/// </para>
/// </summary>
/// <see href="https://masswerk.at/6502/6502_instruction_set.html#TYA"/>
public sealed class TransferYAccumulator : BaseInstruction
{
    #region Constructors
    /// <summary>
    /// Instantiates a new <see cref="TransferYAccumulator"/> instruction
    /// </summary>
    public TransferYAccumulator()
        : base(0x98)
    { }
    #endregion

    /// <inheritdoc/>
    public override void Execute(in ICpuState currentState, in ushort value)
    {
        var loadValue = currentState.Registers.IndexY;

        currentState.Flags.IsZero = loadValue.IsZero();
        currentState.Flags.IsNegative = loadValue.IsLastBitSet();

        currentState.Registers.Accumulator = loadValue;
    }
}
