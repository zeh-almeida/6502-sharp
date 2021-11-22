using System;
using System.Collections.Generic;
using Test.Integrated.Cpu.Common;
using Test.Integrated.Cpu.Files;
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
