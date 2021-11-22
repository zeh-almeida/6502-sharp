using Cpu.Extensions;
using Cpu.Instructions.Exceptions;
using Cpu.Opcodes;
using Cpu.States;

namespace Cpu.Instructions.Illegal
{
    /// <summary>
    /// <para>Rotate Left and Logical AND instruction (RLA)</para>
    /// <para>Illegal, shift left one bit in memory, then AND accumulator with result</para>
    /// <para>
    /// Executes the following opcodes:
    /// <c>0x27</c>,
    /// <c>0x37</c>,
    /// <c>0x2F</c>,
    /// <c>0x3F</c>,
    /// <c>0x3B</c>,
    /// <c>0x23</c>,
    /// <c>0x33</c>
    /// </para>
    /// </summary>
    /// <see href="https://masswerk.at/6502/6502_instruction_set.html#RLA"/>
    /// <seealso cref="Shifts.RotateLeft"/>
    /// <seealso cref="Logic.LogicAnd"/>
    public sealed class RotateLeftLogicalAnd : BaseInstruction
    {
        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="RotateLeftLogicalAnd"/>
        /// </summary>
        public RotateLeftLogicalAnd()
            : base(
                  new OpcodeInformation(0x27, 5, 2),
                            new OpcodeInformation(0x37, 6, 2),
                            new OpcodeInformation(0x2F, 6, 3),
                            new OpcodeInformation(0x3F, 7, 3),
                            new OpcodeInformation(0x3B, 7, 3),
                            new OpcodeInformation(0x23, 8, 2),
                            new OpcodeInformation(0x33, 8, 2))
        { }
        #endregion

        /// <inheritdoc/>
        public override void Execute(ICpuState currentState, ushort value)
        {
            var loadValue = Load(currentState, value);
            var accumulator = currentState.Registers.Accumulator;
            var oldCarry = currentState.Flags.IsCarry;
            var newCarry = loadValue.IsFirstBitSet();

            var shifted = loadValue.RotateLeft(oldCarry);
            var result = (byte)(shifted & accumulator);

            currentState.Flags.IsCarry = newCarry;
            currentState.Flags.IsZero = result.IsZero();
            currentState.Flags.IsNegative = result.IsLastBitSet();

            currentState.Registers.Accumulator = result;

            Write(currentState, value, result);
        }

        private static byte Load(ICpuState currentState, ushort address)
        {
            return currentState.ExecutingOpcode switch
            {
                0x27 => currentState.Memory.ReadZeroPage(address),
                0x37 => currentState.Memory.ReadZeroPageX(address),
                0x2F => currentState.Memory.ReadAbsolute(address),
                0x3F => currentState.Memory.ReadAbsoluteX(address),
                0x3B => currentState.Memory.ReadAbsoluteY(address),
                0x23 => currentState.Memory.ReadIndirectX(address),
                0x33 => currentState.Memory.ReadIndirectY(address),
                _ => throw new UnknownOpcodeException(currentState.ExecutingOpcode),
            };
        }

        private static void Write(ICpuState currentState, ushort address, byte value)
        {
            switch (currentState.ExecutingOpcode)
            {
                case 0x27:
                    currentState.Memory.WriteZeroPage(address, value);
                    break;

                case 0x37:
                    currentState.Memory.WriteZeroPageX(address, value);
                    break;

                case 0x2F:
                    currentState.Memory.WriteAbsolute(address, value);
                    break;

                case 0x3F:
                    currentState.Memory.WriteAbsoluteX(address, value);
                    break;

                case 0x3B:
                    currentState.Memory.WriteAbsoluteY(address, value);
                    break;

                case 0x23:
                    currentState.Memory.WriteIndirectX(address, value);
                    break;

                case 0x33:
                default:
                    currentState.Memory.WriteIndirectY(address, value);
                    break;
            }
        }
    }
}
