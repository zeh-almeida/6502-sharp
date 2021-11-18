using Cpu.Extensions;
using Cpu.Instructions.Exceptions;
using Cpu.Opcodes;
using Cpu.States;

namespace Cpu.Instructions.Arithmetic
{
    /// <summary>
    /// <para>Compare Y Register instruction (CPY)</para>
    /// <para>Compares the contents of the Y register with another memory held value and sets the zero and carry flags as appropriate.</para>
    /// <para>
    /// Executes the following opcodes:
    /// <c>0xC0</c>,
    /// <c>0xC4</c>,
    /// <c>0xCC</c>
    /// </para>
    /// </summary>
    /// <see href="https://masswerk.at/6502/6502_instruction_set.html#CPY"/>
    public sealed class CompareRegisterY : BaseInstruction
    {
        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="CompareRegisterY"/> instruction
        /// </summary>
        public CompareRegisterY()
            : base(
            new OpcodeInformation(0xC0, 2, 2),
                      new OpcodeInformation(0xC4, 3, 2),
                      new OpcodeInformation(0xCC, 4, 3))
        { }
        #endregion

        /// <inheritdoc/>
        public override ICpuState Execute(ICpuState currentState, ushort value)
        {
            var loadValue = Load(currentState, value);
            var accumulator = currentState.Registers.IndexY;

            var operation = (byte)(accumulator - loadValue);

            currentState.Flags.IsZero = (operation.Equals(accumulator));
            currentState.Flags.IsNegative = (operation.IsLastBitSet());
            currentState.Flags.IsCarry = (loadValue <= accumulator);

            return currentState;
        }

        private static ushort Load(ICpuState currentState, ushort address)
        {
            return currentState.ExecutingOpcode switch
            {
                0xC0 => address,
                0xC4 => currentState.Memory.ReadZeroPage(address),
                0xCC => currentState.Memory.ReadAbsolute(address),
                _ => throw new UnknownOpcodeException(currentState.ExecutingOpcode),
            };
        }
    }
}
