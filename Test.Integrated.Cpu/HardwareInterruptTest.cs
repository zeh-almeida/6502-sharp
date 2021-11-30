using Test.Integrated.Cpu.Common;
using Xunit;

namespace Test.Integrated.Cpu
{
    public sealed record HardwareInterruptTest : IClassFixture<MachineFixture>
    {
        #region Properties
        private MachineFixture Fixture { get; }
        #endregion

        #region Constructors
        public HardwareInterruptTest(MachineFixture fixture)
        {
            this.Fixture = fixture;
        }
        #endregion

        [Theory]
        [InlineData("hardware_interrupt")]
        public void Computes(string programName)
        {
            const ushort xLocation = 4 + MachineFixture.RegisterOffset;
            const ushort yLocation = 5 + MachineFixture.RegisterOffset;

            const ushort xMemoryLocation = 0x0400 + MachineFixture.MemoryStateOffset;
            const ushort yMemoryLocation = 0x0401 + MachineFixture.MemoryStateOffset;

            var opcodes = 0;

            var result = this.Fixture.Compute(programName, state =>
            {
                if (state.CyclesLeft == 0)
                {
                    opcodes++;

                    if (opcodes % 13 == 0)
                    {
                        state.IsHardwareInterrupt = true;
                    }
                }
            });

            var xResult = result[xLocation];
            var xMemoryResult = result[xMemoryLocation];

            var yResult = result[yLocation];
            var yMemoryResult = result[yMemoryLocation];

            Assert.Equal(0, xResult);
            Assert.Equal(0, xMemoryResult);

            Assert.NotEqual(0, yResult);
            Assert.NotEqual(0, yMemoryResult);

            Assert.Equal(xMemoryResult, xResult);
            Assert.Equal(yMemoryResult, yResult);

            Assert.Equal(xResult + 1, yResult);
        }
    }
}
