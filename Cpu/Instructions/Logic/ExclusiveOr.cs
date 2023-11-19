using Cpu.Extensions;
using Cpu.Instructions.Exceptions;
using Cpu.States;

namespace Cpu.Instructions.Logic;

/// <summary>
/// <para>Exclusive OR instruction (EOR)</para>
/// <para>An exclusive OR is performed, bit by bit, on the accumulator contents using the contents of a byte of memory.</para>
/// <para>
/// Executes the following opcodes:
/// <c>0x49</c>,
/// <c>0x45</c>,
/// <c>0x55</c>,
/// <c>0x4D</c>,
/// <c>0x5D</c>,
/// <c>0x59</c>,
/// <c>0x41</c>,
/// <c>0x51</c>
/// </para>
/// </summary>
/// <see href="https://masswerk.at/6502/6502_instruction_set.html#EOR"/>
public sealed class ExclusiveOr : BaseInstruction
{
    #region Constructors
    /// <summary>
    /// Instantiates a new <see cref="ExclusiveOr"/>
    /// </summary>
    public ExclusiveOr()
        : base(
            0x49,
            0x45,
            0x55,
            0x4D,
            0x5D,
            0x59,
            0x41,
            0x51)
    { }
    #endregion

    /// <inheritdoc/>
    public override void Execute(in ICpuState currentState, in ushort value)
    {
        var toCompare = ValueToCompare(currentState, value);
        var result = (byte)(toCompare ^ currentState.Registers.Accumulator);

        currentState.Flags.IsZero = result.IsZero();
        currentState.Flags.IsNegative = result.IsLastBitSet();

        currentState.Registers.Accumulator = result;
    }

    private static byte ValueToCompare(ICpuState currentState, ushort address)
    {
        return currentState.ExecutingOpcode switch
        {
            0x49 => (byte)address,
            0x45 => currentState.Memory.ReadZeroPage(address),
            0x55 => currentState.Memory.ReadZeroPageX(address),
            0x4D => currentState.Memory.ReadAbsolute(address),
            0x5D => LoadExtraCycle(currentState, currentState.Memory.ReadAbsoluteX(address)),
            0x59 => LoadExtraCycle(currentState, currentState.Memory.ReadAbsoluteY(address)),
            0x41 => currentState.Memory.ReadIndirectX(address),
            0x51 => LoadExtraCycle(currentState, currentState.Memory.ReadIndirectY(address)),
            _ => throw new UnknownOpcodeException(currentState.ExecutingOpcode),
        };
    }
}
