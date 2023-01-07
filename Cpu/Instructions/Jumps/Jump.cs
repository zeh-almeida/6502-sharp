using Cpu.Instructions.Exceptions;
using Cpu.Opcodes;
using Cpu.States;

namespace Cpu.Instructions.Jumps
{
    /// <summary>
    /// <para>Jump instruction (JMP)</para>
    /// <para>Sets the program counter to the address specified by the operand.</para>
    /// <para>
    /// Executes the following opcodes:
    /// <c>0x4C</c>,
    /// <c>0x6C</c>
    /// </para>
    /// </summary>
    /// <see href="https://masswerk.at/6502/6502_instruction_set.html#JMP"/>
    public sealed class Jump : BaseInstruction
    {
        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="Jump"/>
        /// </summary>
        public Jump()
            : base(
                new OpcodeInformation(0x4C, 3, 3),
                new OpcodeInformation(0x6C, 5, 3))
        { }
        #endregion

        /// <inheritdoc/>
        public override void Execute(ICpuState currentState, ushort value)
        {
            currentState.Registers.ProgramCounter = Load(currentState, value);
        }

        private static ushort Load(ICpuState currentState, ushort value)
        {
            return currentState.ExecutingOpcode switch
            {
                0x4C => value,
                0x6C => currentState.Memory.ReadAbsolute(value),
                _ => throw new UnknownOpcodeException(currentState.ExecutingOpcode),
            };
        }
    }
}
