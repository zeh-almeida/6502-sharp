using Cpu.States;

namespace Cpu.Instructions.StatusChanges;

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
        : base(0xB8)
    { }
    #endregion

    /// <inheritdoc/>
    public override void Execute(in ICpuState currentState, in ushort value)
    {
        currentState.Flags.IsOverflow = false;
    }
}
