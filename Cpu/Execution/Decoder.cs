using Cpu.Extensions;
using Cpu.Instructions;
using Cpu.Instructions.Exceptions;
using Cpu.Opcodes;
using Cpu.States;
using System.Diagnostics;

namespace Cpu.Execution
{
    /// <summary>
    /// Decodes an instruction based on a <see cref="ICpuState"/>.
    /// Implements <see cref="IDecoder"/>
    /// </summary>
    public sealed record Decoder : IDecoder
    {
        #region Properties
        private IEnumerable<OpcodeInformation> Opcodes { get; }
        #endregion

        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="IDecoder"/> with the instruction set
        /// </summary>
        /// <param name="opcodes"><see cref="OpcodeInformation"/> enumeration for instruction metadata</param>
        public Decoder(IEnumerable<OpcodeInformation> opcodes)
        {
            this.Opcodes = opcodes.ToHashSet();
        }
        #endregion

        /// <inheritdoc/>
        public DecodedInstruction Decode(ICpuState currentState)
        {
            var opcode = ReadNextOpcode(currentState);

            var instruction = this.FetchInstruction(opcode);
            var opcodeInfo = instruction.GatherInformation(opcode);

            var instructionValue = ReadOpcodeParameter(currentState, opcodeInfo);
            var result = new DecodedInstruction(opcodeInfo, instructionValue);

            Debug.WriteLine($"{result} @ {currentState.Registers.ProgramCounter.AsHex()}");
            return result;
        }

        private IInstruction FetchInstruction(byte opcode)
        {
            var opcodeData = this.Opcodes
                .FirstOrDefault(ins => opcode.Equals(ins.Opcode));

            return opcodeData?.Instruction
                ?? throw new UnknownOpcodeException(opcode);
        }

        private static ushort ReadOpcodeParameter(ICpuState currentState, OpcodeInformation opcodeInfo)
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

        private static byte ReadNextOpcode(ICpuState currentState)
        {
            return currentState.Memory.ReadAbsolute(currentState.Registers.ProgramCounter);
        }
    }
}
