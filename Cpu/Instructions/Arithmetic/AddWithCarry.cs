using Cpu.Extensions;
using Cpu.Instructions.Exceptions;
using Cpu.States;

namespace Cpu.Instructions.Arithmetic
{
    /// <summary>
    /// <para>Add with Carry instruction (ADC)</para>
    /// <para>
    /// This instruction adds the contents of a memory location to the accumulator together with the carry bit.
    /// If overflow occurs the carry bit is set, this enables multiple byte addition to be performed.
    /// </para>
    /// <para>
    /// Executes the following opcodes:
    /// <c>0x69</c>,
    /// <c>0x65</c>,
    /// <c>0x75</c>,
    /// <c>0x6D</c>,
    /// <c>0x7D</c>,
    /// <c>0x79</c>,
    /// <c>0x61</c>,
    /// <c>0x71</c>
    /// </para>
    /// </summary>
    /// <see href="https://masswerk.at/6502/6502_instruction_set.html#ADC"/>
    /// <see href="https://github.com/amensch/e6502/blob/master/e6502CPU/CPU/e6502.cs"/>
    public sealed class AddWithCarry : BaseInstruction
    {
        #region Constants
        private const byte BinaryOverflowCheck = 0x80;

        private const byte DecimalOverflowCheck = 0x7F;
        #endregion

        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="AddWithCarry"/> instruction
        /// </summary>
        public AddWithCarry()
            : base(
                0x69,
                0x65,
                0x75,
                0x6D,
                0x7D,
                0x79,
                0x61,
                0x71)
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

        private static byte BinaryCalculation(ICpuState currentState, ushort loadValue)
        {
            var accumulator = currentState.Registers.Accumulator;
            var carry = currentState.Flags.IsCarry ? 1 : 0;

            var operation = (ushort)(accumulator + loadValue + carry);

            currentState.Flags.IsCarry = operation.IsBitSet(8);
            currentState.Flags.IsNegative = operation.IsSeventhBitSet();
            currentState.Flags.IsOverflow = !0.Equals((~(accumulator ^ (byte)loadValue)) & (accumulator ^ operation) & BinaryOverflowCheck);

            return (byte)operation;
        }

        private static byte DecimalCalculation(ICpuState currentState, ushort loadValue)
        {
            var carry = currentState.Flags.IsCarry ? 1 : 0;

            var accumulator = currentState.Registers.Accumulator.ToBCD();
            var value = ((byte)loadValue).ToBCD();

            var operation = (byte)(accumulator + value + carry);
            var isCarry = operation > 99;

            if (isCarry)
            {
                operation -= 100;
            }

            var result = operation.ToHex();

            currentState.Flags.IsCarry = isCarry;
            currentState.Flags.IsNegative = result > DecimalOverflowCheck;

            return result;
        }

        private static ushort Load(ICpuState currentState, ushort address)
        {
            return currentState.ExecutingOpcode switch
            {
                0x69 => address,
                0x65 => currentState.Memory.ReadZeroPage(address),
                0x75 => currentState.Memory.ReadZeroPageX(address),
                0x6D => currentState.Memory.ReadAbsolute(address),
                0x7D => LoadExtraCycle(currentState, currentState.Memory.ReadAbsoluteX(address)),
                0x79 => LoadExtraCycle(currentState, currentState.Memory.ReadAbsoluteY(address)),
                0x61 => currentState.Memory.ReadIndirectX(address),
                0x71 => LoadExtraCycle(currentState, currentState.Memory.ReadIndirectY(address)),
                _ => throw new UnknownOpcodeException(currentState.ExecutingOpcode),
            };
        }
    }
}
