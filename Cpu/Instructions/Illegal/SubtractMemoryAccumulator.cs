using Cpu.Extensions;
using Cpu.Instructions.Exceptions;
using Cpu.Opcodes;
using Cpu.States;

namespace Cpu.Instructions.Illegal
{
    /// <summary>
    /// <para>Increment Memory and Subtract Accumulator instruction (ISC/ISB/INS)</para>
    /// <para>Illegal, increments memory then decrements it from accumulator</para>
    /// </summary>
    /// <see href="https://masswerk.at/6502/6502_instruction_set.html#DCP"/>
    /// <seealso cref="Increments.IncrementMemory"/>
    /// <seealso cref="Arithmetic.SubtractWithCarry"/>
    public sealed class SubtractMemoryAccumulator : BaseInstruction
    {
        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="SubtractMemoryAccumulator"/>
        /// </summary>
        public SubtractMemoryAccumulator()
            : base(
                  new OpcodeInformation(0xE7, 5, 2),
                            new OpcodeInformation(0xF7, 6, 2),
                            new OpcodeInformation(0xEF, 6, 3),
                            new OpcodeInformation(0xFF, 7, 3),
                            new OpcodeInformation(0xFB, 7, 3),
                            new OpcodeInformation(0xE3, 8, 2),
                            new OpcodeInformation(0xF3, 4, 2))
        { }
        #endregion

        /// <inheritdoc/>
        public override void Execute(ICpuState currentState, ushort value)
        {
            var accumulator = currentState.Registers.Accumulator;
            var loadValue = Load(currentState, value);
            var carry = currentState.Flags.IsCarry ? 1 : 0;

            var memoryValue = (byte)(loadValue + 1);
            var twoComplement = (byte)(~memoryValue + carry);
            var operation = (ushort)(accumulator + twoComplement);

            var isNegative = operation.IsSeventhBitSet();
            var isOverflow = operation.IsBitSet(8);

            currentState.Flags.IsCarry = (sbyte)operation >= 0;
            currentState.Flags.IsOverflow = isOverflow;

            currentState.Flags.IsZero = operation.IsZero();
            currentState.Flags.IsNegative = isNegative;

            currentState.Registers.Accumulator = ((byte)operation);
        }

        private static byte Load(ICpuState currentState, ushort address)
        {
            return currentState.ExecutingOpcode switch
            {
                0xE7 => currentState.Memory.ReadZeroPage(address),
                0xF7 => currentState.Memory.ReadZeroPageX(address),
                0xEF => currentState.Memory.ReadAbsolute(address),
                0xFF => currentState.Memory.ReadAbsoluteX(address),
                0xFB => currentState.Memory.ReadAbsoluteY(address),
                0xE3 => currentState.Memory.ReadIndirectX(address),
                0xF3 => currentState.Memory.ReadIndirectY(address),
                _ => throw new UnknownOpcodeException(currentState.ExecutingOpcode),
            };
        }
    }
}
