using Cpu.States;

namespace Cpu.Instructions;

/// <summary>
/// Definition of a CPU instruction
/// </summary>
public interface IInstruction
    : IEquatable<IInstruction>
{
    #region Properties
    /// <summary>
    /// Opcode bytes this instruction supports
    /// </summary>
    IEnumerable<byte> Opcodes { get; }
    #endregion

    /// <summary>
    /// Executes the instruction.
    /// <para>May use <see cref="ICpuState"/> to manipulate memory, stack, flags and registers</para>
    /// </summary>
    /// <param name="currentState">Current CPU state</param>
    /// <param name="value">Operand, not always necessary</param>
    /// <returns>Modified CPU state</returns>
    void Execute(in ICpuState currentState, in ushort value);

    /// <summary>
    /// Checks if the instruction supports an specific opcode
    /// </summary>
    /// <param name="opcode">To verify</param>
    /// <returns>True if supported, false otherwise</returns>
    bool HasOpcode(in byte opcode);
}
