using Cpu.States;

namespace Cpu.Instructions.StatusChanges;

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
        : base(0x38)
    { }
    #endregion

    /// <inheritdoc/>
    public override void Execute(in ICpuState currentState, in ushort value)
    {
        currentState.Flags.IsCarry = true;
    }
}
