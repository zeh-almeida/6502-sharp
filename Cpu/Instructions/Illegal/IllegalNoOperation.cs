using Cpu.States;

namespace Cpu.Instructions.Illegal
{
    /// <summary>
    /// <para>Illegal No-Operation instruction (DOP/TOP/SKB/SKW)</para>
    /// <para>Illegal, executes a NO-OP</para>
    /// <para>In hardware they would perform reads in memory with different kinds of access</para>
    /// <para>In this implementation, they are standard NO_OP executions</para>
    /// <para>
    /// Executes the following opcodes:
    /// <c>0x1A</c>,
    /// <c>0x3A</c>,
    /// <c>0x5A</c>,
    /// <c>0x7A</c>,
    /// <c>0xDA</c>,
    /// <c>0xFA</c>,
    /// <c>0x80</c>,
    /// <c>0x82</c>,
    /// <c>0x89</c>,
    /// <c>0xC2</c>,
    /// <c>0xE2</c>,
    /// <c>0x04</c>,
    /// <c>0x44</c>,
    /// <c>0x64</c>,
    /// <c>0x14</c>,
    /// <c>0x34</c>,
    /// <c>0x54</c>,
    /// <c>0x74</c>,
    /// <c>0xD4</c>,
    /// <c>0xF4</c>,
    /// <c>0x0C</c>,
    /// <c>0x1C</c>,
    /// <c>0x3C</c>,
    /// <c>0x5C</c>,
    /// <c>0x7C</c>,
    /// <c>0xDC</c>,
    /// <c>0xFC</c>
    /// </para>
    /// </summary>
    /// <see href="https://masswerk.at/6502/6502_instruction_set.html#NOPs"/>
    /// <seealso cref="SystemFunctions.NoOperation"/>
    public sealed class IllegalNoOperation : BaseInstruction
    {
        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="IllegalNoOperation"/>
        /// </summary>
        public IllegalNoOperation()
            : base(
                0x1A,
                0x3A,
                0x5A,
                0x7A,
                0xDA,
                0xFA,
                0x80,
                0x82,
                0x89,
                0xC2,
                0xE2,
                0x04,
                0x44,
                0x64,
                0x14,
                0x34,
                0x54,
                0x74,
                0xD4,
                0xF4,
                0x0C,
                0x1C,
                0x3C,
                0x5C,
                0x7C,
                0xDC,
                0xFC)
        { }
        #endregion

        /// <inheritdoc/>
        public override void Execute(ICpuState currentState, ushort _)
        {
        }
    }
}
