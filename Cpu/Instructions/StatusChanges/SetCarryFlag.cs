using Cpu.Opcodes;
using Cpu.States;

namespace Cpu.Instructions.StatusChanges
{
    /// <summary>
    /// <para>Set Carry Flag Instruction (SEC)</para>
    /// <para>Set the carry flag to true.</para>
    /// <para>
    /// Executes the following opcodes:
    /// <c>0x38</c>
    /// </para>
    /// </summary>
    public sealed class SetCarryFlag : BaseInstruction
    {
        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="SetCarryFlag"/>
        /// </summary>
        public SetCarryFlag()
            : base(new OpcodeInformation(0x38, 2, 1))
        { }
        #endregion

        /// <inheritdoc/>
        public override ICpuState Execute(ICpuState currentState, ushort _)
        {
            currentState.Flags.IsCarry = true;
            return currentState;
        }
    }
}
