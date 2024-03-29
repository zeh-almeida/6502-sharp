﻿using Cpu.Extensions;
using Cpu.States;

namespace Cpu.Instructions.Increments;

/// <summary>
/// <para>Increment Y Register instruction (INY)</para>
/// <para>Adds one to the Y register setting the zero and negative flags as appropriate.</para>
/// <para>
/// Executes the following opcodes:
/// <c>0xC8</c>
/// </para>
/// </summary>
/// <see href="https://masswerk.at/6502/6502_instruction_set.html#INY"/>
public sealed class IncrementRegisterY : BaseInstruction
{
    #region Constructors
    /// <summary>
    /// Instantiates a new <see cref="IncrementRegisterY"/>
    /// </summary>
    public IncrementRegisterY()
        : base(0xC8)
    { }
    #endregion

    /// <inheritdoc/>
    public override void Execute(in ICpuState currentState, in ushort value)
    {
        var operation = currentState.Registers.IndexY;
        operation = (byte)(operation + 1);

        currentState.Flags.IsZero = operation.IsZero();
        currentState.Flags.IsNegative = operation.IsLastBitSet();

        currentState.Registers.IndexY = operation;
    }
}
