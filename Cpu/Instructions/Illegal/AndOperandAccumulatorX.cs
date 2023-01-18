using Cpu.Extensions;
using Cpu.States;

namespace Cpu.Instructions.Illegal;

/// <summary>
/// <para>Logical AND with Operand, store in X and Accumulator instruction (LXA/LAX immediate)</para>
/// <para>Illegal and highly unstable, executes a logical AND with a constant and operand
/// storing result in Accumulator and X Register</para>
/// <para>In this implementation, we have fixed the constant to be the accumulator</para>
/// <para>
/// Executes the following opcodes:
/// <c>0xAB</c>
/// </para>
/// </summary>
/// <see href="https://masswerk.at/6502/6502_instruction_set.html#LXA"/>
/// <seealso cref="Logic.LogicAnd"/>
public sealed class AndOperandAccumulatorX : BaseInstruction
{
    #region Constructors
    /// <summary>
    /// Instantiates a new <see cref="AndOperandAccumulatorX"/>
    /// </summary>
    public AndOperandAccumulatorX()
        : base(0xAB)
    { }
    #endregion

    /// <inheritdoc/>
    public override void Execute(ICpuState currentState, ushort value)
    {
        var accumulator = currentState.Registers.Accumulator;
        var andValue = (byte)(accumulator & value);

        currentState.Flags.IsZero = andValue.IsZero();
        currentState.Flags.IsNegative = andValue.IsLastBitSet();

        currentState.Registers.Accumulator = andValue;
        currentState.Registers.IndexX = andValue;
    }
}
