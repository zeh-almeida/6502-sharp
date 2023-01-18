using Cpu.States;

namespace Cpu.Instructions;

/// <summary>
/// Defines common functions of any <see cref="IInstruction"/> implementation
/// </summary>
public abstract class BaseInstruction : IInstruction
{
    #region Properties
    /// <inheritdoc/>
    public IEnumerable<byte> Opcodes { get; }
    #endregion

    #region Constructors
    /// <summary>
    /// Instantiates the base class
    /// </summary>
    /// <param name="opcodes">Allowed opcode and their respective information</param>
    protected BaseInstruction(params byte[] opcodes)
    {
        this.Opcodes = new HashSet<byte>(opcodes);
    }
    #endregion

    /// <inheritdoc/>
    public abstract void Execute(ICpuState currentState, ushort value);

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return this.Opcodes.GetHashCode();
    }

    /// <inheritdoc/>
    public override bool Equals(object? other)
    {
        return other is BaseInstruction instruction
            && this.Equals(instruction);
    }

    /// <inheritdoc/>
    public bool Equals(IInstruction? other)
    {
        return other is not null
            && this.Opcodes.Equals(other.Opcodes);
    }

    /// <inheritdoc/>
    public bool HasOpcode(byte opcode)
    {
        return this.Opcodes.Contains(opcode);
    }

    /// <summary>
    /// Increases the total execution cycles when a memory operation crosses boundaries
    /// </summary>
    /// <param name="currentState"><see cref="ICpuState"/> reference to increase cycles</param>
    /// <param name="result">If marked as true, must increase the cycles</param>
    /// <param name="additionalCycles">Number of cycles to add</param>
    /// <returns>The value read from memory</returns>
    protected static byte LoadExtraCycle(
        ICpuState currentState,
        (bool, byte) result,
        int additionalCycles = 1)
    {
        if (result.Item1)
        {
            currentState.IncrementCycles(additionalCycles);
        }

        return result.Item2;
    }
}
