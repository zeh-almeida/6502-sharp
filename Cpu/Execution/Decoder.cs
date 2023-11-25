using CommunityToolkit.Diagnostics;
using Cpu.Extensions;
using Cpu.Instructions;
using Cpu.Instructions.Exceptions;
using Cpu.Opcodes;
using Cpu.States;

namespace Cpu.Execution;

/// <summary>
/// Decodes an instruction based on a <see cref="ICpuState"/>.
/// Implements <see cref="IDecoder"/>
/// </summary>
public sealed record Decoder : IDecoder
{
    #region Properties
    private HashSet<IOpcodeInformation> Opcodes { get; }

    private HashSet<IInstruction> Instructions { get; }
    #endregion

    #region Constructors
    /// <summary>
    /// Instantiates a new <see cref="IDecoder"/> with the instruction set
    /// </summary>
    /// <param name="opcodes"><see cref="IOpcodeInformation"/> enumeration for instruction metadata</param>
    /// <param name="instructions"><see cref="IInstruction"/> enumeration for instruction executors</param>
    public Decoder(
        IEnumerable<IOpcodeInformation> opcodes,
        IEnumerable<IInstruction> instructions)
    {
        Guard.IsNotNull(opcodes);
        Guard.IsNotNull(instructions);

        this.Opcodes = opcodes.ToHashSet();
        this.Instructions = instructions.ToHashSet();
    }
    #endregion

    /// <inheritdoc/>
    public DecodedInstruction Decode(in ICpuState currentState)
    {
        Guard.IsNotNull(currentState);

        var opcode = ReadNextOpcode(currentState);
        var opcodeInfo = this.FetchOpcode(opcode);
        var instruction = this.FetchInstruction(opcode);

        var instructionValue = ReadOpcodeParameter(currentState, opcodeInfo);
        return new DecodedInstruction(opcodeInfo, instruction, instructionValue);
    }

    private IOpcodeInformation FetchOpcode(byte opcode)
    {
        var result = this.Opcodes
            .FirstOrDefault(item => opcode.Equals(item.Opcode));

        return result
            ?? throw new UnknownOpcodeException(opcode);
    }

    private IInstruction FetchInstruction(byte opcode)
    {
        var result = this.Instructions
            .FirstOrDefault(item => item.HasOpcode(opcode));

        return result
            ?? throw new UnknownOpcodeException(opcode);
    }

    private static ushort ReadOpcodeParameter(in ICpuState currentState, in IOpcodeInformation opcodeInfo)
    {
        var pc = currentState.Registers.ProgramCounter;

        if (opcodeInfo.Bytes <= 1)
        {
            return 0;
        }
        else if (2.Equals(opcodeInfo.Bytes))
        {
            return currentState.Memory.ReadAbsolute((ushort)(pc + 1));
        }
        else
        {
            var lsb = currentState.Memory.ReadAbsolute((ushort)(pc + 1));
            var msb = currentState.Memory.ReadAbsolute((ushort)(pc + 2));

            return lsb.CombineBytes(msb);
        }
    }

    private static byte ReadNextOpcode(in ICpuState currentState)
    {
        return currentState.Memory.ReadAbsolute(currentState.Registers.ProgramCounter);
    }
}
