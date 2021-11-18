using Cpu.Opcodes;
using Cpu.States;

namespace Cpu.Instructions.SystemFunctions
{
    /// <summary>
    /// <para>No Operation instruction (NOP)</para>
    /// <para>Causes no changes to the processor other than the normal incrementing of the program counter to the next instruction.</para>
    /// </summary>
    /// <para>
    /// Executes the following opcodes:
    /// <c>0xEA</c>
    /// </para>
    /// <see href="https://masswerk.at/6502/6502_instruction_set.html#NOP"/>
    public sealed class NoOperation : BaseInstruction
    {
        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="NoOperation"/>
        /// </summary>
        public NoOperation()
            : base(new OpcodeInformation(0xEA, 2, 1))
        { }
        #endregion

        /// <inheritdoc/>
        public override ICpuState Execute(ICpuState currentState, ushort _)
        {
            return currentState;
        }
    }
}
