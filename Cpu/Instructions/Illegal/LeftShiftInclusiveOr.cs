using Cpu.Extensions;
using Cpu.Instructions.Exceptions;
using Cpu.Opcodes;
using Cpu.States;

namespace Cpu.Instructions.Illegal
{
    /// <summary>
    /// <para>Rotate Left and OR instruction (SLO/ASO)</para>
    /// <para>Illegal, shift left one bit in memory, then OR accumulator with memory</para>
    /// <para>
    /// Executes the following opcodes:
    /// <c>0x07</c>,
    /// <c>0x17</c>,
    /// <c>0x0F</c>,
    /// <c>0x1F</c>,
    /// <c>0x1B</c>,
    /// <c>0x03</c>,
    /// <c>0x13</c>
    /// </para>
    /// </summary>
    /// <see href="https://masswerk.at/6502/6502_instruction_set.html#SLO"/>
    /// <seealso cref="Shifts.ArithmeticShiftLeft"/>
    /// <seealso cref="Logic.InclusiveOr"/>
    public sealed class LeftShiftInclusiveOr : BaseInstruction
    {
        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="LeftShiftInclusiveOr"/>
        /// </summary>
        public LeftShiftInclusiveOr()
            : base(
                  new OpcodeInformation(0x07, 5, 2),
                            new OpcodeInformation(0x17, 6, 2),
                            new OpcodeInformation(0x0F, 6, 3),
                            new OpcodeInformation(0x1F, 7, 3),
                            new OpcodeInformation(0x1B, 7, 3),
                            new OpcodeInformation(0x03, 8, 2),
                            new OpcodeInformation(0x13, 8, 2))
        { }
        #endregion

        /// <inheritdoc/>
        public override void Execute(ICpuState currentState, ushort value)
        {
            var accumulator = currentState.Registers.Accumulator;
            var loadValue = Load(currentState, value);

            var shifted = (byte)(loadValue << 1);
            var result = (byte)(shifted | accumulator);

            currentState.Flags.IsZero = (result.IsZero());
            currentState.Flags.IsNegative = (result.IsLastBitSet());
            currentState.Flags.IsCarry = (loadValue.IsLastBitSet());

            Write(currentState, value, result);
        }

        private static byte Load(ICpuState currentState, ushort address)
        {
            return currentState.ExecutingOpcode switch
            {
                0x07 => currentState.Memory.ReadZeroPage(address),
                0x17 => currentState.Memory.ReadZeroPageX(address),
                0x0F => currentState.Memory.ReadAbsolute(address),
                0x1F => currentState.Memory.ReadAbsoluteX(address),
                0x1B => currentState.Memory.ReadAbsoluteY(address),
                0x03 => currentState.Memory.ReadIndirectX(address),
                0x13 => currentState.Memory.ReadIndirectY(address),
                _ => throw new UnknownOpcodeException(currentState.ExecutingOpcode),
            };
        }

        private static void Write(ICpuState currentState, ushort address, byte value)
        {
            switch (currentState.ExecutingOpcode)
            {
                case 0x07:
                    currentState.Memory.WriteZeroPage(address, value);
                    break;

                case 0x17:
                    currentState.Memory.WriteZeroPageX(address, value);
                    break;

                case 0x0F:
                    currentState.Memory.WriteAbsolute(address, value);
                    break;

                case 0x1F:
                    currentState.Memory.WriteAbsoluteX(address, value);
                    break;

                case 0x1B:
                    currentState.Memory.WriteAbsoluteY(address, value);
                    break;

                case 0x03:
                    currentState.Memory.WriteIndirectX(address, value);
                    break;

                case 0x13:
                default:
                    currentState.Memory.WriteIndirectY(address, value);
                    break;
            }
        }
    }
}
