using Cpu.Extensions;
using Cpu.Instructions.Exceptions;
using Cpu.States;

namespace Cpu.Instructions.Illegal;

/// <summary>
/// <para>Increment Memory and Subtract Accumulator instruction (ISC/ISB/INS)</para>
/// <para>Illegal, increments memory then decrements it from accumulator</para>
/// </summary>
/// <see href="https://masswerk.at/6502/6502_instruction_set.html#ISC"/>
/// <seealso cref="Increments.IncrementMemory"/>
/// <seealso cref="Arithmetic.SubtractWithCarry"/>
public sealed class SubtractMemoryAccumulator : BaseInstruction
{
    #region Constructors
    /// <summary>
    /// Instantiates a new <see cref="SubtractMemoryAccumulator"/>
    /// </summary>
    public SubtractMemoryAccumulator()
        : base(
            0xE7,
            0xF7,
            0xEF,
            0xFF,
            0xFB,
            0xE3,
            0xF3)
    { }
    #endregion

    /// <inheritdoc/>
    public override void Execute(ICpuState currentState, ushort value)
    {
        var accumulator = currentState.Registers.Accumulator;
        var loadValue = Load(currentState, value);
        var carry = currentState.Flags.IsCarry.AsBinary();

        var memoryValue = (byte)(loadValue + 1);
        var twoComplement = (byte)(~memoryValue + carry);
        var operation = (ushort)(accumulator + twoComplement);

        var isNegative = operation.IsSeventhBitSet();
        var isOverflow = operation.IsBitSet(8);

        currentState.Flags.IsCarry = (sbyte)operation >= 0;
        currentState.Flags.IsOverflow = isOverflow;

        currentState.Flags.IsZero = operation.IsZero();
        currentState.Flags.IsNegative = isNegative;

        currentState.Registers.Accumulator = (byte)operation;
    }

    private static byte Load(ICpuState currentState, ushort address)
    {
        return currentState.ExecutingOpcode switch
        {
            0xE7 => currentState.Memory.ReadZeroPage(address),
            0xF7 => currentState.Memory.ReadZeroPageX(address),
            0xEF => currentState.Memory.ReadAbsolute(address),
            0xFF => currentState.Memory.ReadAbsoluteX(address).Item2,
            0xFB => currentState.Memory.ReadAbsoluteY(address).Item2,
            0xE3 => currentState.Memory.ReadIndirectX(address),
            0xF3 => currentState.Memory.ReadIndirectY(address).Item2,
            _ => throw new UnknownOpcodeException(currentState.ExecutingOpcode),
        };
    }
}
