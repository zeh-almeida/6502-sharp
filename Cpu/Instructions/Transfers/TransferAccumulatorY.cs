using Cpu.Extensions;
using Cpu.Opcodes;
using Cpu.States;

namespace Cpu.Instructions.Transfers
{
    /// <summary>
    /// <para>Transfer Accumulator to Y instruction (TAY)</para>
    /// <para>Copies the current contents of the accumulator into the Y register and sets the zero and negative flags as appropriate.</para>
    /// <para>
    /// Executes the following opcodes:
    /// <c>0xA8</c>
    /// </para>
    /// </summary>
    /// <see href="https://masswerk.at/6502/6502_instruction_set.html#TAY"/>
    public sealed class TransferAccumulatorY : BaseInstruction
    {
        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="TransferAccumulatorY"/>
        /// </summary>
        public TransferAccumulatorY()
            : base(new OpcodeInformation(0xA8, 2, 1))
        { }
        #endregion

        /// <inheritdoc/>
        public override ICpuState Execute(ICpuState currentState, ushort _)
        {
            var loadValue = currentState.Registers.Accumulator;

            currentState.Flags.IsZero = loadValue.IsZero();
            currentState.Flags.IsNegative = loadValue.IsLastBitSet();

            currentState.Registers.IndexY = loadValue;
            return currentState;
        }
    }
}
