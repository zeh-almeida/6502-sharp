using Cpu.Extensions;
using Cpu.Instructions.Exceptions;
using Cpu.Opcodes;
using Cpu.States;

namespace Cpu.Instructions.Load
{
    /// <summary>
    /// <para>Load Accumulator Instruction (LDA)</para>
    /// <para>Loads a byte of memory into the accumulator setting the zero and negative flags as appropriate.</para>
    /// <para>
    /// Executes the following opcodes:
    /// <c>0xA9</c>,
    /// <c>0xA5</c>,
    /// <c>0xB5</c>,
    /// <c>0xAD</c>,
    /// <c>0xBD</c>,
    /// <c>0xB9</c>,
    /// <c>0xA1</c>,
    /// <c>0xB1</c>
    /// </para>
    /// </summary>
    public sealed class LoadAccumulator : BaseInstruction
    {
        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="LoadAccumulator"/>
        /// </summary>
        public LoadAccumulator()
            : base(
                new OpcodeInformation(0xA9, 2, 2),
                new OpcodeInformation(0xA5, 3, 2),
                new OpcodeInformation(0xB5, 4, 2),
                new OpcodeInformation(0xAD, 4, 3),
                new OpcodeInformation(0xBD, 5, 3),
                new OpcodeInformation(0xB9, 5, 3),
                new OpcodeInformation(0xA1, 6, 2),
                new OpcodeInformation(0xB1, 6, 2))
        { }
        #endregion

        /// <inheritdoc/>
        public override void Execute(ICpuState currentState, ushort value)
        {
            var loadValue = Load(currentState, value);

            currentState.Flags.IsZero = loadValue.IsZero();
            currentState.Flags.IsNegative = loadValue.IsLastBitSet();

            currentState.Registers.Accumulator = loadValue;
        }

        private static byte Load(ICpuState currentState, ushort address)
        {
            return currentState.ExecutingOpcode switch
            {
                0xA9 => (byte)address,
                0xA5 => currentState.Memory.ReadZeroPage(address),
                0xB5 => currentState.Memory.ReadZeroPageX(address),
                0xAD => currentState.Memory.ReadAbsolute(address),
                0xBD => currentState.Memory.ReadAbsoluteX(address),
                0xB9 => currentState.Memory.ReadAbsoluteY(address),
                0xA1 => currentState.Memory.ReadIndirectX(address),
                0xB1 => currentState.Memory.ReadIndirectY(address),
                _ => throw new UnknownOpcodeException(currentState.ExecutingOpcode),
            };
        }
    }
}
