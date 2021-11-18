using System.Collections.Generic;
using Test.Integrated.Cpu.Common;
using Xunit;

namespace Test.Integrated.Cpu
{
    public sealed record StackLoadAccumulatorTest : IClassFixture<MachineFixture>
    {
        #region Properties
        private MachineFixture Fixture { get; }
        #endregion

        #region Constructors
        public StackLoadAccumulatorTest(MachineFixture fixture)
        {
            this.Fixture = fixture;
        }
        #endregion

        [Fact]
        public void Stacking_Accumulator_Computes()
        {
            const byte stackPointerValue = 3;
            const byte accumulatorValue = 8;

            const byte firstStackValue = 0x01;
            const byte secondStackValue = 0x05;
            const byte thirdStackValue = 0x08;

            const ushort stackPointerLocation = 2 + MachineFixture.RegisterOffset;
            const ushort accumulatorLocation = 3 + MachineFixture.RegisterOffset;

            const ushort firstStackLocation = 0x0200 + MachineFixture.MemoryStateOffset;
            const ushort secondStackLocation = 0x0201 + MachineFixture.MemoryStateOffset;
            const ushort thirdStackLocation = 0x0202 + MachineFixture.MemoryStateOffset;

            var programStream = BuildProgramStream();
            var finalState = this.Fixture.Compute(programStream);

            Assert.Equal(stackPointerValue, finalState[stackPointerLocation]);
            Assert.Equal(accumulatorValue, finalState[accumulatorLocation]);

            Assert.Equal(firstStackValue, finalState[firstStackLocation]);
            Assert.Equal(secondStackValue, finalState[secondStackLocation]);
            Assert.Equal(thirdStackValue, finalState[thirdStackLocation]);
        }

        private static IEnumerable<byte> BuildProgramStream()
        {
            var state = new byte[MachineFixture.LoadDataLength];

            state[0x0000 + MachineFixture.RegisterOffset] = 0b_0000_0000;
            state[0x0001 + MachineFixture.RegisterOffset] = 0b_0000_0110;

            state[0x0600 + MachineFixture.MemoryStateOffset] = 0xA9;
            state[0x0601 + MachineFixture.MemoryStateOffset] = 0x01;
            state[0x0602 + MachineFixture.MemoryStateOffset] = 0x8D;
            state[0x0603 + MachineFixture.MemoryStateOffset] = 0x00;
            state[0x0604 + MachineFixture.MemoryStateOffset] = 0x02;
            state[0x0605 + MachineFixture.MemoryStateOffset] = 0xA9;
            state[0x0606 + MachineFixture.MemoryStateOffset] = 0x05;
            state[0x0607 + MachineFixture.MemoryStateOffset] = 0x8D;
            state[0x0608 + MachineFixture.MemoryStateOffset] = 0x01;
            state[0x0609 + MachineFixture.MemoryStateOffset] = 0x02;
            state[0x060A + MachineFixture.MemoryStateOffset] = 0xA9;
            state[0x060B + MachineFixture.MemoryStateOffset] = 0x08;
            state[0x060C + MachineFixture.MemoryStateOffset] = 0x8D;
            state[0x060D + MachineFixture.MemoryStateOffset] = 0x02;
            state[0x060E + MachineFixture.MemoryStateOffset] = 0x02;
            state[0xFFFE + MachineFixture.MemoryStateOffset] = 0xFF;
            state[0xFFFF + MachineFixture.MemoryStateOffset] = 0xFF;

            return state;
        }
    }
}
