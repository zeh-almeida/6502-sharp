using Cpu.Extensions;
using Cpu.Opcodes;
using Cpu.States;

namespace Cpu.Instructions.Branches
{
    /// <summary>
    /// <para>Branch if Equal instruction (BEQ)</para>
    /// <para>If the zero flag is set then add the relative displacement to the program counter to cause a branch to a new location.</para>
    /// <para>
    /// Executes the following opcodes:
    /// <c>0xF0</c>
    /// </para>
    /// </summary>
    /// <see href="https://masswerk.at/6502/6502_instruction_set.html#BEQ"/>
    public sealed class BranchEqual : BaseInstruction
    {
        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="BranchEqual"/> instruction
        /// </summary>
        public BranchEqual()
            : base(new OpcodeInformation(0xF0, 5, 2))
        { }
        #endregion

        /// <inheritdoc/>
        public override void Execute(ICpuState currentState, ushort value)
        {
            if (currentState.Flags.IsZero)
            {
                currentState.Registers.ProgramCounter = currentState.Registers.ProgramCounter.BranchAddress((byte)value);
            }
        }
    }
}
