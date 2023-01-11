using Cpu.Extensions;
using Cpu.Instructions.Exceptions;
using Cpu.States;

namespace Cpu.Instructions.Arithmetic
{
    /// <summary>
    /// <para>Subtract Carry instruction (SBC)</para>
    /// <para>
    /// This instruction subtracts the contents of a memory location to the accumulator together with the not of the carry bit.
    /// If overflow occurs the carry bit is clear, this enables multiple byte subtraction to be performed.
    /// </para>
    /// <para>
    /// Executes the following opcodes:
    /// <c>0xE9</c>,
    /// <c>0xE5</c>,
    /// <c>0xF5</c>,
    /// <c>0xED</c>,
    /// <c>0xFD</c>,
    /// <c>0xF9</c>,
    /// <c>0xE1</c>,
    /// <c>0xF1</c>
    /// </para>
    /// </summary>
    /// <see href="https://masswerk.at/6502/6502_instruction_set.html#SBC"/>
    /// <see href="https://github.com/amensch/e6502/blob/master/e6502CPU/CPU/e6502.cs"/>
    public sealed class SubtractWithCarry : BaseInstruction
    {
        #region Constants
        private const byte DecimalOverflowCheck = 0x7F;
        #endregion

        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="SubtractWithCarry"/> instruction
        /// </summary>
        public SubtractWithCarry()
            : base(
                0xE9,
                0xE5,
                0xF5,
                0xED,
                0xFD,
                0xF9,
                0xE1,
                0xF1)
        { }
        #endregion

        /// <inheritdoc/>
        public override void Execute(ICpuState currentState, ushort value)
        {
            var loadValue = Load(currentState, value);

            var operation = currentState.Flags.IsDecimalMode
                              ? DecimalCalculation(currentState, loadValue)
                              : BinaryCalculation(currentState, loadValue);

            currentState.Flags.IsZero = operation.IsZero();
            currentState.Registers.Accumulator = operation;
        }

        private static byte DecimalCalculation(ICpuState currentState, ushort loadValue)
        {
            var carry = currentState.Flags.IsCarry ? 0 : 1;

            var accumulator = currentState.Registers.Accumulator.ToBCD();
            var value = ((byte)loadValue).ToBCD();

            var operation = (ushort)(accumulator - value - carry);
            var isCarry = (short)operation >= 0;

            if (!isCarry)
            {
                // BCD numbers wrap around when subtraction is negative
                operation += 100;
            }

            var result = ((byte)operation).ToHex();

            currentState.Flags.IsCarry = isCarry;
            currentState.Flags.IsNegative = result > DecimalOverflowCheck;

            return result;
        }

        private static byte BinaryCalculation(ICpuState currentState, ushort loadValue)
        {
            var carry = currentState.Flags.IsCarry ? 1 : 0;

            var twoComplement = (byte)(~loadValue + carry);
            var operation = (ushort)(currentState.Registers.Accumulator + twoComplement);

            currentState.Flags.IsCarry = (sbyte)operation >= 0;
            currentState.Flags.IsOverflow = operation.IsBitSet(8);
            currentState.Flags.IsNegative = operation.IsSeventhBitSet();

            return (byte)operation;
        }

        private static ushort Load(ICpuState currentState, ushort address)
        {
            return currentState.ExecutingOpcode switch
            {
                0xE9 => address,
                0xE5 => currentState.Memory.ReadZeroPage(address),
                0xF5 => currentState.Memory.ReadZeroPageX(address),
                0xED => currentState.Memory.ReadAbsolute(address),
                0xFD => currentState.Memory.ReadAbsoluteX(address),
                0xF9 => currentState.Memory.ReadAbsoluteY(address),
                0xE1 => currentState.Memory.ReadIndirectX(address),
                0xF1 => currentState.Memory.ReadIndirectY(address),
                _ => throw new UnknownOpcodeException(currentState.ExecutingOpcode),
            };
        }
    }
}
