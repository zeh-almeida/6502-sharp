using System.Collections.Generic;
using Test.Integrated.Cpu.Common;
using Xunit;

namespace Test.Integrated.Cpu
{
    /// <summary>
    /// <see href="http://skilldrick.github.io/easy6502/#jumping"/>
    /// </summary>
    public sealed record JumpTest : IClassFixture<MachineFixture>
    {
        #region Properties
        private MachineFixture Fixture { get; }
        #endregion

        #region Constructors
        public JumpTest(MachineFixture fixture)
        {
            this.Fixture = fixture;
        }
        #endregion

        [Fact]
        public void Executes()
        {
            const byte value = 0x03;

            const ushort accumulatorLocation = 3 + MachineFixture.RegisterOffset;
            const ushort memoryLocation = 0x0200 + MachineFixture.MemoryStateOffset;

            var programStream = BuildProgramStream();
            var finalState = this.Fixture.Compute(programStream);

            Assert.Equal(value, finalState[accumulatorLocation]);
            Assert.Equal(value, finalState[memoryLocation]);
        }

        private static IEnumerable<byte> BuildProgramStream()
        {
            var state = new byte[MachineFixture.LoadDataLength];

            state[0x0000 + MachineFixture.RegisterOffset] = 0b_0000_0000;
            state[0x0001 + MachineFixture.RegisterOffset] = 0b_0000_0110;

            state[0x0600 + MachineFixture.MemoryStateOffset] = 0xA9;
            state[0x0601 + MachineFixture.MemoryStateOffset] = 0x03;
            state[0x0602 + MachineFixture.MemoryStateOffset] = 0x4C;
            state[0x0603 + MachineFixture.MemoryStateOffset] = 0x08;
            state[0x0604 + MachineFixture.MemoryStateOffset] = 0x06;
            state[0x0605 + MachineFixture.MemoryStateOffset] = 0x00;
            state[0x0606 + MachineFixture.MemoryStateOffset] = 0x00;
            state[0x0607 + MachineFixture.MemoryStateOffset] = 0x00;
            state[0x0608 + MachineFixture.MemoryStateOffset] = 0x8D;
            state[0x0609 + MachineFixture.MemoryStateOffset] = 0x00;
            state[0x060A + MachineFixture.MemoryStateOffset] = 0x02;

            state[0xFFFE + MachineFixture.MemoryStateOffset] = 0xFF;
            state[0xFFFF + MachineFixture.MemoryStateOffset] = 0xFF;

            return state;
        }
    }
}
