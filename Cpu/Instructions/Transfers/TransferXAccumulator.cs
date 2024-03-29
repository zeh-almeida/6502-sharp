﻿using Cpu.Extensions;
using Cpu.States;

namespace Cpu.Instructions.Transfers;

/// <summary>
/// <para>Transfer X to Accumulator instruction (TXA)</para>
/// <para>Copies the current contents of the X register into the accumulator and sets the zero and negative flags as appropriate.</para>
/// <para>
/// Executes the following opcodes:
/// <c>0x8A</c>
/// </para>
/// </summary>
/// <see href="https://masswerk.at/6502/6502_instruction_set.html#TXA"/>
public sealed class TransferXAccumulator : BaseInstruction
{
    #region Constructors
    /// <summary>
    /// Instantiates a new <see cref="TransferXAccumulator"/>
    /// </summary>
    public TransferXAccumulator()
        : base(0x8A)
    { }
    #endregion

    /// <inheritdoc/>
    public override void Execute(in ICpuState currentState, in ushort value)
    {
        var loadValue = currentState.Registers.IndexX;

        currentState.Flags.IsZero = loadValue.IsZero();
        currentState.Flags.IsNegative = loadValue.IsLastBitSet();

        currentState.Registers.Accumulator = loadValue;
    }
}
