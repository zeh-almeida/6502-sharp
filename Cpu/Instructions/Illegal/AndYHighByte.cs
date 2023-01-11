using Cpu.States;

namespace Cpu.Instructions.Illegal
{
    /// <summary>
    /// <para>Logical AND with Address High Byte and Register Y instruction (SHY/SYA/SAY)</para>
    /// <para>Illegal and unstable, executes a logical AND with the Operand High Byte and Y register
    /// then stores in the Operand address</para>
    /// <para>May sometimes drop off the High Byte or not cross the memory boundaries correctly, hence the unstable definition</para>
    /// <para>In this implementation, it doesn't suffer from instabilities</para>
    /// <para>
    /// Executes the following opcodes:
    /// <c>0x9C</c>
    /// </para>
    /// </summary>
    /// <see href="https://masswerk.at/6502/6502_instruction_set.html#SHY"/>
    /// <seealso cref="Logic.LogicAnd"/>
    /// <seealso cref="Store.StoreRegisterY"/>
    public sealed class AndYHighByte : BaseInstruction
    {
        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="AndYHighByte"/>
        /// </summary>
        public AndYHighByte()
            : base(0x9C)
        { }
        #endregion

        /// <inheritdoc/>
        public override void Execute(ICpuState currentState, ushort value)
        {
            var highByte = (byte)((byte)value + 1);
            var registerY = currentState.Registers.IndexY;

            var andValue = (byte)(registerY & highByte);

            currentState.Memory.WriteAbsoluteX(value, andValue);
        }
    }
}
