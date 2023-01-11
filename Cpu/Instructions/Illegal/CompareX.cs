using Cpu.Extensions;
using Cpu.States;

namespace Cpu.Instructions.Illegal
{
    /// <summary>
    /// <para>Compare Accumulator with X Register instruction (SBX, AXS, SAX)</para>
    /// <para>Illegal, compares Accumulator with X Register then decrements memory</para>
    /// <para>
    /// Executes the following opcodes:
    /// <c>0xCB</c>
    /// </para>
    /// </summary>
    /// <see href="https://masswerk.at/6502/6502_instruction_set.html#SBX"/>
    /// <seealso cref="Arithmetic.CompareAccumulator"/>
    /// <seealso cref="Decrements.DecrementRegisterX"/>
    public sealed class CompareX : BaseInstruction
    {
        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="CompareX"/>
        /// </summary>
        public CompareX()
            : base(0xCB)
        { }
        #endregion

        /// <inheritdoc/>
        public override void Execute(ICpuState currentState, ushort value)
        {
            var registerX = currentState.Registers.IndexX;
            var accumulator = currentState.Registers.Accumulator;

            var andValue = (byte)(accumulator & registerX);
            var operation = (byte)(andValue - value);

            currentState.Flags.IsZero = operation.Equals(accumulator);
            currentState.Flags.IsNegative = operation.IsLastBitSet();
            currentState.Flags.IsCarry = operation <= accumulator;

            currentState.Registers.IndexX = operation;
        }
    }
}
