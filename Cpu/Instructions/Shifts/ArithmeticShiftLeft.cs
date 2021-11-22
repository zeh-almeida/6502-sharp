using Cpu.Extensions;
using Cpu.Instructions.Exceptions;
using Cpu.Opcodes;
using Cpu.States;

namespace Cpu.Instructions.Shifts
{
    /// <summary>
    /// <para>Arithmetic Shift Left instruction (ASL)</para>
    /// <para>
    /// Shifts all the bits of the accumulator or memory contents one bit left.
    /// Bit 0 is set to 0 and bit 7 is placed in the carry flag.
    /// </para>
    /// <para>
    /// The effect of this operation is to multiply the memory contents by 2 (ignoring 2's complement considerations),
    /// setting the carry if the result will not fit in 8 bits.
    /// </para>
    /// <para>
    /// Executes the following opcodes:
    /// <c>0x0A</c>,
    /// <c>0x06</c>,
    /// <c>0x16</c>,
    /// <c>0x0E</c>,
    /// <c>0x1E</c>
    /// </para>
    /// </summary>
    /// <see href="https://masswerk.at/6502/6502_instruction_set.html#ASL"/>
    public sealed class ArithmeticShiftLeft : BaseInstruction
    {
        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="ArithmeticShiftLeft"/>
        /// </summary>
        public ArithmeticShiftLeft()
            : base(
            new OpcodeInformation(0x0A, 2, 1),
                      new OpcodeInformation(0x06, 5, 2),
                      new OpcodeInformation(0x16, 6, 2),
                      new OpcodeInformation(0x0E, 6, 3),
                      new OpcodeInformation(0x1E, 7, 3))
        { }
        #endregion

        /// <inheritdoc/>
        public override void Execute(ICpuState currentState, ushort value)
        {
            var loadValue = Load(currentState, value);
            var shifted = (byte)(loadValue << 1);

            Write(currentState, value, shifted);

            currentState.Flags.IsCarry = loadValue.IsLastBitSet();
            currentState.Flags.IsNegative = shifted.IsLastBitSet();
            currentState.Flags.IsZero = shifted.IsZero();

        }

        private static byte Load(ICpuState currentState, ushort address)
        {
            return currentState.ExecutingOpcode switch
            {
                0x0A => currentState.Registers.Accumulator,
                0x06 => currentState.Memory.ReadZeroPage(address),
                0x16 => currentState.Memory.ReadZeroPageX(address),
                0x0E => currentState.Memory.ReadAbsolute(address),
                0x1E => currentState.Memory.ReadAbsoluteX(address),
                _ => throw new UnknownOpcodeException(currentState.ExecutingOpcode),
            };
        }

        private static void Write(ICpuState currentState, ushort address, byte value)
        {
            switch (currentState.ExecutingOpcode)
            {
                case 0x0A:
                    currentState.Registers.Accumulator = value;
                    break;

                case 0x06:
                    currentState.Memory.WriteZeroPage(address, value);
                    break;

                case 0x16:
                    currentState.Memory.WriteZeroPageX(address, value);
                    break;

                case 0x0E:
                    currentState.Memory.WriteAbsolute(address, value);
                    break;

                case 0x1E:
                default:
                    currentState.Memory.WriteAbsoluteX(address, value);
                    break;
            }
        }
    }
}
