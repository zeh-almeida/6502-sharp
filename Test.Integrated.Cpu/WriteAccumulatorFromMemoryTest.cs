using Cpu.States;
using System.Collections.Generic;
using Test.Integrated.Cpu.Common;
using Xunit;

namespace Test.Integrated.Cpu
{
    public sealed record WriteAccumulatorFromMemoryTest : IClassFixture<MachineFixture>
    {
        #region Properties
        private MachineFixture Fixture { get; }
        #endregion

        #region Constructors
        public WriteAccumulatorFromMemoryTest(MachineFixture fixture)
        {
            this.Fixture = fixture;
        }
        #endregion

        [Fact]
        public void IncrementAccumulator_FromMemory_Computes()
        {
            const byte yValue = 0x04;
            const byte accValue = 0x00;

            const byte firstValue = 0x06;
            const byte secondValue = 0x05;
            const byte thirdValue = 0x01;
            const byte fourthValue = 0x02;
            const byte fifthValue = 0x00;

            const ushort yLocation = 5 + ICpuState.RegisterOffset;
            const ushort accLocation = 3 + ICpuState.RegisterOffset;

            var opcodeCount = 0;

            var programStream = BuildProgramStream();
            var finalState = this.Fixture.Compute(programStream, cpuState =>
            {
                if (0.Equals(cpuState.CyclesLeft))
                {
                    opcodeCount++;
                    var value = cpuState.Registers.Accumulator;

                    switch (opcodeCount)
                    {
                        case 4:
                            Assert.Equal(firstValue, value);
                            break;

                        case 9:
                            Assert.Equal(secondValue, value);
                            break;

                        case 14:
                            Assert.Equal(thirdValue, value);
                            break;

                        case 19:
                            Assert.Equal(fourthValue, value);
                            break;

                        case 24:
                            Assert.Equal(fifthValue, value);
                            break;
                    }
                }
            });

            Assert.Equal(yValue, finalState[yLocation]);
            Assert.Equal(accValue, finalState[accLocation]);
        }

        private static IEnumerable<byte> BuildProgramStream()
        {
            var state = new byte[ICpuState.Length];

            state[0x0000 + ICpuState.RegisterOffset] = 0b_0000_0000;
            state[0x0001 + ICpuState.RegisterOffset] = 0b_0000_0110;

            state[0x0600 + ICpuState.MemoryStateOffset] = 0xA0;
            state[0x0601 + ICpuState.MemoryStateOffset] = 0x00;
            state[0x0602 + ICpuState.MemoryStateOffset] = 0xB9;
            state[0x0603 + ICpuState.MemoryStateOffset] = 0x0E;
            state[0x0604 + ICpuState.MemoryStateOffset] = 0x06;
            state[0x0605 + ICpuState.MemoryStateOffset] = 0xF0;
            state[0x0606 + ICpuState.MemoryStateOffset] = 0x06;
            state[0x0607 + ICpuState.MemoryStateOffset] = 0x99;
            state[0x0608 + ICpuState.MemoryStateOffset] = 0x00;
            state[0x0609 + ICpuState.MemoryStateOffset] = 0xF0;
            state[0x060A + ICpuState.MemoryStateOffset] = 0xC8;
            state[0x060B + ICpuState.MemoryStateOffset] = 0xD0;
            state[0x060C + ICpuState.MemoryStateOffset] = 0xF5;
            state[0x060D + ICpuState.MemoryStateOffset] = 0x00;
            state[0x060E + ICpuState.MemoryStateOffset] = 0x06;
            state[0x060F + ICpuState.MemoryStateOffset] = 0x05;
            state[0x0610 + ICpuState.MemoryStateOffset] = 0x01;
            state[0x0611 + ICpuState.MemoryStateOffset] = 0x02;
            state[0x0612 + ICpuState.MemoryStateOffset] = 0x00;

            state[0xFFFE + ICpuState.MemoryStateOffset] = 0xFF;
            state[0xFFFF + ICpuState.MemoryStateOffset] = 0xFF;

            return state;
        }
    }
}
