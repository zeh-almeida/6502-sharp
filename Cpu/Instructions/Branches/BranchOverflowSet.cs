using Cpu.Extensions;
using Cpu.Opcodes;
using Cpu.States;

namespace Cpu.Instructions.Branches
{
    /// <summary>
    /// <para>Branch if Overflow Set instruction (BVC)</para>
    /// <para>If the overflow flag is clear then add the relative displacement to the program counter to cause a branch to a new location.</para>
    /// <para>
    /// Executes the following opcodes:
    /// <c>0x70</c>
    /// </para>
    /// </summary>
    /// <see href="https://masswerk.at/6502/6502_instruction_set.html#BVC"/>
    public sealed class BranchOverflowSet : BaseInstruction
    {
        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="BranchOverflowSet"/> instruction
        /// </summary>
        public BranchOverflowSet()
            : base(new OpcodeInformation(0x70, 5, 2))
        { }
        #endregion

        /// <inheritdoc/>
        public override ICpuState Execute(ICpuState currentState, ushort value)
        {
            var isFlag = currentState.Flags.IsOverflow;

            if (isFlag)
            {
                var pc = currentState.Registers.ProgramCounter;
                var valueCorrected = ((byte)value).BranchAddress();

                var newAddress = (ushort)(pc - valueCorrected);
                currentState.Registers.ProgramCounter = newAddress;
            }

            return currentState;
        }
    }
}
