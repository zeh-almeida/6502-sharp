using Cpu.Extensions;
using Cpu.States;

namespace Cpu.Instructions.SystemFunctions
{
    /// <summary>
    /// <para>Break Interrupt instruction (BRK)</para>
    /// <para>
    /// Forces the generation of an interrupt request.
    /// The program counter and processor status are pushed on the stack
    /// then the IRQ interrupt vector at $FFFE/F is loaded into the PC
    /// and the break flag in the status set to one.
    /// </para>
    /// <para>
    /// Executes the following opcodes:
    /// <c>0x00</c>
    /// </para>
    /// </summary>
    /// <see href="https://masswerk.at/6502/6502_instruction_set.html#BRK"/>
    public sealed class ForceInterrupt : BaseInstruction
    {
        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="ForceInterrupt"/>
        /// </summary>
        public ForceInterrupt()
            : base(0x00)
        { }
        #endregion

        /// <inheritdoc/>
        public override void Execute(ICpuState currentState, ushort _)
        {
            StoreProcessorStatus(currentState);
            StoreProgramCounter(currentState);

            LoadInterruptProgramAddress(currentState);

            currentState.Flags.IsInterruptDisable = true;
        }

        private static void StoreProcessorStatus(ICpuState currentState)
        {
            currentState.Flags.IsBreakCommand = true;

            var bits = currentState.Flags.Save();
            currentState.Stack.Push(bits);
        }

        private static void StoreProgramCounter(ICpuState currentState)
        {
            currentState.Stack.Push16(currentState.Registers.ProgramCounter);
        }

        private static void LoadInterruptProgramAddress(ICpuState currentState)
        {
            var msb = currentState.Memory.ReadAbsolute(0xFFFF);
            var lsb = currentState.Memory.ReadAbsolute(0xFFFE);

            currentState.Registers.ProgramCounter = lsb.CombineBytes(msb);
        }
    }
}
