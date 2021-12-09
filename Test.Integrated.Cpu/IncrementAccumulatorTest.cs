using Cpu.States;
using System.Collections.Generic;
using Test.Integrated.Cpu.Common;
using Xunit;

namespace Test.Integrated.Cpu
{
    public sealed record IncrementAccumulatorTest : IClassFixture<MachineFixture>
    {
        #region Properties
        private MachineFixture Fixture { get; }
        #endregion

        #region Constructors
        public IncrementAccumulatorTest(MachineFixture fixture)
        {
            this.Fixture = fixture;
        }
        #endregion

        [Fact]
        public void Incrementing_Accumulator_Computes()
        {
            const byte xValue = 193;
            const byte accumulatorValue = 132;

            const ushort xLocation = 4 + ICpuState.RegisterOffset;
            const ushort accumulatorLocation = 3 + ICpuState.RegisterOffset;

            var programStream = BuildProgramStream();
            var finalState = this.Fixture.Compute(programStream);

            Assert.Equal(xValue, finalState[xLocation]);
            Assert.Equal(accumulatorValue, finalState[accumulatorLocation]);
        }

        private static IEnumerable<byte> BuildProgramStream()
        {
            var state = new byte[ICpuState.Length];

            state[0x0000 + ICpuState.RegisterOffset] = 0b_0000_0000;
            state[0x0001 + ICpuState.RegisterOffset] = 0b_0000_0110;

            state[0x0600 + ICpuState.MemoryStateOffset] = 0xA9;
            state[0x0601 + ICpuState.MemoryStateOffset] = 0xC0;
            state[0x0602 + ICpuState.MemoryStateOffset] = 0xAA;
            state[0x0603 + ICpuState.MemoryStateOffset] = 0xE8;
            state[0x0604 + ICpuState.MemoryStateOffset] = 0x69;
            state[0x0605 + ICpuState.MemoryStateOffset] = 0xC4;
            state[0xFFFE + ICpuState.MemoryStateOffset] = 0xFF;
            state[0xFFFF + ICpuState.MemoryStateOffset] = 0xFF;

            return state;
        }
    }
}
