using Cpu.Extensions;
using Cpu.Opcodes;
using Cpu.States;

namespace Cpu.Instructions.Illegal
{
    /// <summary>
    /// <para>Illegal Subtract Carry instruction (USBC)</para>
    /// <para>
    /// Illegal, subtracts the contents of a memory location to the accumulator together with the not of the carry bit.
    /// If overflow occurs the carry bit is clear, this enables multiple byte subtraction to be performed.
    /// </para>
    /// <para>Essentially the same as SBC <c>0xE9</c> opcode.</para>
    /// <para>
    /// Executes the following opcodes:
    /// <c>0xEB</c>
    /// </para>
    /// </summary>
    /// <see href="https://masswerk.at/6502/6502_instruction_set.html#USBC"/>
    /// <seealso cref="Arithmetic.SubtractWithCarry"/>
    public sealed class IllegalSubtractWithCarry : BaseInstruction
    {
        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="IllegalSubtractWithCarry"/>
        /// </summary>
        public IllegalSubtractWithCarry()
            : base(new OpcodeInformation(0xEB, 2, 2))
        { }
        #endregion

        /// <inheritdoc/>
        public override ICpuState Execute(ICpuState currentState, ushort value)
        {
            var accumulator = currentState.Registers.Accumulator;
            var carry = currentState.Flags.IsCarry ? 1 : 0;

            var twoComplement = (byte)(~value + carry);
            var operation = (ushort)(accumulator + twoComplement);

            var isNegative = operation.IsSeventhBitSet();
            var isOverflow = operation.IsBitSet(8);

            currentState.Flags.IsCarry = (sbyte)operation >= 0;
            currentState.Flags.IsOverflow = isOverflow;

            currentState.Flags.IsZero = operation.IsZero();
            currentState.Flags.IsNegative = isNegative;

            currentState.Registers.Accumulator = ((byte)operation);
            return currentState;
        }
    }
}
