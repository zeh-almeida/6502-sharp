using Cpu.States;
using Test.Integrated.Cpu.Common;
using Xunit;

namespace Test.Integrated.Cpu
{
    public sealed record ADCInstructionTest : IClassFixture<MachineFixture>
    {
        #region Properties
        private MachineFixture Fixture { get; }
        #endregion

        #region Constructors
        public ADCInstructionTest(MachineFixture fixture)
        {
            this.Fixture = fixture;
        }
        #endregion

        [Theory]
        [InlineData("adc1", 0x02)]
        [InlineData("adc2", 0x00)]
        [InlineData("adc3", 0x80)]
        [InlineData("adc4", 0x7f)]
        [InlineData("adc5", 0x80)]
        [InlineData("adc6", 0x04)]
        [InlineData("adc7", 0x05)]
        [InlineData("adc8", 0x41)]
        public void Program_Executes(string programName, byte expectedAccumulator)
        {
            const ushort accLocation = 3 + ICpuState.RegisterOffset;
            var result = this.Fixture.Compute(programName);

            Assert.Equal(expectedAccumulator, result[accLocation]);
        }
    }
}
