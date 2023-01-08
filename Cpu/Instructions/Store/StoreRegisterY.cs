using Cpu.Instructions.Exceptions;
using Cpu.Opcodes;
using Cpu.States;

namespace Cpu.Instructions.Store
{
    /// <summary>
    /// <para>Store Register Y instruction (STY)</para>
    /// <para>Stores the contents of the Y register into memory.</para>
    /// <para>
    /// Executes the following opcodes:
    /// <c>0x84</c>,
    /// <c>0x94</c>,
    /// <c>0x8C</c>
    /// </para>
    /// </summary>
    /// <see href="https://masswerk.at/6502/6502_instruction_set.html#STX"/>
    public sealed class StoreRegisterY : BaseInstruction
    {
        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="StoreRegisterY"/>
        /// </summary>
        public StoreRegisterY()
            : base(
                new OpcodeInformation(0x84, 3, 2),
                new OpcodeInformation(0x94, 4, 2),
                new OpcodeInformation(0x8C, 4, 3))
        { }
        #endregion

        /// <inheritdoc/>
        public override void Execute(ICpuState currentState, ushort value)
        {
            Write(currentState, value);
        }

        private static void Write(ICpuState currentState, ushort address)
        {
            var register = currentState.Registers.IndexY;

            switch (currentState.ExecutingOpcode)
            {
                case 0x84:
                    currentState.Memory.WriteZeroPage(address, register);
                    break;

                case 0x94:
                    currentState.Memory.WriteZeroPageX(address, register);
                    break;

                case 0x8C:
                    currentState.Memory.WriteAbsolute(address, register);
                    break;

                default:
                    throw new UnknownOpcodeException(currentState.ExecutingOpcode);
            }
        }
    }
}
