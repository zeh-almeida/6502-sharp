using Cpu.Extensions;
using Cpu.Instructions.Exceptions;
using Cpu.Opcodes;
using Cpu.States;

namespace Cpu.Instructions.Decrements
{
    /// <summary>
    /// <para>Decrement Memory instruction (DEC)</para>
    /// <para>Subtracts one from the value held at a specified memory location setting the zero and negative flags as appropriate.</para>
    /// <para>
    /// Executes the following opcodes:
    /// <c>0xC6</c>,
    /// <c>0xD6</c>,
    /// <c>0xCE</c>,
    /// <c>0xDE</c>
    /// </para>
    /// </summary>
    /// <see href="https://masswerk.at/6502/6502_instruction_set.html#DEC"/>
    public sealed class DecrementMemory : BaseInstruction
    {
        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="DecrementMemory"/>
        /// </summary>
        public DecrementMemory()
            : base(
            new OpcodeInformation(0xC6, 5, 2),
                      new OpcodeInformation(0xD6, 6, 2),
                      new OpcodeInformation(0xCE, 6, 2),
                      new OpcodeInformation(0xDE, 7, 3))
        { }
        #endregion

        /// <inheritdoc/>
        public override ICpuState Execute(ICpuState currentState, ushort value)
        {
            var operation = (byte)(Read(currentState, value) - 1);

            currentState.Flags.IsZero = operation.IsZero();
            currentState.Flags.IsNegative = operation.IsLastBitSet();

            Write(currentState, value, operation);
            return currentState;
        }

        private static void Write(ICpuState currentState, ushort address, byte value)
        {
            switch (currentState.ExecutingOpcode)
            {
                case 0xC6:
                    currentState.Memory.WriteZeroPage(address, value);
                    break;

                case 0xD6:
                    currentState.Memory.WriteZeroPageX(address, value);
                    break;

                case 0xCE:
                    currentState.Memory.WriteAbsolute(address, value);
                    break;

                case 0xDE:
                default:
                    currentState.Memory.WriteAbsoluteX(address, value);
                    break;
            }
        }

        private static byte Read(ICpuState currentState, ushort address)
        {
            return currentState.ExecutingOpcode switch
            {
                0xC6 => currentState.Memory.ReadZeroPage(address),
                0xD6 => currentState.Memory.ReadZeroPageX(address),
                0xCE => currentState.Memory.ReadAbsolute(address),
                0xDE => currentState.Memory.ReadAbsoluteX(address),
                _ => throw new UnknownOpcodeException(currentState.ExecutingOpcode),
            };
        }
    }
}
