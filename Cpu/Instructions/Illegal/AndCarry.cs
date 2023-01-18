using Cpu.Extensions;
using Cpu.States;

namespace Cpu.Instructions.Illegal;

/// <summary>
/// <para>Logical AND and Carry instruction (ANC/ANC2)</para>
/// <para>Illegal, executes a logical AND then moves bit 7 of accumulator into the Carry flag</para>
/// <para>
/// Executes the following opcodes:
/// <c>0x0B</c>,
/// <c>0x2B</c>
/// </para>
/// </summary>
/// <see href="https://masswerk.at/6502/6502_instruction_set.html#ANC"/>
/// <seealso cref="Logic.LogicAnd"/>
/// <seealso cref="Shifts.ArithmeticShiftLeft"/>
public sealed class AndCarry : BaseInstruction
{
    #region Constructors
    /// <summary>
    /// Instantiates a new <see cref="AndCarry"/>
    /// </summary>
    public AndCarry()
        : base(
            0x0B,
            0x2B)
    { }
    #endregion

    /// <inheritdoc/>
    public override void Execute(ICpuState currentState, ushort value)
    {
        var accumulator = currentState.Registers.Accumulator;
        var andValue = (byte)(value & accumulator);

        var is7thBitSet = andValue.IsLastBitSet();
        currentState.Flags.IsZero = andValue.IsZero();

        currentState.Flags.IsCarry = is7thBitSet;
        currentState.Flags.IsNegative = is7thBitSet;

        currentState.Registers.Accumulator = andValue;
    }
}
