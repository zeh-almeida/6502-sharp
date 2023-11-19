using Cpu.Extensions;
using Cpu.States;

namespace Cpu.Instructions.Transfers;

/// <summary>
/// <para>Transfer Accumulator to X instruction (TAX)</para>
/// <para>Copies the current contents of the accumulator into the X register and sets the zero and negative flags as appropriate.</para>
/// <para>
/// Executes the following opcodes:
/// <c>0xAA</c>
/// </para>
/// </summary>
/// <see href="https://masswerk.at/6502/6502_instruction_set.html#TAX"/>
public sealed class TransferAccumulatorX : BaseInstruction
{
    #region Constructors
    /// <summary>
    /// Instantiates a new <see cref="TransferAccumulatorX"/>
    /// </summary>
    public TransferAccumulatorX()
        : base(0xAA)
    { }
    #endregion

    /// <inheritdoc/>
    public override void Execute(in ICpuState currentState, in ushort value)
    {
        var loadValue = currentState.Registers.Accumulator;

        currentState.Flags.IsZero = loadValue.IsZero();
        currentState.Flags.IsNegative = loadValue.IsLastBitSet();

        currentState.Registers.IndexX = loadValue;
    }
}
