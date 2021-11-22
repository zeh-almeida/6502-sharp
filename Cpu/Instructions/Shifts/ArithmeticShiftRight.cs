using Cpu.Extensions;
using Cpu.Instructions.Exceptions;
using Cpu.Opcodes;
using Cpu.States;

namespace Cpu.Instructions.Shifts
{
    /// <summary>
    /// <para>Logical Shift Right instruction (LSR)</para>
    /// <para>
    /// Each of the bits in A or M is shift one place to the right.
    /// The bit that was in bit 0 is shifted into the carry flag. Bit 7 is set to zero.
    /// </para>
    /// <para>
    /// Executes the following opcodes:
    /// <c>0x4A</c>,
    /// <c>0x46</c>,
    /// <c>0x56</c>,
    /// <c>0x4E</c>,
    /// <c>0x5E</c>
    /// </para>
    /// </summary>
    /// <see href="https://masswerk.at/6502/6502_instruction_set.html#LSR"/>
    public sealed class ArithmeticShiftRight : BaseInstruction
    {
        #region Constructors
        /// <summary>
        /// Instantiates a  new <see cref="ArithmeticShiftRight"/>
        /// </summary>
        public ArithmeticShiftRight()
            : base(
            new OpcodeInformation(0x4A, 2, 1),
                      new OpcodeInformation(0x46, 5, 2),
                      new OpcodeInformation(0x56, 6, 2),
                      new OpcodeInformation(0x4E, 6, 3),
                      new OpcodeInformation(0x5E, 7, 3))
        { }
        #endregion

        /// <inheritdoc/>
        public override void Execute(ICpuState currentState, ushort value)
        {
            var loadValue = Load(currentState, value);
            var shifted = (byte)(loadValue >> 1);

            Write(currentState, value, shifted);

            currentState.Flags.IsCarry = loadValue.IsFirstBitSet();
            currentState.Flags.IsNegative = shifted.IsFirstBitSet();
            currentState.Flags.IsZero = shifted.IsZero();

        }

        private static ushort Load(ICpuState currentState, ushort address)
        {
            return currentState.ExecutingOpcode switch
            {
                0x4A => currentState.Registers.Accumulator,
                0x46 => currentState.Memory.ReadZeroPage(address),
                0x56 => currentState.Memory.ReadZeroPageX(address),
                0x4E => currentState.Memory.ReadAbsolute(address),
                0x5E => currentState.Memory.ReadAbsoluteX(address),
                _ => throw new UnknownOpcodeException(currentState.ExecutingOpcode),
            };
        }

        private static void Write(ICpuState currentState, ushort address, byte value)
        {
            switch (currentState.ExecutingOpcode)
            {
                case 0x4A:
                    currentState.Registers.Accumulator = value;
                    break;

                case 0x46:
                    currentState.Memory.WriteZeroPage(address, value);
                    break;

                case 0x56:
                    currentState.Memory.WriteZeroPageX(address, value);
                    break;

                case 0x4E:
                    currentState.Memory.WriteAbsolute(address, value);
                    break;

                case 0x5E:
                default:
                    currentState.Memory.WriteAbsoluteX(address, value);
                    break;
            }
        }
    }
}
