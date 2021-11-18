using Cpu.Extensions;
using Cpu.Instructions.Exceptions;
using Cpu.Opcodes;
using Cpu.States;

namespace Cpu.Instructions.Arithmetic
{
    /// <summary>
    /// <para>Add with Carry instruction (ADC)</para>
    /// <para>
    /// This instruction adds the contents of a memory location to the accumulator together with the carry bit.
    /// If overflow occurs the carry bit is set, this enables multiple byte addition to be performed.
    /// </para>
    /// <para>
    /// Executes the following opcodes:
    /// <c>0x69</c>,
    /// <c>0x65</c>,
    /// <c>0x75</c>,
    /// <c>0x6D</c>,
    /// <c>0x7D</c>,
    /// <c>0x79</c>,
    /// <c>0x61</c>,
    /// <c>0x71</c>
    /// </para>
    /// </summary>
    /// <see href="https://masswerk.at/6502/6502_instruction_set.html#ADC"/>
    public sealed class AddWithCarry : BaseInstruction
    {
        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="AddWithCarry"/> instruction
        /// </summary>
        public AddWithCarry()
            : base(
            new OpcodeInformation(0x69, 2, 2),
                      new OpcodeInformation(0x65, 3, 2),
                      new OpcodeInformation(0x75, 4, 2),
                      new OpcodeInformation(0x6D, 4, 3),
                      new OpcodeInformation(0x7D, 5, 3),
                      new OpcodeInformation(0x79, 5, 3),
                      new OpcodeInformation(0x61, 6, 2),
                      new OpcodeInformation(0x71, 6, 2))
        { }
        #endregion

        /// <inheritdoc/>
        public override ICpuState Execute(ICpuState currentState, ushort value)
        {
            var loadValue = Load(currentState, value);
            var accumulator = currentState.Registers.Accumulator;
            var carry = currentState.Flags.IsCarry ? 1 : 0;

            var operation = (ushort)(accumulator + loadValue + carry);

            var isNegative = operation.IsSeventhBitSet();
            var isOverflow = 0x80.Equals((~(accumulator ^ loadValue)) & (accumulator ^ operation) & 0x80);

            currentState.Flags.IsCarry = (operation.IsBitSet(8));
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
                0x69 => address,
                0x65 => currentState.Memory.ReadZeroPage(address),
                0x75 => currentState.Memory.ReadZeroPageX(address),
                0x6D => currentState.Memory.ReadAbsolute(address),
                0x7D => currentState.Memory.ReadAbsoluteX(address),
                0x79 => currentState.Memory.ReadAbsoluteY(address),
                0x61 => currentState.Memory.ReadIndirectX(address),
                0x71 => currentState.Memory.ReadIndirectY(address),
                _ => throw new UnknownOpcodeException(currentState.ExecutingOpcode),
            };
        }
    }
}
