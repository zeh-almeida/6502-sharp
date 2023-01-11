using Cpu.States;

namespace Cpu.Instructions.Illegal
{
    /// <summary>
    /// <para>Logical AND with Address High Byte and Register X instruction (SHX/SXA/XAS)</para>
    /// <para>Illegal and unstable, executes a logical AND with the Operand High Byte and X register
    /// then stores in the Operand address</para>
    /// <para>May sometimes drop off the High Byte or not cross the memory boundaries correctly, hence the unstable definition</para>
    /// <para>In this implementation, it doesn't suffer from instabilities</para>
    /// <para>
    /// Executes the following opcodes:
    /// <c>0x9E</c>
    /// </para>
    /// </summary>
    /// <see href="https://masswerk.at/6502/6502_instruction_set.html#SHX"/>
    /// <seealso cref="Logic.LogicAnd"/>
    /// <seealso cref="Store.StoreRegisterX"/>
    public sealed class AndXHighByte : BaseInstruction
    {
        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="AndXHighByte"/>
        /// </summary>
        public AndXHighByte()
            : base(0x9E)
        { }
        #endregion

        /// <inheritdoc/>
        public override void Execute(ICpuState currentState, ushort value)
        {
            var highByte = (byte)((byte)value + 1);
            var registerX = currentState.Registers.IndexX;

            var andValue = (byte)(registerX & highByte);

            currentState.Memory.WriteAbsoluteY(value, andValue);
        }
    }
}
