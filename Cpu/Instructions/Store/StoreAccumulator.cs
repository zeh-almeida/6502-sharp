using Cpu.Instructions.Exceptions;
using Cpu.States;

namespace Cpu.Instructions.Store;

/// <summary>
/// <para>Store Accumulator instruction (STA)</para>
/// <para>Stores the contents of the accumulator into memory.</para>
/// <para>
/// Executes the following opcodes:
/// <c>0x85</c>,
/// <c>0x95</c>,
/// <c>0x8D</c>,
/// <c>0x9D</c>,
/// <c>0x99</c>,
/// <c>0x81</c>,
/// <c>0x91</c>
/// </para>
/// </summary>
/// <see href="https://masswerk.at/6502/6502_instruction_set.html#STA"/>
public sealed class StoreAccumulator : BaseInstruction
{
    #region Constructors
    /// <summary>
    /// Instantiates a new <see cref="StoreAccumulator"/>
    /// </summary>
    public StoreAccumulator()
        : base(
            0x85,
            0x95,
            0x8D,
            0x9D,
            0x99,
            0x81,
            0x91)
    { }
    #endregion

    /// <inheritdoc/>
    public override void Execute(ICpuState currentState, ushort value)
    {
        Write(currentState, value);
    }

    private static void Write(ICpuState currentState, ushort address)
    {
        var value = currentState.Registers.Accumulator;

        switch (currentState.ExecutingOpcode)
        {
            case 0x85:
                currentState.Memory.WriteZeroPage(address, value);
                break;

            case 0x95:
                currentState.Memory.WriteZeroPageX(address, value);
                break;

            case 0x8D:
                currentState.Memory.WriteAbsolute(address, value);
                break;

            case 0x9D:
                currentState.Memory.WriteAbsoluteX(address, value);
                break;

            case 0x99:
                currentState.Memory.WriteAbsoluteY(address, value);
                break;

            case 0x81:
                currentState.Memory.WriteIndirectX(address, value);
                break;

            case 0x91:
                currentState.Memory.WriteIndirectY(address, value);
                break;

            default:
                throw new UnknownOpcodeException(currentState.ExecutingOpcode);
        }
    }
}
