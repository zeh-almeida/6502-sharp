using Cpu.Extensions;
using Cpu.Instructions.Exceptions;
using Cpu.States;

namespace Cpu.Instructions.Shifts;

/// <summary>
/// <para>Rotate Left instruction (ROL)</para>
/// <para>
/// Move each of the bits in either A or M one place to the left.
/// Bit 0 is filled with the current value of the carry flag whilst the old bit 7 becomes the new carry flag value.
/// </para>
/// <para>
/// Executes the following opcodes:
/// <c>0x2A</c>,
/// <c>0x26</c>,
/// <c>0x36</c>,
/// <c>0x2E</c>,
/// <c>0x3E</c>
/// </para>
/// </summary>
/// <see href="https://masswerk.at/6502/6502_instruction_set.html#ROL"/>
public sealed class RotateLeft : BaseInstruction
{
    #region Constructors
    /// <summary>
    /// Instantiates a new <see cref="RotateLeft"/>
    /// </summary>
    public RotateLeft()
        : base(0x2A, 0x26, 0x36, 0x2E, 0x3E)
    { }
    #endregion

    /// <inheritdoc/>
    public override void Execute(ICpuState currentState, ushort value)
    {
        var loadValue = Load(currentState, value);
        var newCarry = loadValue.IsLastBitSet();

        var shifted = loadValue.RotateLeft(currentState.Flags.IsCarry);
        Write(currentState, value, shifted);

        currentState.Flags.IsCarry = newCarry;
        currentState.Flags.IsZero = shifted.IsZero();
        currentState.Flags.IsNegative = shifted.IsLastBitSet();
    }

    private static byte Load(ICpuState currentState, ushort address)
    {
        return currentState.ExecutingOpcode switch
        {
            0x2A => currentState.Registers.Accumulator,
            0x26 => currentState.Memory.ReadZeroPage(address),
            0x36 => currentState.Memory.ReadZeroPageX(address),
            0x2E => currentState.Memory.ReadAbsolute(address),
            0x3E => currentState.Memory.ReadAbsoluteX(address).Item2,
            _ => throw new UnknownOpcodeException(currentState.ExecutingOpcode),
        };
    }

    private static void Write(ICpuState currentState, ushort address, byte value)
    {
        switch (currentState.ExecutingOpcode)
        {
            case 0x2A:
                currentState.Registers.Accumulator = value;
                break;

            case 0x26:
                currentState.Memory.WriteZeroPage(address, value);
                break;

            case 0x36:
                currentState.Memory.WriteZeroPageX(address, value);
                break;

            case 0x2E:
                currentState.Memory.WriteAbsolute(address, value);
                break;

            case 0x3E:
            default:
                currentState.Memory.WriteAbsoluteX(address, value);
                break;
        }
    }
}
