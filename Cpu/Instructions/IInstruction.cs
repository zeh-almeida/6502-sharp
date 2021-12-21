using Cpu.Opcodes;
using Cpu.States;

namespace Cpu.Instructions
{
    /// <summary>
    /// Definition of a CPU instruction
    /// </summary>
    public interface IInstruction
        : IEquatable<IInstruction>
    {
        #region Properties
        /// <summary>
        /// <see cref="OpcodeInformation"/> this instruction supports
        /// </summary>
        IEnumerable<OpcodeInformation> Opcodes { get; }
        #endregion

        /// <summary>
        /// Executes the instruction.
        /// <para>May use <see cref="ICpuState"/> to manipulate memory, stack, flags and registers</para>
        /// </summary>
        /// <param name="currentState">Current CPU state</param>
        /// <param name="value">Operand, not always necessary</param>
        /// <returns>Modified CPU state</returns>
        void Execute(ICpuState currentState, ushort value);

        /// <summary>
        /// Checks if the instruction supports an specific opcode
        /// </summary>
        /// <param name="opcode">To verify</param>
        /// <returns>True if supported, false otherwise</returns>
        bool HasOpcode(byte opcode);

        /// <summary>
        /// Returns the necessary information for a specific supported opcode
        /// </summary>
        /// <param name="opcode">To verify</param>
        /// <returns>Known opcode information</returns>
        /// <exception cref="Exceptions.UnknownOpcodeException">thrown if opcode is not supported by this instruction</exception>
        OpcodeInformation GatherInformation(byte opcode);
    }
}
