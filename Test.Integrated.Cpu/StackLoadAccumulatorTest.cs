using Cpu.States;
using Test.Integrated.Cpu.Common;
using Xunit;

namespace Test.Integrated.Cpu
{
    public sealed record StackLoadAccumulatorTest : IClassFixture<MachineFixture>
    {
        #region Properties
        private MachineFixture Fixture { get; }
        #endregion

        #region Constructors
        public StackLoadAccumulatorTest(MachineFixture fixture)
        {
            this.Fixture = fixture;
        }
        #endregion

        [Fact]
        public void Stacking_Accumulator_Computes()
        {
            const byte stackPointerValue = 0xFD;
            const byte accumulatorValue = 8;

            const byte firstStackValue = 0x01;
            const byte secondStackValue = 0x05;
            const byte thirdStackValue = 0x08;

            const ushort stackPointerLocation = 2 + ICpuState.RegisterOffset;
            const ushort accumulatorLocation = 3 + ICpuState.RegisterOffset;

            const ushort firstStackLocation = 0x0200 + ICpuState.MemoryStateOffset;
            const ushort secondStackLocation = 0x0201 + ICpuState.MemoryStateOffset;
            const ushort thirdStackLocation = 0x0202 + ICpuState.MemoryStateOffset;

            var programStream = BuildProgramStream();
            var finalState = this.Fixture.Compute(programStream);

            Assert.Equal(stackPointerValue, finalState[stackPointerLocation]);
            Assert.Equal(accumulatorValue, finalState[accumulatorLocation]);

            Assert.Equal(firstStackValue, finalState[firstStackLocation]);
            Assert.Equal(secondStackValue, finalState[secondStackLocation]);
            Assert.Equal(thirdStackValue, finalState[thirdStackLocation]);
        }

        private static IEnumerable<byte> BuildProgramStream()
        {
            var state = new byte[ICpuState.Length];

            state[0x0000 + ICpuState.RegisterOffset] = 0b_0000_0000;
            state[0x0001 + ICpuState.RegisterOffset] = 0b_0000_0110;

            state[0x0600 + ICpuState.MemoryStateOffset] = 0xA9;
            state[0x0601 + ICpuState.MemoryStateOffset] = 0x01;
            state[0x0602 + ICpuState.MemoryStateOffset] = 0x8D;
            state[0x0603 + ICpuState.MemoryStateOffset] = 0x00;
            state[0x0604 + ICpuState.MemoryStateOffset] = 0x02;
            state[0x0605 + ICpuState.MemoryStateOffset] = 0xA9;
            state[0x0606 + ICpuState.MemoryStateOffset] = 0x05;
            state[0x0607 + ICpuState.MemoryStateOffset] = 0x8D;
            state[0x0608 + ICpuState.MemoryStateOffset] = 0x01;
            state[0x0609 + ICpuState.MemoryStateOffset] = 0x02;
            state[0x060A + ICpuState.MemoryStateOffset] = 0xA9;
            state[0x060B + ICpuState.MemoryStateOffset] = 0x08;
            state[0x060C + ICpuState.MemoryStateOffset] = 0x8D;
            state[0x060D + ICpuState.MemoryStateOffset] = 0x02;
            state[0x060E + ICpuState.MemoryStateOffset] = 0x02;
            state[0xFFFE + ICpuState.MemoryStateOffset] = 0xFF;
            state[0xFFFF + ICpuState.MemoryStateOffset] = 0xFF;

            return state;
        }
    }
}
