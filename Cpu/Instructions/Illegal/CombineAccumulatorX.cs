using Cpu.Instructions.Exceptions;
using Cpu.States;

namespace Cpu.Instructions.Illegal
{
    /// <summary>
    /// <para>AND Accumulator and Register X instruction (SAX, AXS, AAX)</para>
    /// <para>Illegal, performs a logical AND with the Accumulator and Register X values then writes to memory</para>
    /// <para>
    /// Executes the following opcodes:
    /// <c>0x87</c>,
    /// <c>0x97</c>,
    /// <c>0x8F</c>,
    /// <c>0x83</c>
    /// </para>
    /// </summary>
    /// <see href="https://masswerk.at/6502/6502_instruction_set.html#SAX"/>
    /// <seealso cref="Store.StoreAccumulator"/>
    /// <seealso cref="Store.StoreRegisterX"/>
    public sealed class CombineAccumulatorX : BaseInstruction
    {
        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="CombineAccumulatorX"/>
        /// </summary>
        public CombineAccumulatorX()
            : base(
                0x87,
                0x97,
                0x8F,
                0x83)
        { }
        #endregion

        /// <inheritdoc/>
        public override void Execute(ICpuState currentState, ushort value)
        {
            var accumulator = currentState.Registers.Accumulator;
            var registerX = currentState.Registers.IndexX;

            var operation = (byte)(accumulator & registerX);
            Write(currentState, value, operation);
        }

        private static void Write(ICpuState currentState, ushort address, byte value)
        {
            switch (currentState.ExecutingOpcode)
            {
                case 0x87:
                    currentState.Memory.WriteZeroPage(address, value);
                    break;

                case 0x97:
                    currentState.Memory.WriteZeroPageX(address, value);
                    break;

                case 0x8F:
                    currentState.Memory.WriteAbsolute(address, value);
                    break;

                case 0x83:
                    currentState.Memory.WriteIndirectX(address, value);
                    break;

                default:
                    throw new UnknownOpcodeException(currentState.ExecutingOpcode);
            }
        }
    }
}
