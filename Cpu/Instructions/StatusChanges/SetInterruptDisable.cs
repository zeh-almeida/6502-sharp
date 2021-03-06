using Cpu.Opcodes;
using Cpu.States;

namespace Cpu.Instructions.StatusChanges
{
    /// <summary>
    /// <para>Set Interrupt Disable (SEI)</para>
    /// <para>Set the interrupt disable flag to one.</para>
    /// <para>
    /// Executes the following opcodes:
    /// <c>0x78</c>
    /// </para>
    /// </summary>
    public sealed class SetInterruptDisable : BaseInstruction
    {
        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="SetInterruptDisable"/>
        /// </summary>
        public SetInterruptDisable()
            : base(new OpcodeInformation(0x78, 2, 1))
        { }
        #endregion

        /// <inheritdoc/>
        public override void Execute(ICpuState currentState, ushort _)
        {
            currentState.Flags.IsInterruptDisable = true;
        }
    }
}
