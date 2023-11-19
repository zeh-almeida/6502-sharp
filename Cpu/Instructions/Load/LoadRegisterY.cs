using Cpu.Extensions;
using Cpu.Instructions.Exceptions;
using Cpu.States;

namespace Cpu.Instructions.Load;

/// <summary>
/// <para>Load Y Register Instruction (LDY)</para>
/// <para>Loads a byte of memory into the Y register setting the zero and negative flags as appropriate.</para>
/// <para>
/// Executes the following opcodes:
/// <c>0xA0</c>,
/// <c>0xA4</c>,
/// <c>0xB4</c>,
/// <c>0xAC</c>,
/// <c>0xBC</c>
/// </para>
/// </summary>
public sealed class LoadRegisterY : BaseInstruction
{
    #region Constructors
    /// <summary>
    /// Instantiates a new <see cref="LoadRegisterY"/>
    /// </summary>
    public LoadRegisterY()
        : base(
            0xA0,
            0xA4,
            0xB4,
            0xAC,
            0xBC)
    { }
    #endregion

    /// <inheritdoc/>
    public override void Execute(in ICpuState currentState, in ushort value)
    {
        var loadValue = Load(currentState, value);

        currentState.Flags.IsZero = loadValue.IsZero();
        currentState.Flags.IsNegative = loadValue.IsLastBitSet();

        currentState.Registers.IndexY = loadValue;
    }

    private static byte Load(ICpuState currentState, ushort address)
    {
        return currentState.ExecutingOpcode switch
        {
            0xA0 => (byte)address,
            0xA4 => currentState.Memory.ReadZeroPage(address),
            0xB4 => currentState.Memory.ReadZeroPageX(address),
            0xAC => currentState.Memory.ReadAbsolute(address),
            0xBC => LoadExtraCycle(currentState, currentState.Memory.ReadAbsoluteX(address)),
            _ => throw new UnknownOpcodeException(currentState.ExecutingOpcode),
        };
    }
}
