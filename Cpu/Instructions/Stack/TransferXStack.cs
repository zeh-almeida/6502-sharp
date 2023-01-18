using Cpu.Extensions;
using Cpu.States;

namespace Cpu.Instructions.Stack;

/// <summary>
/// <para>Transfer X to Stack Pointer instruction (TXS)</para>
/// <para>Copies the current contents of the X register into the stack register.</para>
/// <para>
/// Executes the following opcodes:
/// <c>0x9A</c>
/// </para>
/// </summary>
/// <see href="https://masswerk.at/6502/6502_instruction_set.html#TXS"/>
public sealed class TransferXStack : BaseInstruction
{
    #region Constructors
    /// <summary>
    /// Instantiates a new <see cref="TransferXStack"/>
    /// </summary>
    public TransferXStack()
        : base(0x9A)
    { }
    #endregion

    /// <inheritdoc/>
    public override void Execute(ICpuState currentState, ushort _)
    {
        var loadValue = currentState.Registers.IndexX;

        currentState.Flags.IsZero = loadValue.IsZero();
        currentState.Flags.IsNegative = loadValue.IsLastBitSet();

        currentState.Registers.StackPointer = loadValue;
    }
}
