using Cpu.Extensions;
using Cpu.Instructions.Exceptions;
using Cpu.Opcodes;
using Cpu.States;

namespace Cpu.Instructions.Logic
{
    /// <summary>
    /// <para>Exclusive OR instruction (EOR)</para>
    /// <para>An exclusive OR is performed, bit by bit, on the accumulator contents using the contents of a byte of memory.</para>
    /// <para>
    /// Executes the following opcodes:
    /// <c>0x49</c>,
    /// <c>0x45</c>,
    /// <c>0x55</c>,
    /// <c>0x4D</c>,
    /// <c>0x5D</c>,
    /// <c>0x59</c>,
    /// <c>0x41</c>,
    /// <c>0x51</c>
    /// </para>
    /// </summary>
    /// <see href="https://masswerk.at/6502/6502_instruction_set.html#EOR"/>
    public sealed class ExclusiveOr : BaseInstruction
    {
        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="ExclusiveOr"/>
        /// </summary>
        public ExclusiveOr()
            : base(
            new OpcodeInformation(0x49, 2, 2),
                      new OpcodeInformation(0x45, 3, 2),
                      new OpcodeInformation(0x55, 4, 2),
                      new OpcodeInformation(0x4D, 4, 3),
                      new OpcodeInformation(0x5D, 5, 3),
                      new OpcodeInformation(0x59, 5, 3),
                      new OpcodeInformation(0x41, 6, 2),
                      new OpcodeInformation(0x51, 6, 2))
        { }
        #endregion

        /// <inheritdoc/>
        public override void Execute(ICpuState currentState, ushort value)
        {
            var toCompare = ValueToCompare(currentState, value);
            var result = (byte)(toCompare ^ currentState.Registers.Accumulator);

            currentState.Flags.IsZero = result.IsZero();
            currentState.Flags.IsNegative = result.IsLastBitSet();

            currentState.Registers.Accumulator = result;
        }

        private static byte ValueToCompare(ICpuState currentState, ushort address)
        {
            return currentState.ExecutingOpcode switch
            {
                0x49 => (byte)address,
                0x45 => currentState.Memory.ReadZeroPage(address),
                0x55 => currentState.Memory.ReadZeroPageX(address),
                0x4D => currentState.Memory.ReadAbsolute(address),
                0x5D => currentState.Memory.ReadAbsoluteX(address),
                0x59 => currentState.Memory.ReadAbsoluteY(address),
                0x41 => currentState.Memory.ReadIndirectX(address),
                0x51 => currentState.Memory.ReadIndirectY(address),
                _ => throw new UnknownOpcodeException(currentState.ExecutingOpcode),
            };
        }
    }
}
