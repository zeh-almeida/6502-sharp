using Cpu.Opcodes;
using Cpu.States;

namespace Cpu.Instructions.StatusChanges
{
    /// <summary>
    /// <para>Set Decimal Flag (SED)</para>
    /// <para>Set the decimal mode flag to one.</para>
    /// <para>
    /// Executes the following opcodes:
    /// <c>0xF8</c>
    /// </para>
    /// </summary>
    public sealed class SetDecimalFlag : BaseInstruction
    {
        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="SetDecimalFlag"/>
        /// </summary>
        public SetDecimalFlag()
            : base(new OpcodeInformation(0xF8, 2, 1))
        { }
        #endregion

        /// <inheritdoc/>
        public override ICpuState Execute(ICpuState currentState, ushort _)
        {
            currentState.Flags.IsDecimalMode = true;
            return currentState;
        }
    }
}
