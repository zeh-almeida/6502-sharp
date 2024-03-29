﻿using Cpu.Extensions;
using Cpu.States;

namespace Cpu.Instructions.Decrements;

/// <summary>
/// <para>Decrement X Register instruction (DEX)</para>
/// <para>Subtracts one from the X register setting the zero and negative flags as appropriate.</para>
/// <para>
/// Executes the following opcodes:
/// <c>0xCA</c>
/// </para>
/// </summary>
/// <see href="https://masswerk.at/6502/6502_instruction_set.html#DEX"/>
public sealed class DecrementRegisterX : BaseInstruction
{
    #region Constructors
    /// <summary>
    /// Instantiates a new <see cref="DecrementRegisterX"/>
    /// </summary>
    public DecrementRegisterX()
        : base(0xCA)
    { }
    #endregion

    /// <inheritdoc/>
    public override void Execute(in ICpuState currentState, in ushort value)
    {
        var operation = currentState.Registers.IndexX;
        operation = (byte)(operation - 1);

        currentState.Flags.IsZero = operation.IsZero();
        currentState.Flags.IsNegative = operation.IsLastBitSet();

        currentState.Registers.IndexX = operation;
    }
}
