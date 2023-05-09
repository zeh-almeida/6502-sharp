using Cpu.Extensions;
using Cpu.States;

namespace Cpu.Instructions.Stack;

/// <summary>
/// <para>Transfer Stack Pointer to X instruction (TSX)</para>
/// <para>Copies the current contents of the stack register into the X register and sets the zero and negative flags as appropriate.</para>
/// <para>
/// Executes the following opcodes:
/// <c>0xBA</c>
/// </para>
/// </summary>
/// <see href="https://masswerk.at/6502/6502_instruction_set.html#TSX"/>
public sealed class TransferStackX : BaseInstruction
{
    #region Constructors
    /// <summary>
    /// Instantiates a new <see cref="TransferStackX"/>
    /// </summary>
    public TransferStackX()
        : base(0xBA)
    { }
    #endregion

    /// <inheritdoc/>
    public override void Execute(ICpuState currentState, ushort value)
    {
        var loadValue = currentState.Registers.StackPointer;

        currentState.Flags.IsZero = loadValue.IsZero();
        currentState.Flags.IsNegative = loadValue.IsLastBitSet();

        currentState.Registers.IndexX = loadValue;
    }
}
