using Cpu.Opcodes;
using Cpu.States;

namespace Cpu.Instructions.StatusChanges
{
    /// <summary>
    /// <para>Clear Carry Flag Instruction (CLC)</para>
    /// <para>Set the carry flag to zero.</para>
    /// <para>
    /// Executes the following opcodes:
    /// <c>0x18</c>
    /// </para>
    /// </summary>
    public sealed class ClearCarryFlag : BaseInstruction
    {
        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="ClearCarryFlag"/>
        /// </summary>
        public ClearCarryFlag()
            : base(new OpcodeInformation(0x18, 2, 1))
        { }
        #endregion

        /// <inheritdoc/>
        public override void Execute(ICpuState currentState, ushort _)
        {
            currentState.Flags.IsCarry = false;
        }
    }
}
