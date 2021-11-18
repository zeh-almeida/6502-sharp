using System;
using System.Collections.Generic;
using Test.Integrated.Cpu.Common;
using Test.Integrated.Cpu.Files;
using Xunit;

namespace Test.Integrated.Cpu
{
    public sealed record SLOInstructionTest : IClassFixture<MachineFixture>
    {
        #region Properties
        private MachineFixture Fixture { get; }
        #endregion

        #region Constructors
        public SLOInstructionTest(MachineFixture fixture)
        {
            this.Fixture = fixture;
        }
        #endregion

        [Theory]
        [InlineData("asoa")]
        [InlineData("asoax")]
        [InlineData("asoay")]
        [InlineData("asoix")]
        [InlineData("asoiy")]
        [InlineData("asoz")]
        [InlineData("asozx")]
        public void Illegal_Instruction_Computes(string programName)
        {
            var programStream = BuildProgramStream(programName);
            _ = this.Fixture.Compute(programStream);
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
