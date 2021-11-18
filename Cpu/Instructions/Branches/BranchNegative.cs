﻿using Cpu.Extensions;
using Cpu.Opcodes;
using Cpu.States;

namespace Cpu.Instructions.Branches
{
    /// <summary>
    /// <para>Branch if Minus instruction (BMI)</para>
    /// <para>If the negative flag is set then add the relative displacement to the program counter to cause a branch to a new location.</para>
    /// <para>
    /// Executes the following opcodes:
    /// <c>0x30</c>
    /// </para>
    /// </summary>
    /// <see href="https://masswerk.at/6502/6502_instruction_set.html#BMI"/>
    public sealed class BranchNegative : BaseInstruction
    {
        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="BranchNegative"/> instruction
        /// </summary>
        public BranchNegative()
            : base(new OpcodeInformation(0x30, 5, 2))
        { }
        #endregion

        /// <inheritdoc/>
        public override ICpuState Execute(ICpuState currentState, ushort value)
        {
            var isFlag = currentState.Flags.IsNegative;

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
