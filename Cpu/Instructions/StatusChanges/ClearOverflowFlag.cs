using Cpu.Opcodes;
using Cpu.States;

namespace Cpu.Instructions.StatusChanges
{
    /// <summary>
    /// <para>Clear Overflow Flag Instruction (CLV)</para>
    /// <para>Clears the overflow flag.</para>
    /// <para>
    /// Executes the following opcodes:
    /// <c>0xB8</c>
    /// </para>
    /// </summary>
    public sealed class ClearOverflowFlag : BaseInstruction
    {
        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="ClearOverflowFlag"/>
        /// </summary>
        public ClearOverflowFlag()
            : base(new OpcodeInformation(0xB8, 2, 1))
        { }
        #endregion

        /// <inheritdoc/>
        public override ICpuState Execute(ICpuState currentState, ushort _)
        {
            currentState.Flags.IsOverflow = false;
            return currentState;
        }
    }
}
