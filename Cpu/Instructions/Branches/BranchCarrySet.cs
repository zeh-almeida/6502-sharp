using Cpu.Extensions;
using Cpu.States;

namespace Cpu.Instructions.Branches;

/// <summary>
/// <para>Branch if Carry Clear instruction (BCS)</para>
/// <para>If the carry flag is set then add the relative displacement to the program counter to cause a branch to a new location.</para>
/// <para>
/// Executes the following opcodes:
/// <c>0xB0</c>
/// </para>
/// </summary>
/// <see href="https://masswerk.at/6502/6502_instruction_set.html#BCS"/>
public sealed class BranchCarrySet : BaseInstruction
{
    #region Constructors
    /// <summary>
    /// Instantiates a new <see cref="BranchCarrySet"/> instruction
    /// </summary>
    public BranchCarrySet()
        : base(0xB0)
    { }
    #endregion

    /// <inheritdoc/>
    public override void Execute(ICpuState currentState, ushort value)
    {
        if (currentState.Flags.IsCarry)
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
