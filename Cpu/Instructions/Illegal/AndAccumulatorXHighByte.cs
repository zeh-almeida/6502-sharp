using Cpu.Instructions.Exceptions;
using Cpu.Opcodes;
using Cpu.States;

namespace Cpu.Instructions.Illegal
{
    /// <summary>
    /// <para>Logical AND with Address High Byte, Register X and Accumulator instruction (SHA/AXA/AHX)</para>
    /// <para>Illegal and unstable, executes a logical AND with the Operand High Byte and Accumulator
    /// followed by another logical AND with the X register</para>
    /// <para>May sometimes drop off the High Byte or not cross the memory boundaries correctly, hence the unstable definition</para>
    /// <para>In this implementation, it doesn't suffer from instabilities</para>
    /// <para>
    /// Executes the following opcodes:
    /// <c>0x93</c>,
    /// <c>0x9F</c>
    /// </para>
    /// </summary>
    /// <see href="https://masswerk.at/6502/6502_instruction_set.html#SHA"/>
    /// <seealso cref="Logic.LogicAnd"/>
    /// <seealso cref="Store.StoreAccumulator"/>
    /// <seealso cref="Store.StoreRegisterX"/>
    public sealed class AndAccumulatorXHighByte : BaseInstruction
    {
        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="AndAccumulatorXHighByte"/>
        /// </summary>
        public AndAccumulatorXHighByte()
            : base(
                  new OpcodeInformation(0x93, 6, 2),
                            new OpcodeInformation(0x9F, 5, 3))
        { }
        #endregion

        /// <inheritdoc/>
        public override ICpuState Execute(ICpuState currentState, ushort value)
        {
            var highByte = ReadHighByte(currentState, value);
            var accumulator = currentState.Registers.Accumulator;
            var registerX = currentState.Registers.IndexX;

            var andValue = (byte)(accumulator & registerX & highByte);

            Write(currentState, value, andValue);
            return currentState;
        }

        private static byte ReadHighByte(ICpuState currentState, ushort value)
        {
            var highByte = currentState.ExecutingOpcode switch
            {
                0x93 => (byte)(value >> 8),
                0x9F => (byte)value,
                _ => throw new UnknownOpcodeException(currentState.ExecutingOpcode),
            };

            return (byte)(highByte + 1);
        }

        private static void Write(ICpuState currentState, ushort address, byte value)
        {
            switch (currentState.ExecutingOpcode)
            {
                case 0x93:
                    currentState.Memory.WriteZeroPageY(address, value);
                    break;

                case 0x9F:
                default:
                    currentState.Memory.WriteAbsoluteY(address, value);
                    break;
            }
        }
    }
}
