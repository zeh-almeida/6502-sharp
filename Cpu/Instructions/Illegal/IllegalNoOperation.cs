using Cpu.Opcodes;
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
    /// <c>0x7A</c>
    /// <c>0xDA</c>,
    /// <c>0xFA</c>,
    /// <c>0x80</c>,
    /// <c>0x82</c>
    /// <c>0x89</c>,
    /// <c>0xC2</c>,
    /// <c>0xE2</c>,
    /// <c>0x04</c>
    /// <c>0x44</c>,
    /// <c>0x64</c>,
    /// <c>0x14</c>,
    /// <c>0x34</c>
    /// <c>0x54</c>,
    /// <c>0x74</c>,
    /// <c>0xD4</c>,
    /// <c>0xF4</c>
    /// <c>0x0C</c>,
    /// <c>0x1C</c>,
    /// <c>0x3C</c>,
    /// <c>0x5C</c>
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
                  new OpcodeInformation(0x1A, 2, 1),
                            new OpcodeInformation(0x3A, 2, 1),
                            new OpcodeInformation(0x5A, 2, 1),
                            new OpcodeInformation(0x7A, 2, 1),
                            new OpcodeInformation(0xDA, 2, 1),
                            new OpcodeInformation(0xFA, 2, 1),
                            new OpcodeInformation(0x80, 2, 2),
                            new OpcodeInformation(0x82, 2, 2),
                            new OpcodeInformation(0x89, 2, 2),
                            new OpcodeInformation(0xC2, 2, 2),
                            new OpcodeInformation(0xE2, 2, 2),
                            new OpcodeInformation(0x04, 3, 2),
                            new OpcodeInformation(0x44, 3, 2),
                            new OpcodeInformation(0x64, 3, 2),
                            new OpcodeInformation(0x14, 4, 2),
                            new OpcodeInformation(0x34, 4, 2),
                            new OpcodeInformation(0x54, 4, 2),
                            new OpcodeInformation(0x74, 4, 2),
                            new OpcodeInformation(0xD4, 4, 2),
                            new OpcodeInformation(0xF4, 4, 2),
                            new OpcodeInformation(0x0C, 4, 3),
                            new OpcodeInformation(0x1C, 5, 3),
                            new OpcodeInformation(0x3C, 5, 3),
                            new OpcodeInformation(0x5C, 5, 3),
                            new OpcodeInformation(0x7C, 5, 3),
                            new OpcodeInformation(0xDC, 5, 3),
                            new OpcodeInformation(0xFC, 5, 3))
        { }
        #endregion

        /// <inheritdoc/>
        public override ICpuState Execute(ICpuState currentState, ushort _)
        {
            return currentState;
        }
    }
}
