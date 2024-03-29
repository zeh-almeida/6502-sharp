﻿using Cpu.Extensions;
using Cpu.States;

namespace Cpu.Instructions.Illegal;

/// <summary>
/// <para>AND Memory and Stack Pointer instruction (LAS/LAR)</para>
/// <para>Illegal, AND memory with the Stack Pointer then sets value for Accumulator, X Register and Stack Pointer</para>
/// <para>
/// Executes the following opcodes:
/// <c>0xBB</c>
/// </para>
/// </summary>
/// <see href="https://masswerk.at/6502/6502_instruction_set.html#LAS"/>
/// <seealso cref="Stack.TransferPointerX"/>
/// <seealso cref="Transfers.TransferXAccumulator"/>
/// <seealso cref="Logic.LogicAnd"/>
/// <seealso cref="Transfers.TransferAccumulatorX"/>
/// <seealso cref="Stack.TransferXPointer"/>
public sealed class AndStackPointer : BaseInstruction
{
    #region Constructors
    /// <summary>
    /// Instantiates a new <see cref="AndStackPointer"/>
    /// </summary>
    public AndStackPointer()
        : base(0xBB)
    { }
    #endregion

    /// <inheritdoc/>
    public override void Execute(in ICpuState currentState, in ushort value)
    {
        var (crossed, loadValue) = currentState.Memory.ReadAbsoluteY(value);

        if (crossed)
        {
            currentState.IncrementCycles(1);
        }

        var stackPointer = currentState.Registers.StackPointer;

        var operation = (byte)(loadValue & stackPointer);

        currentState.Flags.IsZero = operation.IsZero();
        currentState.Flags.IsNegative = operation.IsLastBitSet();

        currentState.Registers.IndexX = operation;
        currentState.Registers.Accumulator = operation;
        currentState.Registers.StackPointer = operation;
    }
}
