using Cpu.Instructions.Exceptions;
using Cpu.Opcodes;
using Cpu.States;

namespace Cpu.Instructions.Store
{
    /// <summary>
    /// <para>Store Register X instruction (STX)</para>
    /// <para>Stores the contents of the X register into memory.</para>
    /// <para>
    /// Executes the following opcodes:
    /// <c>0x86</c>,
    /// <c>0x96</c>,
    /// <c>0x8E</c>
    /// </para>
    /// </summary>
    /// <see href="https://masswerk.at/6502/6502_instruction_set.html#STX"/>
    public sealed class StoreRegisterX : BaseInstruction
    {
        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="StoreRegisterX"/>
        /// </summary>
        public StoreRegisterX()
            : base(
                new OpcodeInformation(0x86, 3, 2),
                new OpcodeInformation(0x96, 4, 2),
                new OpcodeInformation(0x8E, 4, 3))
        { }
        #endregion

        /// <inheritdoc/>
        public override void Execute(ICpuState currentState, ushort value)
        {
            Write(currentState, value);
        }

        private static void Write(ICpuState currentState, ushort address)
        {
            var register = currentState.Registers.IndexX;

            switch (currentState.ExecutingOpcode)
            {
                case 0x86:
                    currentState.Memory.WriteZeroPage(address, register);
                    break;

                case 0x96:
                    currentState.Memory.WriteZeroPageY(address, register);
                    break;

                case 0x8E:
                    currentState.Memory.WriteAbsolute(address, register);
                    break;

                default:
                    throw new UnknownOpcodeException(currentState.ExecutingOpcode);
            }
        }
    }
}
