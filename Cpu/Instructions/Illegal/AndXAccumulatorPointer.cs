using Cpu.States;

namespace Cpu.Instructions.Illegal;

/// <summary>
/// <para>Logical AND with Register X and Accumulator instruction (TAS/SAS/SHS)</para>
/// <para>Illegal and unstable, executes a logical AND with the R register and Accumulator
/// then set it as the Stack Pointer
/// then executes a logical AND with operand high bit +1
/// then stores in the Operand address</para>
/// <para>May sometimes drop off the High Byte or not cross the memory boundaries correctly, hence the unstable definition</para>
/// <para>In this implementation, it doesn't suffer from instabilities</para>
/// <para>
/// Executes the following opcodes:
/// <c>0x9B</c>
/// </para>
/// </summary>
/// <see href="https://masswerk.at/6502/6502_instruction_set.html#TAS"/>
/// <seealso cref="Logic.LogicAnd"/>
/// <seealso cref="Stack.TransferXPointer"/>
/// <seealso cref="Store.StoreAccumulator"/>
/// <seealso cref="Stack.TransferPointerX"/>
/// <seealso cref="Load.LoadAccumulator"/>
public sealed class AndXAccumulatorPointer : BaseInstruction
{
    #region Constructors
    /// <summary>
    /// Instantiates a new <see cref="AndXAccumulatorPointer"/>
    /// </summary>
    public AndXAccumulatorPointer()
        : base(0x9B)
    { }
    #endregion

    /// <inheritdoc/>
    public override void Execute(ICpuState currentState, ushort value)
    {
        var highByte = (byte)((byte)(value >> 8) + 1);

        var registerX = currentState.Registers.IndexX;
        var accumulator = currentState.Registers.Accumulator;

        var spValue = (byte)(registerX & accumulator);
        var memoryValue = (byte)(spValue & highByte);

        currentState.Registers.StackPointer = spValue;
        currentState.Memory.WriteAbsoluteY(value, memoryValue);
    }
}
