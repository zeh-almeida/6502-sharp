using Cpu.Extensions;
using Cpu.Opcodes;
using Cpu.States;

namespace Cpu.Instructions.Decrements
{
    /// <summary>
    /// <para>Decrement T Register instruction (DEY)</para>
    /// <para>Subtracts one from the Y register setting the zero and negative flags as appropriate.</para>
    /// <para>
    /// Executes the following opcodes:
    /// <c>0x88</c>
    /// </para>
    /// </summary>
    /// <see href="https://masswerk.at/6502/6502_instruction_set.html#DEY"/>
    public sealed class DecrementRegisterY : BaseInstruction
    {
        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="DecrementRegisterY"/>
        /// </summary>
        public DecrementRegisterY()
            : base(new OpcodeInformation(0x88, 2, 1))
        { }
        #endregion

        /// <inheritdoc/>
        public override void Execute(ICpuState currentState, ushort value)
        {
            var operation = currentState.Registers.IndexY;
            operation = (byte)(operation - 1);

            currentState.Flags.IsZero = operation.IsZero();
            currentState.Flags.IsNegative = operation.IsLastBitSet();

            currentState.Registers.IndexY = operation;
        }
    }
}
