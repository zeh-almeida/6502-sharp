using Cpu.Extensions;
using Cpu.Instructions.Exceptions;
using Cpu.Opcodes;
using Cpu.States;

namespace Cpu.Instructions.Logic
{
    /// <summary>
    /// <para>Logical Inclusive OR instruction (ORA)</para>
    /// <para>An inclusive OR is performed, bit by bit, on the accumulator contents using the contents of a byte of memory.</para>
    /// <para>
    /// Executes the following opcodes:
    /// <c>0x09</c>,
    /// <c>0x05</c>,
    /// <c>0x15</c>,
    /// <c>0x0D</c>,
    /// <c>0x1D</c>,
    /// <c>0x19</c>,
    /// <c>0x01</c>,
    /// <c>0x11</c>
    /// </para>
    /// </summary>
    /// <see href="https://masswerk.at/6502/6502_instruction_set.html#ORA"/>
    public sealed class InclusiveOr : BaseInstruction
    {
        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="InclusiveOr"/>
        /// </summary>
        public InclusiveOr()
            : base(
            new OpcodeInformation(0x09, 2, 2),
                      new OpcodeInformation(0x05, 3, 2),
                      new OpcodeInformation(0x15, 4, 2),
                      new OpcodeInformation(0x0D, 4, 3),
                      new OpcodeInformation(0x1D, 5, 3),
                      new OpcodeInformation(0x19, 5, 3),
                      new OpcodeInformation(0x01, 6, 2),
                      new OpcodeInformation(0x11, 6, 2))
        { }
        #endregion

        /// <inheritdoc/>
        public override void Execute(ICpuState currentState, ushort value)
        {
            var toCompare = ValueToCompare(currentState, value);
            var result = (byte)(toCompare | currentState.Registers.Accumulator);

            currentState.Flags.IsZero = result.IsZero();
            currentState.Flags.IsNegative = result.IsLastBitSet();

            currentState.Registers.Accumulator = result;
        }

        private static byte ValueToCompare(ICpuState currentState, ushort address)
        {
            return currentState.ExecutingOpcode switch
            {
                0x09 => (byte)address,
                0x05 => currentState.Memory.ReadZeroPage(address),
                0x15 => currentState.Memory.ReadZeroPageX(address),
                0x0D => currentState.Memory.ReadAbsolute(address),
                0x1D => currentState.Memory.ReadAbsoluteX(address),
                0x19 => currentState.Memory.ReadAbsoluteY(address),
                0x01 => currentState.Memory.ReadIndirectX(address),
                0x11 => currentState.Memory.ReadIndirectY(address),
                _ => throw new UnknownOpcodeException(currentState.ExecutingOpcode),
            };
        }
    }
}
