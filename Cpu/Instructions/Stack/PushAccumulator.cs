using Cpu.Opcodes;
using Cpu.States;

namespace Cpu.Instructions.Stack
{
    /// <summary>
    /// <para>Push Accumulator instruction (PHA)</para>
    /// <para>Pushes a copy of the accumulator on to the stack.</para>
    /// <para>
    /// Executes the following opcodes:
    /// <c>0x48</c>
    /// </para>
    /// </summary>
    /// <see href="https://masswerk.at/6502/6502_instruction_set.html#PHA"/>
    public sealed class PushAccumulator : BaseInstruction
    {
        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="PushAccumulator"/>
        /// </summary>
        public PushAccumulator()
            : base(new OpcodeInformation(0x48, 3, 1))
        { }
        #endregion

        /// <inheritdoc/>
        public override ICpuState Execute(ICpuState currentState, ushort _)
        {
            var accumulator = currentState.Registers.Accumulator;
            currentState.Stack.Push16(accumulator);

            return currentState;
        }
    }
}
