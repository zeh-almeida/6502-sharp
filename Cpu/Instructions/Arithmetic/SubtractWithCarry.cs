using Cpu.Extensions;
using Cpu.Instructions.Exceptions;
using Cpu.Opcodes;
using Cpu.States;

namespace Cpu.Instructions.Arithmetic
{
    /// <summary>
    /// <para>Subtract Carry instruction (SBC)</para>
    /// <para>
    /// This instruction subtracts the contents of a memory location to the accumulator together with the not of the carry bit.
    /// If overflow occurs the carry bit is clear, this enables multiple byte subtraction to be performed.
    /// </para>
    /// <para>
    /// Executes the following opcodes:
    /// <c>0xE9</c>,
    /// <c>0xE5</c>,
    /// <c>0xF5</c>,
    /// <c>0xED</c>,
    /// <c>0xFD</c>,
    /// <c>0xF9</c>,
    /// <c>0xE1</c>,
    /// <c>0xF1</c>
    /// </para>
    /// </summary>
    /// <see href="https://masswerk.at/6502/6502_instruction_set.html#SBC"/>
    public sealed class SubtractWithCarry : BaseInstruction
    {
        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="SubtractWithCarry"/> instruction
        /// </summary>
        public SubtractWithCarry()
            : base(
            new OpcodeInformation(0xE9, 2, 2),
                      new OpcodeInformation(0xE5, 3, 2),
                      new OpcodeInformation(0xF5, 4, 2),
                      new OpcodeInformation(0xED, 4, 3),
                      new OpcodeInformation(0xFD, 5, 3),
                      new OpcodeInformation(0xF9, 5, 3),
                      new OpcodeInformation(0xE1, 6, 2),
                      new OpcodeInformation(0xF1, 6, 2))
        { }
        #endregion

        /// <inheritdoc/>
        public override ICpuState Execute(ICpuState currentState, ushort value)
        {
            var loadValue = Load(currentState, value);
            var accumulator = currentState.Registers.Accumulator;
            var carry = currentState.Flags.IsCarry ? 1 : 0;

            var twoComplement = (byte)(~loadValue + carry);
            var operation = (ushort)(accumulator + twoComplement);

            var isNegative = operation.IsSeventhBitSet();
            var isOverflow = operation.IsBitSet(8);

            currentState.Flags.IsCarry = ((sbyte)operation >= 0);
            currentState.Flags.IsOverflow = isOverflow;

            currentState.Flags.IsZero = (operation.IsZero());
            currentState.Flags.IsNegative = isNegative;

            currentState.Registers.Accumulator = ((byte)operation);
            return currentState;
        }

        private static ushort Load(ICpuState currentState, ushort address)
        {
            return currentState.ExecutingOpcode switch
            {
                0xE9 => address,
                0xE5 => currentState.Memory.ReadZeroPage(address),
                0xF5 => currentState.Memory.ReadZeroPageX(address),
                0xED => currentState.Memory.ReadAbsolute(address),
                0xFD => currentState.Memory.ReadAbsoluteX(address),
                0xF9 => currentState.Memory.ReadAbsoluteY(address),
                0xE1 => currentState.Memory.ReadIndirectX(address),
                0xF1 => currentState.Memory.ReadIndirectY(address),
                _ => throw new UnknownOpcodeException(currentState.ExecutingOpcode),
            };
        }
    }
}
