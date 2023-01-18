using Cpu.Extensions;
using Cpu.Instructions.Exceptions;
using Cpu.States;

namespace Cpu.Instructions.Logic;

/// <summary>
/// <para>Logical AND instruction (AND)</para>
/// <para>A logical AND is performed, bit by bit, on the accumulator contents using the contents of a byte of memory.</para>
/// <para>
/// Executes the following opcodes:
/// <c>0x29</c>,
/// <c>0x25</c>,
/// <c>0x35</c>,
/// <c>0x2D</c>,
/// <c>0x3D</c>,
/// <c>0x39</c>,
/// <c>0x21</c>,
/// <c>0x31</c>
/// </para>
/// </summary>
/// <see href="https://masswerk.at/6502/6502_instruction_set.html#AND"/>
public sealed class LogicAnd : BaseInstruction
{
    #region Constructors
    /// <summary>
    /// Instantiates a new <see cref="LogicAnd"/>
    /// </summary>
    public LogicAnd()
        : base(0x29, 0x25, 0x35, 0x2D, 0x3D, 0x39, 0x21, 0x31)
    { }
    #endregion

    /// <inheritdoc/>
    public override void Execute(ICpuState currentState, ushort value)
    {
        var toCompare = ValueToCompare(currentState, value);
        var result = (byte)(toCompare & currentState.Registers.Accumulator);

        currentState.Flags.IsZero = result.IsZero();
        currentState.Flags.IsNegative = result.IsLastBitSet();

        currentState.Registers.Accumulator = result;
    }

    private static byte ValueToCompare(ICpuState currentState, ushort address)
    {
        return currentState.ExecutingOpcode switch
        {
            0x29 => (byte)address,
            0x25 => currentState.Memory.ReadZeroPage(address),
            0x35 => currentState.Memory.ReadZeroPageX(address),
            0x2D => currentState.Memory.ReadAbsolute(address),
            0x3D => currentState.Memory.ReadAbsoluteX(address).Item2,
            0x39 => currentState.Memory.ReadAbsoluteY(address).Item2,
            0x21 => currentState.Memory.ReadIndirectX(address),
            0x31 => currentState.Memory.ReadIndirectY(address).Item2,
            _ => throw new UnknownOpcodeException(currentState.ExecutingOpcode),
        };
    }
}
