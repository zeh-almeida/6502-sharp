using Cpu.Extensions;
using Cpu.Instructions.Exceptions;
using Cpu.States;

namespace Cpu.Instructions.Logic
{
    /// <summary>
    /// <para>Bit Test instruction (BIT)</para>
    /// <para>
    /// This instructions is used to test if one or more bits are set in a target memory location.
    /// The mask pattern in A is ANDed with the value in memory to set or clear the zero flag, but the result is not kept.
    /// Bits 7 and 6 of the value from memory are copied into the N and V flags.
    /// </para>
    /// <para>
    /// Executes the following opcodes:
    /// <c>0x24</c>,
    /// <c>0x2C</c>
    /// </para>
    /// </summary>
    /// <see href="https://masswerk.at/6502/6502_instruction_set.html#BIT"/>
    public sealed class BitTest : BaseInstruction
    {
        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="BitTest"/>
        /// </summary>
        public BitTest()
            : base(
                0x24,
                0x2C)
        { }
        #endregion

        /// <inheritdoc/>
        public override void Execute(ICpuState currentState, ushort value)
        {
            var toCompare = ValueToCompare(currentState, value);
            var result = (ushort)(toCompare & currentState.Registers.Accumulator);

            currentState.Flags.IsZero = result.IsZero();
            currentState.Flags.IsOverflow = toCompare.IsBitSet(6);
            currentState.Flags.IsNegative = toCompare.IsSeventhBitSet();
        }

        private static ushort ValueToCompare(ICpuState currentState, ushort address)
        {
            return currentState.ExecutingOpcode switch
            {
                0x24 => currentState.Memory.ReadZeroPage(address),
                0x2C => address,
                _ => throw new UnknownOpcodeException(currentState.ExecutingOpcode),
            };
        }
    }
}
