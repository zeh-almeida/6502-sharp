﻿using Cpu.Opcodes;
using Cpu.States;

namespace Cpu.Instructions.Stack
{
    /// <summary>
    /// <para>Push Processor Status instruction (PHP)</para>
    /// <para>Pushes a copy of the status flags on to the stack.</para>
    /// <para>
    /// Executes the following opcodes:
    /// <c>0x08</c>
    /// </para>
    /// </summary>
    /// <see href="https://masswerk.at/6502/6502_instruction_set.html#PHP"/>
    public sealed class PushProcessorStatus : BaseInstruction
    {
        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="PushProcessorStatus"/>
        /// </summary>
        public PushProcessorStatus()
            : base(new OpcodeInformation(0x08, 3, 1))
        { }
        #endregion

        /// <inheritdoc/>
        public override ICpuState Execute(ICpuState currentState, ushort _)
        {
            var bits = currentState.Flags.Save();
            currentState.Stack.Push(bits);

            return currentState;
        }
    }
}
