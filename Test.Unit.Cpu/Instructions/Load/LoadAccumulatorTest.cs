using Cpu.Instructions.Exceptions;
using Cpu.Instructions.Load;
using Cpu.States;
using Moq;
using Test.Unit.Cpu.Utils;
using Xunit;

namespace Test.Unit.Cpu.Instructions.Load
{
    public sealed record LoadAccumulatorTest : IClassFixture<LoadAccumulator>
    {
        #region Properties
        private LoadAccumulator Subject { get; }
        #endregion

        #region Constructors
        public LoadAccumulatorTest(LoadAccumulator subject)
        {
            this.Subject = subject;
        }
        #endregion

        [Theory]
        [InlineData(0xA9)]
        [InlineData(0xA5)]
        [InlineData(0xB5)]
        [InlineData(0xAD)]
        [InlineData(0xBD)]
        [InlineData(0xB9)]
        [InlineData(0xA1)]
        [InlineData(0xB1)]
        public void HasOpcode_Matches_True(byte opcode)
        {
            Assert.True(this.Subject.HasOpcode(opcode));
            Assert.NotNull(this.Subject.GatherInformation(opcode));
        }

        [Fact]
        public void GatherInformation_NoMatch_Throws()
        {
            _ = Assert.Throws<UnknownOpcodeException>(() => this.Subject.GatherInformation(0xFF));
        }

        [Fact]
        public void HashCode_Matches_True()
        {
            Assert.Equal(this.Subject.GetHashCode(), this.Subject.Opcodes.GetHashCode());
        }

        [Fact]
        public void Equals_Object_IsTrueForInstruction()
        {
            Assert.True(this.Subject.Equals(this.Subject));
            Assert.True(this.Subject.Equals(this.Subject as object));
        }

        [Fact]
        public void Equals_Object_IsFalseForNonInstructions()
        {
            Assert.False(this.Subject.Equals(1));
        }

        [Fact]
        public void Execute_UnknownOpcode_Throws()
        {
            var stateMock = SetupMock(0x00);
            _ = Assert.Throws<UnknownOpcodeException>(() => this.Subject.Execute(stateMock.Object, 0));
        }

        [Fact]
        public void Execute_Equals_WritesZeroFlag()
        {
            const byte value = 0b_0000_0000;
            const byte result = 0b_0000_0000;

            var stateMock = SetupMock(0xA9);

            this.Subject.Execute(stateMock.Object, value);

            stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());

            stateMock.VerifySet(state => state.Flags.IsZero = true, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsNegative = false, Times.Once());
        }

        [Fact]
        public void Execute_ResultSeventhBitSet_WritesNegativeFlag()
        {
            const byte value = 0b_1000_0011;
            const byte result = 0b_1000_0011;

            var stateMock = SetupMock(0xA9);

            this.Subject.Execute(stateMock.Object, value);

            stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());

            stateMock.VerifySet(state => state.Flags.IsZero = false, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsNegative = true, Times.Once());
        }

        [Fact]
        public void Execute_Immediate_Compares()
        {
            const byte value = 0b_0000_0011;
            const byte result = 0b_0000_0011;

            var stateMock = SetupMock(0xA9);

            this.Subject.Execute(stateMock.Object, value);

            stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());
        }

        [Fact]
        public void Execute_ZeroPage_Compares()
        {
            const ushort address = 2;

            const byte value = 0b_0000_0011;
            const byte result = 0b_0000_0011;

            var stateMock = SetupMock(0xA5);

            _ = stateMock
                .Setup(s => s.Memory.ReadZeroPage(address))
                .Returns(value);

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Memory.ReadZeroPage(address), Times.Once());

            stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());
        }

        [Fact]
        public void Execute_ZeroPageX_Compares()
        {
            const ushort address = 2;

            const byte value = 0b_0000_0011;
            const byte result = 0b_0000_0011;

            var stateMock = SetupMock(0xB5);

            _ = stateMock
                .Setup(s => s.Memory.ReadZeroPageX(address))
                .Returns(value);

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Memory.ReadZeroPageX(address), Times.Once());
            stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());
        }

        [Fact]
        public void Execute_Absolute_Compares()
        {
            const ushort address = 2;

            const byte value = 0b_0000_0011;
            const byte result = 0b_0000_0011;

            var stateMock = SetupMock(0xAD);

            _ = stateMock
                .Setup(s => s.Memory.ReadAbsolute(address))
                .Returns(value);

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Memory.ReadAbsolute(address), Times.Once());
            stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());
        }

        [Fact]
        public void Execute_AbsoluteX_Compares()
        {
            const ushort address = 2;

            const byte value = 0b_0000_0011;
            const byte result = 0b_0000_0011;

            var stateMock = SetupMock(0xBD);

            _ = stateMock
                .Setup(s => s.Memory.ReadAbsoluteX(address))
                .Returns(value);

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Memory.ReadAbsoluteX(address), Times.Once());
            stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());
        }

        [Fact]
        public void Execute_AbsoluteY_Compares()
        {
            const ushort address = 2;

            const byte value = 0b_0000_0011;
            const byte result = 0b_0000_0011;

            var stateMock = SetupMock(0xB9);

            _ = stateMock
                .Setup(s => s.Memory.ReadAbsoluteY(address))
                .Returns(value);

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Memory.ReadAbsoluteY(address), Times.Once());
            stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());
        }

        [Fact]
        public void Execute_IndirectX_Compares()
        {
            const ushort address = 1;

            const byte value = 0b_0000_0011;
            const byte result = 0b_0000_0011;

            var stateMock = SetupMock(0xA1);

            _ = stateMock
                .Setup(s => s.Memory.ReadIndirectX(address))
                .Returns(value);

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Memory.ReadIndirectX(address), Times.Once());
            stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());
        }

        [Fact]
        public void Execute_IndirectY_Compares()
        {
            const ushort address = 0;

            const byte value = 0b_0000_0011;
            const byte result = 0b_0000_0011;

            var stateMock = SetupMock(0xB1);

            _ = stateMock
                .Setup(s => s.Memory.ReadIndirectY(address))
                .Returns(value);

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Memory.ReadIndirectY(address), Times.Once());
            stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());
        }

        private static Mock<ICpuState> SetupMock(byte opcode)
        {
            var stateMock = TestUtils.GenerateStateMock();

            _ = stateMock
                .Setup(s => s.ExecutingOpcode)
                .Returns(opcode);

            return stateMock;
        }
    }
}
