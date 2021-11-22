using Cpu.Extensions;
using Cpu.Instructions;
using Cpu.Instructions.Exceptions;
using Cpu.Opcodes;
using Cpu.States;
using System.Collections.Generic;
using System.Linq;

namespace Cpu.Execution
{
    /// <summary>
    /// Decodes an instruction based on a <see cref="ICpuState"/>.
    /// Implements <see cref="IDecoder"/>
    /// </summary>
    public sealed record Decoder : IDecoder
    {
        #region Properties
        private IEnumerable<IInstruction> Instructions { get; }
        #endregion

        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="IDecoder"/> with the instruction set
        /// </summary>
        /// <param name="instructions"><see cref="IInstruction"/> set to use for decoding</param>
        public Decoder(IEnumerable<IInstruction> instructions)
        {
            this.Instructions = instructions.ToArray();
        }
        #endregion

        /// <inheritdoc/>
        public DecodedInstruction Decode(ICpuState currentState)
        {
            var opcode = ReadNextOpcode(currentState);

            var instruction = this.FetchInstruction(opcode);
            var opcodeInfo = instruction.GatherInformation(opcode);

            var instructionValue = ReadOpcodeParameter(currentState, opcodeInfo);
            return new DecodedInstruction(opcodeInfo, instructionValue);
        }

        private IInstruction FetchInstruction(byte opcode)
        {
            var instruction = this.Instructions
                .FirstOrDefault(ins => ins.HasOpcode(opcode));

            return instruction ?? throw new UnknownOpcodeException(opcode);
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
