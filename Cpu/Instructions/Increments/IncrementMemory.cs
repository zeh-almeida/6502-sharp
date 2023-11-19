using Cpu.Extensions;
using Cpu.Instructions.Exceptions;
using Cpu.States;

namespace Cpu.Instructions.Increments;

/// <summary>
/// <para>Increment Memory instruction (INC)</para>
/// <para>Adds one to the value held at a specified memory location setting the zero and negative flags as appropriate.</para>
/// <para>
/// Executes the following opcodes:
/// <c>0xE6</c>,
/// <c>0xF6</c>,
/// <c>0xEE</c>,
/// <c>0xFE</c>
/// </para>
/// </summary>
/// <see href="https://masswerk.at/6502/6502_instruction_set.html#INC"/>
public sealed class IncrementMemory : BaseInstruction
{
    #region Constructors
    /// <summary>
    /// Instantiates a new <see cref="IncrementMemory"/>
    /// </summary>
    public IncrementMemory()
        : base(
            0xE6,
            0xF6,
            0xEE,
            0xFE)
    { }
    #endregion

    /// <inheritdoc/>
    public override void Execute(in ICpuState currentState, in ushort value)
    {
        var operation = Read(currentState, value);
        operation = (byte)(operation + 1);

        currentState.Flags.IsZero = operation.IsZero();
        currentState.Flags.IsNegative = operation.IsLastBitSet();

        Write(currentState, value, operation);
    }

    private static void Write(ICpuState currentState, ushort address, byte value)
    {
        switch (currentState.ExecutingOpcode)
        {
            case 0xE6:
                currentState.Memory.WriteZeroPage(address, value);
                break;

            case 0xF6:
                currentState.Memory.WriteZeroPageX(address, value);
                break;

            case 0xEE:
                currentState.Memory.WriteAbsolute(address, value);
                break;

            case 0xFE:
            default:
                currentState.Memory.WriteAbsoluteX(address, value);
                break;
        }
    }

    private static byte Read(ICpuState currentState, ushort address)
    {
        return currentState.ExecutingOpcode switch
        {
            0xE6 => currentState.Memory.ReadZeroPage(address),
            0xF6 => currentState.Memory.ReadZeroPageX(address),
            0xEE => currentState.Memory.ReadAbsolute(address),
            0xFE => currentState.Memory.ReadAbsoluteX(address).Item2,
            _ => throw new UnknownOpcodeException(currentState.ExecutingOpcode),
        };
    }
}
