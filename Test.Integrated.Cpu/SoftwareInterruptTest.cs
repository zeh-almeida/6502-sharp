using Cpu.States;
using Test.Integrated.Cpu.Common;
using Xunit;

namespace Test.Integrated.Cpu
{
    public sealed record SoftwareInterruptTest : IClassFixture<MachineFixture>
    {
        #region Properties
        private MachineFixture Fixture { get; }
        #endregion

        #region Constructors
        public SoftwareInterruptTest(MachineFixture fixture)
        {
            this.Fixture = fixture;
        }
        #endregion

        [Theory]
        [InlineData("software_interrupt")]
        public void Computes(string programName)
        {
            const ushort xLocation = 4 + ICpuState.RegisterOffset;
            const ushort yLocation = 5 + ICpuState.RegisterOffset;

            const ushort xMemoryLocation = 0x0400 + ICpuState.MemoryStateOffset;
            const ushort yMemoryLocation = 0x0401 + ICpuState.MemoryStateOffset;

            var opcodes = 0;

            var result = this.Fixture.Compute(programName, 0, state =>
            {
                if (state.CyclesLeft == 0)
                {
                    opcodes++;

                    if (opcodes % 13 == 0)
                    {
                        state.IsSoftwareInterrupt = true;
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
