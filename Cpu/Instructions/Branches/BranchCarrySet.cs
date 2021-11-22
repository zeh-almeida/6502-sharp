using Cpu.Extensions;
using Cpu.Opcodes;
using Cpu.States;

namespace Cpu.Instructions.Branches
{
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
        /// Instantiates a new <see cref="BranchCarryClear"/> instruction
        /// </summary>
        public BranchCarrySet()
            : base(new OpcodeInformation(0xB0, 5, 2))
        { }
        #endregion

        /// <inheritdoc/>
        public override void Execute(ICpuState currentState, ushort value)
        {
            var isFlag = currentState.Flags.IsCarry;

            if (isFlag)
            {
                var pc = currentState.Registers.ProgramCounter;
                var valueCorrected = ((byte)value).BranchAddress();

                var newAddress = (ushort)(pc - valueCorrected);
                currentState.Registers.ProgramCounter = newAddress;
            }

        }
    }
}
