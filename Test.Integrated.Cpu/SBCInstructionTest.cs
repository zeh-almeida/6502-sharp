using Cpu.States;
using Test.Integrated.Cpu.Common;
using Xunit;

namespace Test.Integrated.Cpu
{
    public sealed record SBCInstructionTest : IClassFixture<MachineFixture>
    {
        #region Properties
        private MachineFixture Fixture { get; }
        #endregion

        #region Constructors
        public SBCInstructionTest(MachineFixture fixture)
        {
            this.Fixture = fixture;
        }
        #endregion

        [Theory]
        [InlineData("sbc1", 0xFF)]
        [InlineData("sbc2", 0x7F)]
        [InlineData("sbc3", 0x80)]
        [InlineData("sbc4", 0x7F)]
        [InlineData("sbc5", 0x34)]
        [InlineData("sbc6", 0x91)]
        public void Program_Executes(string programName, byte expectedAccumulator)
        {
            const ushort accLocation = 3 + ICpuState.RegisterOffset;
            var result = this.Fixture.Compute(programName);

            Assert.Equal(expectedAccumulator, result[accLocation]);
        }
    }
}
