using Cpu.Extensions;
using Cpu.States;
using System.Diagnostics.CodeAnalysis;

namespace Cpu.Instructions;

/// <summary>
/// Defines common functions of any <see cref="IInstruction"/> implementation
/// </summary>
public abstract class BaseInstruction : IInstruction
{
    #region Constants
    /// <summary>
    /// Amount of additional clocks when a branch is taken
    /// </summary>
    public const int BranchTaken = 2;

    /// <summary>
    /// Amount of additional clocks when a branch is not taken
    /// </summary>
    public const int BranchNotTaken = 1;
    #endregion

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

    #region Equality
    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return this.Opcodes.GetHashCode();
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is BaseInstruction instruction
            && this.Equals(instruction);
    }

    /// <inheritdoc/>
    public bool Equals(IInstruction? other)
    {
        return other is not null
            && this.Opcodes.Equals(other.Opcodes);
    }
    #endregion

    /// <inheritdoc/>
    public abstract void Execute(in ICpuState currentState, in ushort value);

    /// <inheritdoc/>
    public bool HasOpcode(in byte opcode)
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
        [NotNull] in ICpuState currentState,
        (bool, byte) result,
        in int additionalCycles = 1)
    {
        if (result.Item1)
        {
            currentState.IncrementCycles(additionalCycles);
        }

        return result.Item2;
    }

    /// <summary>
    /// Performs the branch path for a instruction
    /// </summary>
    /// <param name="currentState">State to read and write to</param>
    /// <param name="value">Branch address to take</param>
    protected static void ExecuteBranch([NotNull] in ICpuState currentState, in ushort value)
    {
        var currentAddress = currentState.Registers.ProgramCounter;
        currentState.Registers.ProgramCounter = currentAddress.BranchAddress((byte)value);

        var additionalCycles = GetAdditionalCycles(currentAddress, value);
        currentState.IncrementCycles(additionalCycles);
    }

    /// <summary>
    /// Calculates the amount of additional cycles when crossing pages
    /// </summary>
    /// <param name="address">Initial address</param>
    /// <param name="value">Amount of addresses to jump</param>
    /// <returns>Amount of additional clock cycles for the branch</returns>
    private static int GetAdditionalCycles(in ushort address, in ushort value)
    {
        return address.CheckPageCrossed((ushort)(address + value))
             ? BranchTaken
             : BranchNotTaken;
    }
}
