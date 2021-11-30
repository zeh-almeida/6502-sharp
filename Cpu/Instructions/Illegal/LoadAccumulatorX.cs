using Cpu.Extensions;
using Cpu.Instructions.Exceptions;
using Cpu.Opcodes;
using Cpu.States;

namespace Cpu.Instructions.Illegal
{
    /// <summary>
    /// <para>Load into Accumulator and X Register instruction (LAX)</para>
    /// <para>Illegal, loads memory value into both accumulator and X Register</para>
    /// <para>
    /// Executes the following opcodes:
    /// <c>0xA7</c>,
    /// <c>0xB7</c>,
    /// <c>0xAF</c>,
    /// <c>0xBF</c>,
    /// <c>0xA3</c>,
    /// <c>0xB3</c>
    /// </para>
    /// </summary>
    /// <see href="https://masswerk.at/6502/6502_instruction_set.html#LAX"/>
    /// <seealso cref="Load.LoadAccumulator"/>
    /// <seealso cref="Load.LoadRegisterX"/>
    public sealed class LoadAccumulatorX : BaseInstruction
    {
        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="LoadAccumulatorX"/>
        /// </summary>
        public LoadAccumulatorX()
            : base(
                  new OpcodeInformation(0xA7, 3, 2),
                            new OpcodeInformation(0xB7, 4, 2),
                            new OpcodeInformation(0xAF, 6, 2),
                            new OpcodeInformation(0xBF, 6, 2),
                            new OpcodeInformation(0xA3, 4, 3),
                            new OpcodeInformation(0xB3, 4, 3))
        { }
        #endregion

        /// <inheritdoc/>
        public override void Execute(ICpuState currentState, ushort value)
        {
            var loadValue = Load(currentState, value);

            currentState.Flags.IsZero = (loadValue.IsZero());
            currentState.Flags.IsNegative = (loadValue.IsLastBitSet());

            currentState.Registers.Accumulator = loadValue;
            currentState.Registers.IndexX = loadValue;
        }

        private static byte Load(ICpuState currentState, ushort address)
        {
            return currentState.ExecutingOpcode switch
            {
                0xA7 => currentState.Memory.ReadZeroPage(address),
                0xB7 => currentState.Memory.ReadZeroPageY(address),
                0xAF => currentState.Memory.ReadAbsolute(address),
                0xBF => currentState.Memory.ReadAbsoluteY(address),
                0xA3 => currentState.Memory.ReadIndirectX(address),
                0xB3 => currentState.Memory.ReadIndirectY(address),
                _ => throw new UnknownOpcodeException(currentState.ExecutingOpcode),
            };
        }
    }
}
