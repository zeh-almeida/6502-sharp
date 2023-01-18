using Cpu.Extensions;
using Cpu.States;

namespace Cpu.Instructions.Branches;

/// <summary>
/// <para>Branch if Carry Clear instruction (BCC)</para>
/// <para>If the carry flag is clear then add the relative displacement to the program counter to cause a branch to a new location.</para>
/// <para>
/// Executes the following opcodes:
/// <c>0x90</c>
/// </para>
/// </summary>
/// <see href="https://masswerk.at/6502/6502_instruction_set.html#BCC"/>
public sealed class BranchCarryClear : BaseInstruction
{
    #region Constructors
    /// <summary>
    /// Instantiates a new <see cref="BranchCarryClear"/> instruction
    /// </summary>
    public BranchCarryClear()
        : base(0x90)
    { }
    #endregion

    /// <inheritdoc/>
    public override void Execute(ICpuState currentState, ushort value)
    {
        if (!currentState.Flags.IsCarry)
        {
            var currentAddress = currentState.Registers.ProgramCounter;
            var address = currentAddress.BranchAddress((byte)value);

            var additionalCycles = currentAddress.CheckPageCrossed((ushort)(currentAddress + value))
                ? 2
                : 1;

            currentState.Registers.ProgramCounter = address;
            currentState.IncrementCycles(additionalCycles);
        }
    }
}
