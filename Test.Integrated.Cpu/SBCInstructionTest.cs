using System;
using System.Collections.Generic;
using Test.Integrated.Cpu.Common;
using Test.Integrated.Cpu.Files;
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
            const ushort accLocation = 3 + MachineFixture.RegisterOffset;

            var programStream = BuildProgramStream(programName);
            var result = this.Fixture.Compute(programStream);

            Assert.Equal(expectedAccumulator, result[accLocation]);
        }

        private static IEnumerable<byte> BuildProgramStream(string programName)
        {
            var state = new byte[MachineFixture.LoadDataLength];
            var program = Resources.ResourceManager.GetObject(programName) as byte[];

            Array.Copy(program, 0, state, MachineFixture.MemoryStateOffset, program.Length);

            state[0xFFFE + MachineFixture.MemoryStateOffset] = 0xFF;
            state[0xFFFF + MachineFixture.MemoryStateOffset] = 0xFF;

            return state;
        }
    }
}
