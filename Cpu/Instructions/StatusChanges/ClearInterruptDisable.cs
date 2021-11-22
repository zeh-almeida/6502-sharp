using Cpu.Opcodes;
using Cpu.States;

namespace Cpu.Instructions.StatusChanges
{
    /// <summary>
    /// <para>Clear Interrupt Disable Instruction (CLI)</para>
    /// <para>Clears the interrupt disable flag allowing normal interrupt requests to be serviced.</para>
    /// <para>
    /// Executes the following opcodes:
    /// <c>0x58</c>
    /// </para>
    /// </summary>
    public sealed class ClearInterruptDisable : BaseInstruction
    {
        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="ClearInterruptDisable"/>
        /// </summary>
        public ClearInterruptDisable()
            : base(new OpcodeInformation(0x58, 2, 1))
        { }
        #endregion

        /// <inheritdoc/>
        public override void Execute(ICpuState currentState, ushort _)
        {
            currentState.Flags.IsInterruptDisable = false;
        }
    }
}
