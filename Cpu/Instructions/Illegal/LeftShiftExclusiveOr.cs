using Cpu.Extensions;
using Cpu.Instructions.Exceptions;
using Cpu.States;

namespace Cpu.Instructions.Illegal;

/// <summary>
/// <para>Rotate Left and EOR instruction (SRE/LSE)</para>
/// <para>Illegal, shift left one bit in memory, then OR accumulator with memory</para>
/// <para>
/// Executes the following opcodes:
/// <c>0x47</c>,
/// <c>0x57</c>,
/// <c>0x4F</c>,
/// <c>0x5F</c>,
/// <c>0x5B</c>,
/// <c>0x43</c>,
/// <c>0x53</c>
/// </para>
/// </summary>
/// <see href="https://masswerk.at/6502/6502_instruction_set.html#SRE"/>
/// <seealso cref="Shifts.ArithmeticShiftLeft"/>
/// <seealso cref="Logic.ExclusiveOr"/>
public sealed class LeftShiftExclusiveOr : BaseInstruction
{
    #region Constructors
    /// <summary>
    /// Instantiates a new <see cref="LeftShiftExclusiveOr"/>
    /// </summary>
    public LeftShiftExclusiveOr()
        : base(0x47, 0x57, 0x4F, 0x5F, 0x5B, 0x43, 0x53)
    { }
    #endregion

    /// <inheritdoc/>
    public override void Execute(ICpuState currentState, ushort value)
    {
        var accumulator = currentState.Registers.Accumulator;
        var loadValue = Load(currentState, value);

        var shifted = (byte)(loadValue << 1);
        var result = (byte)(shifted ^ accumulator);

        currentState.Flags.IsZero = result.IsZero();
        currentState.Flags.IsNegative = result.IsLastBitSet();
        currentState.Flags.IsCarry = loadValue.IsLastBitSet();

        Write(currentState, value, result);
    }

    private static byte Load(ICpuState currentState, ushort address)
    {
        return currentState.ExecutingOpcode switch
        {
            0x47 => currentState.Memory.ReadZeroPage(address),
            0x57 => currentState.Memory.ReadZeroPageX(address),
            0x4F => currentState.Memory.ReadAbsolute(address),
            0x5F => currentState.Memory.ReadAbsoluteX(address).Item2,
            0x5B => currentState.Memory.ReadAbsoluteY(address).Item2,
            0x43 => currentState.Memory.ReadIndirectX(address),
            0x53 => currentState.Memory.ReadIndirectY(address).Item2,
            _ => throw new UnknownOpcodeException(currentState.ExecutingOpcode),
        };
    }

    private static void Write(ICpuState currentState, ushort address, byte value)
    {
        switch (currentState.ExecutingOpcode)
        {
            case 0x47:
                currentState.Memory.WriteZeroPage(address, value);
                break;

            case 0x57:
                currentState.Memory.WriteZeroPageX(address, value);
                break;

            case 0x4F:
                currentState.Memory.WriteAbsolute(address, value);
                break;

            case 0x5F:
                currentState.Memory.WriteAbsoluteX(address, value);
                break;

            case 0x5B:
                currentState.Memory.WriteAbsoluteY(address, value);
                break;

            case 0x43:
                currentState.Memory.WriteIndirectX(address, value);
                break;

            case 0x53:
            default:
                currentState.Memory.WriteIndirectY(address, value);
                break;
        }
    }
}
