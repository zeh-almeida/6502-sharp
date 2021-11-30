using Cpu.Extensions;
using Cpu.Opcodes;
using Cpu.States;

namespace Cpu.Instructions.Increments
{
    /// <summary>
    /// <para>Increment X Register instruction (INX)</para>
    /// <para>Adds one to the X register setting the zero and negative flags as appropriate.</para>
    /// <para>
    /// Executes the following opcodes:
    /// <c>0xE8</c>
    /// </para>
    /// </summary>
    /// <see href="https://masswerk.at/6502/6502_instruction_set.html#INX"/>
    public sealed class IncrementRegisterX : BaseInstruction
    {
        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="IncrementRegisterX"/>
        /// </summary>
        public IncrementRegisterX()
            : base(new OpcodeInformation(0xE8, 2, 1))
        { }
        #endregion

        /// <inheritdoc/>
        public override void Execute(ICpuState currentState, ushort _)
        {
            var operation = (byte)(currentState.Registers.IndexX + 1);

            currentState.Flags.IsZero = operation.IsZero();
            currentState.Flags.IsNegative = operation.IsLastBitSet();

            currentState.Registers.IndexX = operation;
        }
    }
}
