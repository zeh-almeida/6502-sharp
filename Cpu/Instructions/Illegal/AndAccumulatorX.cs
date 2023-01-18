using Cpu.Extensions;
using Cpu.States;

namespace Cpu.Instructions.Illegal;

/// <summary>
/// <para>Logical AND with Constant, X and Operand instruction (ANE/XAA)</para>
/// <para>Illegal and highly unstable, executes a logical AND with a constant and Accumulator
/// followed by another logical AND with the X register
/// and finally another logical AND with the operand</para>
/// <para>In this implementation, we have fixed the constant to be the accumulator</para>
/// <para>
/// Executes the following opcodes:
/// <c>0x8B</c>
/// </para>
/// </summary>
/// <see href="https://masswerk.at/6502/6502_instruction_set.html#ANE"/>
/// <seealso cref="Logic.LogicAnd"/>
public sealed class AndAccumulatorX : BaseInstruction
{
    #region Constructors
    /// <summary>
    /// Instantiates a new <see cref="AndAccumulatorX"/>
    /// </summary>
    public AndAccumulatorX()
        : base(0x8B)
    { }
    #endregion

    /// <inheritdoc/>
    public override void Execute(ICpuState currentState, ushort value)
    {
        var accumulator = currentState.Registers.Accumulator;
        var registerX = currentState.Registers.IndexX;

        var andValue = (byte)(accumulator & registerX & value);

        currentState.Flags.IsZero = andValue.IsZero();
        currentState.Flags.IsNegative = andValue.IsLastBitSet();

        currentState.Registers.Accumulator = andValue;
    }
}
