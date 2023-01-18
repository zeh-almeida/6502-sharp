using Cpu.Extensions;
using Cpu.Instructions.Exceptions;
using Cpu.States;

namespace Cpu.Instructions.Illegal;

/// <summary>
/// <para>Decrement and Compare instruction (DCP/DCM)</para>
/// <para>Illegal, decrements value and then compares</para>
/// <para>
/// Executes the following opcodes:
/// <c>0xC7</c>,
/// <c>0xD7</c>,
/// <c>0xCF</c>,
/// <c>0xDF</c>,
/// <c>0xDB</c>,
/// <c>0xC3</c>,
/// <c>0xD3</c>
/// </para>
/// </summary>
/// <see href="https://masswerk.at/6502/6502_instruction_set.html#DCP"/>
/// <seealso cref="Decrements.DecrementMemory"/>
/// <seealso cref="Arithmetic.CompareAccumulator"/>
public sealed class DecrementCompare : BaseInstruction
{
    #region Constructors
    /// <summary>
    /// Instantiates a new <see cref="DecrementCompare"/>
    /// </summary>
    public DecrementCompare()
        : base(
            0xC7,
            0xD7,
            0xCF,
            0xDF,
            0xDB,
            0xC3,
            0xD3)
    { }
    #endregion

    /// <inheritdoc/>
    public override void Execute(ICpuState currentState, ushort value)
    {
        var accumulator = currentState.Registers.Accumulator;
        var loadValue = Load(currentState, value);

        var operation = (byte)(loadValue - 1);
        var result = (byte)(accumulator - operation);

        currentState.Flags.IsZero = operation.Equals(accumulator);
        currentState.Flags.IsNegative = operation.IsLastBitSet();
        currentState.Flags.IsCarry = operation <= accumulator;

        Write(currentState, value, result);
    }

    private static byte Load(ICpuState currentState, ushort address)
    {
        return currentState.ExecutingOpcode switch
        {
            0xC7 => currentState.Memory.ReadZeroPage(address),
            0xD7 => currentState.Memory.ReadZeroPageX(address),
            0xCF => currentState.Memory.ReadAbsolute(address),
            0xDF => currentState.Memory.ReadAbsoluteX(address).Item2,
            0xDB => currentState.Memory.ReadAbsoluteY(address).Item2,
            0xC3 => currentState.Memory.ReadIndirectX(address),
            0xD3 => currentState.Memory.ReadIndirectY(address).Item2,
            _ => throw new UnknownOpcodeException(currentState.ExecutingOpcode),
        };
    }

    private static void Write(ICpuState currentState, ushort address, byte value)
    {
        switch (currentState.ExecutingOpcode)
        {
            case 0xC7:
                currentState.Memory.WriteZeroPage(address, value);
                break;

            case 0xD7:
                currentState.Memory.WriteZeroPageX(address, value);
                break;

            case 0xCF:
                currentState.Memory.WriteAbsolute(address, value);
                break;

            case 0xDF:
                currentState.Memory.WriteAbsoluteX(address, value);
                break;

            case 0xDB:
                currentState.Memory.WriteAbsoluteY(address, value);
                break;

            case 0xC3:
                currentState.Memory.WriteIndirectX(address, value);
                break;

            case 0xD3:
            default:
                currentState.Memory.WriteIndirectY(address, value);
                break;
        }
    }
}
