using System.Collections.Generic;
using Test.Integrated.Cpu.Common;
using Xunit;

namespace Test.Integrated.Cpu
{
    /// <summary>
    /// <see href="http://skilldrick.github.io/easy6502/#jumping"/>
    /// </summary>
    public sealed record SubroutineTest : IClassFixture<MachineFixture>
    {
        #region Properties
        private MachineFixture Fixture { get; }
        #endregion

        #region Constructors
        public SubroutineTest(MachineFixture fixture)
        {
            this.Fixture = fixture;
        }
        #endregion

        [Fact]
        public void Executes()
        {
            const byte value = 0x05;

            const ushort xLocation = 4 + MachineFixture.RegisterOffset;

            var programStream = BuildProgramStream();
            var finalState = this.Fixture.Compute(programStream);

            Assert.Equal(value, finalState[xLocation]);
        }

        private static IEnumerable<byte> BuildProgramStream()
        {
            var state = new byte[MachineFixture.LoadDataLength];

            state[0x0001 + MachineFixture.RegisterOffset] = 0x06;

            state[0x0600 + MachineFixture.MemoryStateOffset] = 0x20;
            state[0x0601 + MachineFixture.MemoryStateOffset] = 0x09;
            state[0x0602 + MachineFixture.MemoryStateOffset] = 0x06;
            state[0x0603 + MachineFixture.MemoryStateOffset] = 0x20;
            state[0x0604 + MachineFixture.MemoryStateOffset] = 0x0C;
            state[0x0605 + MachineFixture.MemoryStateOffset] = 0x06;
            state[0x0606 + MachineFixture.MemoryStateOffset] = 0x20;
            state[0x0607 + MachineFixture.MemoryStateOffset] = 0x12;
            state[0x0608 + MachineFixture.MemoryStateOffset] = 0x06;
            state[0x0609 + MachineFixture.MemoryStateOffset] = 0xA2;
            state[0x060A + MachineFixture.MemoryStateOffset] = 0x00;
            state[0x060B + MachineFixture.MemoryStateOffset] = 0x60;
            state[0x060C + MachineFixture.MemoryStateOffset] = 0xE8;
            state[0x060D + MachineFixture.MemoryStateOffset] = 0xE0;
            state[0x060E + MachineFixture.MemoryStateOffset] = 0x05;
            state[0x060F + MachineFixture.MemoryStateOffset] = 0xD0;
            state[0x0610 + MachineFixture.MemoryStateOffset] = 0xFB;
            state[0x0611 + MachineFixture.MemoryStateOffset] = 0x60;
            state[0x0612 + MachineFixture.MemoryStateOffset] = 0x00;

            state[0xFFFE + MachineFixture.MemoryStateOffset] = 0xFF;
            state[0xFFFF + MachineFixture.MemoryStateOffset] = 0xFF;

            return state;
        }
    }
}
