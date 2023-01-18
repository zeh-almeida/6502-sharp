using Cpu.Extensions;
using Cpu.Instructions.Exceptions;
using Cpu.States;

namespace Cpu.Instructions.Arithmetic;

/// <summary>
/// <para>Compare X Register instruction (CPX)</para>
/// <para>Compares the contents of the X register with another memory held value and sets the zero and carry flags as appropriate.</para>
/// <para>
/// Executes the following opcodes:
/// <c>0xE0</c>,
/// <c>0xE4</c>,
/// <c>0xEC</c>
/// </para>
/// </summary>
/// <see href="https://masswerk.at/6502/6502_instruction_set.html#CPX"/>
public sealed class CompareRegisterX : BaseInstruction
{
    #region Constructors
    /// <summary>
    /// Instantiates a new <see cref="CompareRegisterX"/> instruction
    /// </summary>
    public CompareRegisterX()
        : base(
            0xE0,
            0xE4,
            0xEC)
    { }
    #endregion

    /// <inheritdoc/>
    public override void Execute(ICpuState currentState, ushort value)
    {
        var loadValue = Load(currentState, value);
        var register = currentState.Registers.IndexX;

        var operation = (byte)(register - loadValue);

        currentState.Flags.IsZero = operation.IsZero();
        currentState.Flags.IsNegative = operation.IsLastBitSet();
        currentState.Flags.IsCarry = loadValue <= register;
    }

    private static ushort Load(ICpuState currentState, ushort address)
    {
        return currentState.ExecutingOpcode switch
        {
            0xE0 => address,
            0xE4 => currentState.Memory.ReadZeroPage(address),
            0xEC => currentState.Memory.ReadAbsolute(address),
            _ => throw new UnknownOpcodeException(currentState.ExecutingOpcode),
        };
    }
}
