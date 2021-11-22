using Cpu.Opcodes;
using Cpu.States;

namespace Cpu.Instructions.ReturnFromSubroutines
{
    /// <summary>
    /// <para>Return From Subroutine instruction (RTS)</para>
    /// <para>Pushes the address (minus one) of the return point on to the stack and then sets the program counter to the target memory address.</para>
    /// <para>
    /// Executes the following opcodes:
    /// <c>0x60</c>
    /// </para>
    /// </summary>
    /// <see href="https://masswerk.at/6502/6502_instruction_set.html#RTS"/>
    public sealed class ReturnFromSubroutine : BaseInstruction
    {
        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="ReturnFromSubroutine"/>
        /// </summary>
        public ReturnFromSubroutine()
            : base(new OpcodeInformation(0x60, 6, 1))
        { }
        #endregion

        /// <inheritdoc/>
        public override void Execute(ICpuState currentState, ushort _)
        {
            currentState.Registers.ProgramCounter = currentState.Stack.Pull16();
        }
    }
}
