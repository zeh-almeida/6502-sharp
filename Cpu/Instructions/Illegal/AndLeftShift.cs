using Cpu.Extensions;
using Cpu.Opcodes;
using Cpu.States;

namespace Cpu.Instructions.Illegal
{
    /// <summary>
    /// <para>Logical AND and Rotate Left instruction (ALR/ASR)</para>
    /// <para>Illegal, executes a logical AND then rotates to the left</para>
    /// <para>
    /// Executes the following opcodes:
    /// <c>0x4B</c>
    /// </para>
    /// </summary>
    /// <see href="https://masswerk.at/6502/6502_instruction_set.html#ALR"/>
    /// <seealso cref="Logic.LogicAnd"/>
    /// <seealso cref="Shifts.ArithmeticShiftLeft"/>
    public sealed class AndLeftShift : BaseInstruction
    {
        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="AndLeftShift"/>
        /// </summary>
        public AndLeftShift()
            : base(new OpcodeInformation(0x4B, 2, 2))
        { }
        #endregion

        /// <inheritdoc/>
        public override void Execute(ICpuState currentState, ushort value)
        {
            var accumulator = currentState.Registers.Accumulator;

            var andValue = (byte)(value & accumulator);
            var shifted = (byte)(andValue << 1);

            currentState.Flags.IsCarry = (andValue.IsLastBitSet());
            currentState.Flags.IsNegative = (shifted.IsLastBitSet());

            currentState.Flags.IsZero = (0.Equals(shifted));

            currentState.Registers.Accumulator = shifted;
        }
    }
}
