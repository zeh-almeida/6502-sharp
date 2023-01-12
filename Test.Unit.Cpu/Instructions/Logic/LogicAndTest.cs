using Cpu.Instructions.Exceptions;
using Cpu.Instructions.Logic;
using Cpu.States;
using Moq;
using Test.Unit.Cpu.Utils;
using Xunit;

namespace Test.Unit.Cpu.Instructions.Logic
{
    public sealed record LogicAndTest : IClassFixture<LogicAnd>
    {
        #region Properties
        private LogicAnd Subject { get; }
        #endregion

        #region Constructors
        public LogicAndTest(LogicAnd subject)
        {
            this.Subject = subject;
        }
        #endregion

        [Theory]
        [InlineData(0x29)]
        [InlineData(0x25)]
        [InlineData(0x35)]
        [InlineData(0x2D)]
        [InlineData(0x3D)]
        [InlineData(0x39)]
        [InlineData(0x21)]
        [InlineData(0x31)]
        public void HasOpcode_Matches_True(byte opcode)
        {
            Assert.True(this.Subject.HasOpcode(opcode));            
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
            var stateMock = SetupMock(0x00, 0);
            _ = Assert.Throws<UnknownOpcodeException>(() => this.Subject.Execute(stateMock.Object, 0));
        }

        [Fact]
        public void Execute_Equals_WritesZeroFlag()
        {
            const byte value = 0b_0000_0000;
            const byte accumulator = 0b_0000_0000;
            const byte result = 0b_0000_0000;

            var stateMock = SetupMock(0x29, accumulator);

            this.Subject.Execute(stateMock.Object, value);

            stateMock.Verify(state => state.Registers.Accumulator, Times.Once());
            stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());

            stateMock.VerifySet(state => state.Flags.IsZero = true, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsNegative = false, Times.Once());
        }

        [Fact]
        public void Execute_ResultSeventhBitSet_WritesNegativeFlag()
        {
            const byte value = 0b_1000_0011;
            const byte accumulator = 0b_1000_0000;
            const byte result = 0b_1000_0000;

            var stateMock = SetupMock(0x29, accumulator);

            this.Subject.Execute(stateMock.Object, value);

            stateMock.Verify(state => state.Registers.Accumulator, Times.Once());
            stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());

            stateMock.VerifySet(state => state.Flags.IsZero = false, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsNegative = true, Times.Once());
        }

        [Fact]
        public void Execute_Immediate_Compares()
        {
            const byte value = 0b_0000_0011;
            const byte accumulator = 0b_0000_0011;
            const byte result = 0b_0000_0011;

            var stateMock = SetupMock(0x29, accumulator);

            this.Subject.Execute(stateMock.Object, value);

            stateMock.Verify(state => state.Registers.Accumulator, Times.Once());
            stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());
        }

        [Fact]
        public void Execute_ZeroPage_Compares()
        {
            const ushort address = 2;

            const byte value = 0b_0000_0011;
            const byte accumulator = 0b_0000_0011;
            const byte result = 0b_0000_0011;

            var stateMock = SetupMock(0x25, accumulator);

            _ = stateMock
                .Setup(s => s.Memory.ReadZeroPage(address))
                .Returns(value);

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Memory.ReadZeroPage(address), Times.Once());

            stateMock.Verify(state => state.Registers.Accumulator, Times.Once());
            stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());
        }

        [Fact]
        public void Execute_ZeroPageX_Compares()
        {
            const ushort address = 2;

            const byte value = 0b_0000_0011;
            const byte accumulator = 0b_0000_0011;
            const byte result = 0b_0000_0011;

            var stateMock = SetupMock(0x35, accumulator);

            _ = stateMock
                .Setup(s => s.Memory.ReadZeroPageX(address))
                .Returns(value);

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Memory.ReadZeroPageX(address), Times.Once());
            stateMock.Verify(state => state.Registers.Accumulator, Times.Once());
            stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());
        }

        [Fact]
        public void Execute_Absolute_Compares()
        {
            const ushort address = 2;

            const byte value = 0b_0000_0011;
            const byte accumulator = 0b_0000_0011;
            const byte result = 0b_0000_0011;

            var stateMock = SetupMock(0x2D, accumulator);

            _ = stateMock
                .Setup(s => s.Memory.ReadAbsolute(address))
                .Returns(value);

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Memory.ReadAbsolute(address), Times.Once());
            stateMock.Verify(state => state.Registers.Accumulator, Times.Once());
            stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());
        }

        [Fact]
        public void Execute_AbsoluteX_Compares()
        {
            const ushort address = 2;

            const byte value = 0b_0000_0011;
            const byte accumulator = 0b_0000_0011;
            const byte result = 0b_0000_0011;

            var stateMock = SetupMock(0x3D, accumulator);

            _ = stateMock
                .Setup(s => s.Memory.ReadAbsoluteX(address))
                .Returns((false, value));

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Memory.ReadAbsoluteX(address), Times.Once());
            stateMock.Verify(state => state.Registers.Accumulator, Times.Once());
            stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());
        }

        [Fact]
        public void Execute_AbsoluteY_Compares()
        {
            const ushort address = 2;

            const byte value = 0b_0000_0011;
            const byte accumulator = 0b_0000_0011;
            const byte result = 0b_0000_0011;

            var stateMock = SetupMock(0x39, accumulator);

            _ = stateMock
                .Setup(s => s.Memory.ReadAbsoluteY(address))
                .Returns((false, value));

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Memory.ReadAbsoluteY(address), Times.Once());
            stateMock.Verify(state => state.Registers.Accumulator, Times.Once());
            stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());
        }

        [Fact]
        public void Execute_IndirectX_Compares()
        {
            const ushort address = 1;

            const byte value = 0b_0000_0011;
            const byte accumulator = 0b_0000_0011;
            const byte result = 0b_0000_0011;

            var stateMock = SetupMock(0x21, accumulator);

            _ = stateMock
                .Setup(s => s.Memory.ReadIndirectX(address))
                .Returns(value);

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Memory.ReadIndirectX(address), Times.Once());
            stateMock.Verify(state => state.Registers.Accumulator, Times.Once());
            stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());
        }

        [Fact]
        public void Execute_IndirectY_Compares()
        {
            const ushort address = 0;

            const byte value = 0b_0000_0011;
            const byte accumulator = 0b_0000_0011;
            const byte result = 0b_0000_0011;

            var stateMock = SetupMock(0x31, accumulator);

            _ = stateMock
                .Setup(s => s.Memory.ReadIndirectY(address))
                .Returns((false, value));

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Memory.ReadIndirectY(address), Times.Once());
            stateMock.Verify(state => state.Registers.Accumulator, Times.Once());
            stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());
        }

        private static Mock<ICpuState> SetupMock(byte opcode, byte accumulator)
        {
            var stateMock = TestUtils.GenerateStateMock();

            _ = stateMock
                .Setup(s => s.ExecutingOpcode)
                .Returns(opcode);

            _ = stateMock
                .Setup(s => s.Registers.Accumulator)
                .Returns(accumulator);

            return stateMock;
        }
    }
}
