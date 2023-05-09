using Cpu.States;

namespace Cpu.Instructions.StatusChanges;

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
        : base(0xF8)
    { }
    #endregion

    /// <inheritdoc/>
    public override void Execute(ICpuState currentState, ushort value)
    {
        currentState.Flags.IsDecimalMode = true;
    }
}
