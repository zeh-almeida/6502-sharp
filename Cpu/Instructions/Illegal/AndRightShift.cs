using Cpu.Extensions;
using Cpu.Opcodes;
using Cpu.States;

namespace Cpu.Instructions.Illegal
{
    /// <summary>
    /// <para>Logical AND and Rotate Right instruction (ARR)</para>
    /// <para>Illegal, executes a logical AND then rotates to the right</para>
    /// <para>
    /// Executes the following opcodes:
    /// <c>0x6B</c>
    /// </para>
    /// </summary>
    /// <see href="https://masswerk.at/6502/6502_instruction_set.html#ALR"/>
    /// <seealso cref="Logic.LogicAnd"/>
    /// <seealso cref="Shifts.ArithmeticShiftRight"/>
    public sealed class AndRightShift : BaseInstruction
    {
        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="AndRightShift"/>
        /// </summary>
        public AndRightShift()
            : base(new OpcodeInformation(0x6B, 2, 2))
        { }
        #endregion

        /// <inheritdoc/>
        public override void Execute(ICpuState currentState, ushort value)
        {
            var accumulator = currentState.Registers.Accumulator;
            var oldCarry = currentState.Flags.IsCarry;

            var andValue = (byte)(value & accumulator);
            var shifted = andValue.RotateRight(oldCarry);

            var is7thBitSet = andValue.IsLastBitSet();
            var is6thBitSet = ((byte)(andValue >> 6)).IsFirstBitSet();

            currentState.Flags.IsCarry = is7thBitSet;
            currentState.Flags.IsOverflow = ((is7thBitSet || is6thBitSet) && !(is7thBitSet && is6thBitSet));

            currentState.Flags.IsNegative = (shifted.IsLastBitSet());
            currentState.Flags.IsZero = (0.Equals(shifted));

            currentState.Registers.Accumulator = shifted;
        }
    }
}
