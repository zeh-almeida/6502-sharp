using Cpu.Extensions;
using Cpu.Instructions.Exceptions;
using Cpu.States;

namespace Cpu.Instructions.Arithmetic
{
    /// <summary>
    /// <para>Compare Accumulator instruction (CMP)</para>
    /// <para>
    /// Compares the contents of the accumulator with another memory held value and sets the zero and carry flags as appropriate.
    /// </para>
    /// <para>
    /// Executes the following opcodes:
    /// <c>0xC9</c>,
    /// <c>0xC5</c>,
    /// <c>0xD5</c>,
    /// <c>0xCD</c>,
    /// <c>0xDD</c>,
    /// <c>0xD9</c>,
    /// <c>0xC1</c>,
    /// <c>0xD1</c>
    /// </para>
    /// </summary>
    /// <see href="https://masswerk.at/6502/6502_instruction_set.html#CMP"/>
    public sealed class CompareAccumulator : BaseInstruction
    {
        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="CompareAccumulator"/> instruction
        /// </summary>
        public CompareAccumulator()
            : base(
                0xC9,
                0xC5,
                0xD5,
                0xCD,
                0xDD,
                0xD9,
                0xC1,
                0xD1)
        { }
        #endregion

        /// <inheritdoc/>
        public override void Execute(ICpuState currentState, ushort value)
        {
            var loadValue = Load(currentState, value);
            var accumulator = currentState.Registers.Accumulator;

            var operation = (byte)(accumulator - loadValue);

            currentState.Flags.IsZero = operation.Equals(accumulator);
            currentState.Flags.IsNegative = operation.IsLastBitSet();
            currentState.Flags.IsCarry = loadValue <= accumulator;
        }

        private static ushort Load(ICpuState currentState, ushort address)
        {
            return currentState.ExecutingOpcode switch
            {
                0xC9 => address,
                0xC5 => currentState.Memory.ReadZeroPage(address),
                0xD5 => currentState.Memory.ReadZeroPageX(address),
                0xCD => currentState.Memory.ReadAbsolute(address),
                0xDD => currentState.Memory.ReadAbsoluteX(address),
                0xD9 => currentState.Memory.ReadAbsoluteY(address),
                0xC1 => currentState.Memory.ReadIndirectX(address),
                0xD1 => currentState.Memory.ReadIndirectY(address),
                _ => throw new UnknownOpcodeException(currentState.ExecutingOpcode),
            };
        }
    }
}
