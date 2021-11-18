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

            const ushort yLocation = 5 + MachineFixture.RegisterOffset;
            const ushort accLocation = 3 + MachineFixture.RegisterOffset;

            var opcodeCount = 0;

            var programStream = BuildProgramStream();
            var finalState = this.Fixture.Compute(programStream, cpuState =>
            {
                if (0.Equals(this.Fixture.Subject.CyclesLeft))
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
            var state = new byte[MachineFixture.LoadDataLength];

            state[0x0000 + MachineFixture.RegisterOffset] = 0b_0000_0000;
            state[0x0001 + MachineFixture.RegisterOffset] = 0b_0000_0110;

            state[0x0600 + MachineFixture.MemoryStateOffset] = 0xA0;
            state[0x0601 + MachineFixture.MemoryStateOffset] = 0x00;
            state[0x0602 + MachineFixture.MemoryStateOffset] = 0xB9;
            state[0x0603 + MachineFixture.MemoryStateOffset] = 0x0E;
            state[0x0604 + MachineFixture.MemoryStateOffset] = 0x06;
            state[0x0605 + MachineFixture.MemoryStateOffset] = 0xF0;
            state[0x0606 + MachineFixture.MemoryStateOffset] = 0x06;
            state[0x0607 + MachineFixture.MemoryStateOffset] = 0x99;
            state[0x0608 + MachineFixture.MemoryStateOffset] = 0x00;
            state[0x0609 + MachineFixture.MemoryStateOffset] = 0xF0;
            state[0x060A + MachineFixture.MemoryStateOffset] = 0xC8;
            state[0x060B + MachineFixture.MemoryStateOffset] = 0xD0;
            state[0x060C + MachineFixture.MemoryStateOffset] = 0xF5;
            state[0x060D + MachineFixture.MemoryStateOffset] = 0x00;
            state[0x060E + MachineFixture.MemoryStateOffset] = 0x06;
            state[0x060F + MachineFixture.MemoryStateOffset] = 0x05;
            state[0x0610 + MachineFixture.MemoryStateOffset] = 0x01;
            state[0x0611 + MachineFixture.MemoryStateOffset] = 0x02;
            state[0x0612 + MachineFixture.MemoryStateOffset] = 0x00;

            state[0xFFFE + MachineFixture.MemoryStateOffset] = 0xFF;
            state[0xFFFF + MachineFixture.MemoryStateOffset] = 0xFF;

            return state;
        }
    }
}
