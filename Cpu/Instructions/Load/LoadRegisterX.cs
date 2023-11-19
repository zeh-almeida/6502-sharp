using Cpu.Extensions;
using Cpu.Instructions.Exceptions;
using Cpu.States;

namespace Cpu.Instructions.Load;

/// <summary>
/// <para>Load X Register Instruction (LDX)</para>
/// <para>Loads a byte of memory into the X register setting the zero and negative flags as appropriate.</para>
/// <para>
/// Executes the following opcodes:
/// <c>0xA2</c>,
/// <c>0xA6</c>,
/// <c>0xB6</c>,
/// <c>0xAE</c>,
/// <c>0xBE</c>
/// </para>
/// </summary>
public sealed class LoadRegisterX : BaseInstruction
{
    #region Constructors
    /// <summary>
    /// Instantiates a new <see cref="LoadRegisterX"/>
    /// </summary>
    public LoadRegisterX()
        : base(
            0xA2,
            0xA6,
            0xB6,
            0xAE,
            0xBE)
    { }
    #endregion

    /// <inheritdoc/>
    public override void Execute(in ICpuState currentState, in ushort value)
    {
        var loadValue = Load(currentState, value);

        currentState.Flags.IsZero = loadValue.IsZero();
        currentState.Flags.IsNegative = loadValue.IsLastBitSet();

        currentState.Registers.IndexX = loadValue;
    }

    private static byte Load(ICpuState currentState, ushort address)
    {
        return currentState.ExecutingOpcode switch
        {
            0xA2 => (byte)address,
            0xA6 => currentState.Memory.ReadZeroPage(address),
            0xB6 => currentState.Memory.ReadZeroPageY(address),
            0xAE => currentState.Memory.ReadAbsolute(address),
            0xBE => LoadExtraCycle(currentState, currentState.Memory.ReadAbsoluteY(address)),
            _ => throw new UnknownOpcodeException(currentState.ExecutingOpcode),
        };
    }
}
