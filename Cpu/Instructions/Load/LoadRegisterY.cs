using Cpu.Extensions;
using Cpu.Instructions.Exceptions;
using Cpu.Opcodes;
using Cpu.States;

namespace Cpu.Instructions.Load
{
    /// <summary>
    /// <para>Load Y Register Instruction (LDY)</para>
    /// <para>Loads a byte of memory into the Y register setting the zero and negative flags as appropriate.</para>
    /// <para>
    /// Executes the following opcodes:
    /// <c>0xA0</c>,
    /// <c>0xA4</c>,
    /// <c>0xB4</c>,
    /// <c>0xAC</c>,
    /// <c>0xBC</c>
    /// </para>
    /// </summary>
    public sealed class LoadRegisterY : BaseInstruction
    {
        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="LoadRegisterY"/>
        /// </summary>
        public LoadRegisterY()
            : base(
            new OpcodeInformation(0xA0, 2, 2),
                      new OpcodeInformation(0xA4, 3, 2),
                      new OpcodeInformation(0xB4, 4, 2),
                      new OpcodeInformation(0xAC, 4, 3),
                      new OpcodeInformation(0xBC, 5, 3))
        { }
        #endregion

        /// <inheritdoc/>
        public override void Execute(ICpuState currentState, ushort value)
        {
            var loadValue = Load(currentState, value);

            currentState.Flags.IsZero = loadValue.IsZero();
            currentState.Flags.IsNegative = loadValue.IsLastBitSet();

            currentState.Registers.IndexY = loadValue;
        }

        private static byte Load(ICpuState currentState, ushort address)
        {
            return currentState.ExecutingOpcode switch
            {
                0xA0 => (byte)address,
                0xA4 => currentState.Memory.ReadZeroPage(address),
                0xB4 => currentState.Memory.ReadZeroPageX(address),
                0xAC => currentState.Memory.ReadAbsolute(address),
                0xBC => currentState.Memory.ReadAbsoluteX(address),
                _ => throw new UnknownOpcodeException(currentState.ExecutingOpcode),
            };
        }
    }
}
