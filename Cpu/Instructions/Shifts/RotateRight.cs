using Cpu.Extensions;
using Cpu.Instructions.Exceptions;
using Cpu.States;

namespace Cpu.Instructions.Shifts
{
    /// <summary>
    /// <para>Rotate Left instruction (ROR)</para>
    /// <para>
    /// Move each of the bits in either A or M one place to the right.
    /// Bit 7 is filled with the current value of the carry flag whilst the old bit 0 becomes the new carry flag value.
    /// </para>
    /// <para>
    /// Executes the following opcodes:
    /// <c>0x6A</c>,
    /// <c>0x66</c>,
    /// <c>0x76</c>,
    /// <c>0x6E</c>,
    /// <c>0x7E</c>
    /// </para>
    /// </summary>
    /// <see href="https://masswerk.at/6502/6502_instruction_set.html#ROR"/>
    public sealed class RotateRight : BaseInstruction
    {
        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="RotateRight"/>
        /// </summary>
        public RotateRight()
            : base(
                0x6A,
                0x66,
                0x76,
                0x6E,
                0x7E)
        { }
        #endregion

        /// <inheritdoc/>
        public override void Execute(ICpuState currentState, ushort value)
        {
            var loadValue = Load(currentState, value);

            var oldCarry = currentState.Flags.IsCarry;
            var newCarry = loadValue.IsFirstBitSet();

            var shifted = loadValue.RotateRight(oldCarry);
            Write(currentState, value, shifted);

            currentState.Flags.IsCarry = newCarry;
            currentState.Flags.IsNegative = oldCarry;
            currentState.Flags.IsZero = shifted.IsZero();
        }

        private static byte Load(ICpuState currentState, ushort address)
        {
            return currentState.ExecutingOpcode switch
            {
                0x6A => currentState.Registers.Accumulator,
                0x66 => currentState.Memory.ReadZeroPage(address),
                0x76 => currentState.Memory.ReadZeroPageX(address),
                0x6E => currentState.Memory.ReadAbsolute(address),
                0x7E => currentState.Memory.ReadAbsoluteX(address).Item2,
                _ => throw new UnknownOpcodeException(currentState.ExecutingOpcode),
            };
        }

        private static void Write(ICpuState currentState, ushort address, byte value)
        {
            switch (currentState.ExecutingOpcode)
            {
                case 0x6A:
                    currentState.Registers.Accumulator = value;
                    break;

                case 0x66:
                    currentState.Memory.WriteZeroPage(address, value);
                    break;

                case 0x76:
                    currentState.Memory.WriteZeroPageX(address, value);
                    break;

                case 0x6E:
                    currentState.Memory.WriteAbsolute(address, value);
                    break;

                case 0x7E:
                default:
                    currentState.Memory.WriteAbsoluteX(address, value);
                    break;
            }
        }
    }
}
