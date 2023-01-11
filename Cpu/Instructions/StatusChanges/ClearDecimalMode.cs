using Cpu.States;

namespace Cpu.Instructions.StatusChanges
{
    /// <summary>
    /// <para>Clear Decimal Mode Instruction (CLD)</para>
    /// <para>Sets the decimal mode flag to zero.</para>
    /// <para>
    /// Executes the following opcodes:
    /// <c>0xD8</c>
    /// </para>
    /// </summary>
    public sealed class ClearDecimalMode : BaseInstruction
    {
        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="ClearDecimalMode"/>
        /// </summary>
        public ClearDecimalMode()
            : base(0xD8)
        { }
        #endregion

        /// <inheritdoc/>
        public override void Execute(ICpuState currentState, ushort _)
        {
            currentState.Flags.IsDecimalMode = false;
        }
    }
}
