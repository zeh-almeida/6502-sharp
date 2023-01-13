using Cpu.Instructions.Exceptions;
using Cpu.Instructions.Shifts;
using Cpu.States;
using Moq;
using Test.Unit.Cpu.Utils;
using Xunit;

namespace Test.Unit.Cpu.Instructions.Shifts
{
    public sealed record ArithmeticShiftRightTest : IClassFixture<ArithmeticShiftRight>
    {
        #region Properties
        private ArithmeticShiftRight Subject { get; }
        #endregion

        #region Constructors
        public ArithmeticShiftRightTest(ArithmeticShiftRight subject)
        {
            this.Subject = subject;
        }
        #endregion

        [Theory]
        [InlineData(0x4A)]
        [InlineData(0x46)]
        [InlineData(0x56)]
        [InlineData(0x4E)]
        [InlineData(0x5E)]
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
            var stateMock = SetupMock(0x00);
            _ = Assert.Throws<UnknownOpcodeException>(() => this.Subject.Execute(stateMock.Object, 0));
        }

        [Fact]
        public void Execute_PositiveSeventhBit_WritesNegativeFlag()
        {
            const byte value = 0b_1111_1110;
            const byte finalValue = 0b_0111_1111;

            var stateMock = SetupMock(0x4A);

            _ = stateMock
                .Setup(s => s.Registers.Accumulator)
                .Returns(value);

            this.Subject.Execute(stateMock.Object, value);

            stateMock.Verify(state => state.Registers.Accumulator, Times.Once());
            stateMock.VerifySet(state => state.Registers.Accumulator = finalValue, Times.Once());

            stateMock.VerifySet(state => state.Flags.IsCarry = false, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsNegative = true, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsZero = false, Times.Once());
        }

        [Fact]
        public void Execute_PositiveZeroBit_WritesCarryFlag()
        {
            const byte value = 0b_0111_1101;
            const byte finalValue = 0b_0011_1110;

            var stateMock = SetupMock(0x4A);

            _ = stateMock
                .Setup(s => s.Registers.Accumulator)
                .Returns(value);

            this.Subject.Execute(stateMock.Object, value);

            stateMock.Verify(state => state.Registers.Accumulator, Times.Once());
            stateMock.VerifySet(state => state.Registers.Accumulator = finalValue, Times.Once());

            stateMock.VerifySet(state => state.Flags.IsCarry = true, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsNegative = false, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsZero = false, Times.Once());
        }

        [Fact]
        public void Execute_Zero_WritesZeroFlag()
        {
            const byte value = 0;
            const byte finalValue = 0;

            var stateMock = SetupMock(0x4A);

            _ = stateMock
                .Setup(s => s.Registers.Accumulator)
                .Returns(value);

            this.Subject.Execute(stateMock.Object, value);

            stateMock.Verify(state => state.Registers.Accumulator, Times.Once());
            stateMock.VerifySet(state => state.Registers.Accumulator = finalValue, Times.Once());

            stateMock.VerifySet(state => state.Flags.IsCarry = false, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsNegative = false, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsZero = true, Times.Once());
        }

        [Fact]
        public void Execute_AccumulatorAddress_ReadWritesValue()
        {
            const byte value = 0;
            const byte finalValue = 0;

            var stateMock = SetupMock(0x4A);

            _ = stateMock
                .Setup(s => s.Registers.Accumulator)
                .Returns(value);

            this.Subject.Execute(stateMock.Object, value);

            stateMock.Verify(state => state.Registers.Accumulator, Times.Once());
            stateMock.VerifySet(state => state.Registers.Accumulator = finalValue, Times.Once());
        }

        [Fact]
        public void Execute_ZeroPageAddress_ReadWritesValue()
        {
            const byte value = 0;
            const ushort address = 0;
            const byte finalValue = 0;

            var stateMock = SetupMock(0x46);

            _ = stateMock
                .Setup(s => s.Memory.ReadZeroPage(address))
                .Returns(value);

            this.Subject.Execute(stateMock.Object, value);

            stateMock.Verify(state => state.Memory.ReadZeroPage(address), Times.Once());
            stateMock.Verify(state => state.Memory.WriteZeroPage(address, finalValue), Times.Once());
        }

        [Fact]
        public void Execute_ZeroPageXAddress_ReadWritesValue()
        {
            const byte value = 0;
            const ushort address = 0;

            const byte finalValue = 0;

            var stateMock = SetupMock(0x56);

            _ = stateMock
                .Setup(s => s.Memory.ReadZeroPageX(address))
                .Returns(value);

            this.Subject.Execute(stateMock.Object, value);

            stateMock.Verify(state => state.Memory.ReadZeroPageX(address), Times.Once());
            stateMock.Verify(state => state.Memory.WriteZeroPageX(address, finalValue), Times.Once());
        }

        [Fact]
        public void Execute_AbsoluteAddress_ReadWritesValue()
        {
            const byte value = 0;
            const ushort address = 0;
            const byte finalValue = 0;

            var stateMock = SetupMock(0x4E);

            _ = stateMock
                .Setup(s => s.Memory.ReadAbsolute(address))
                .Returns(value);

            this.Subject.Execute(stateMock.Object, value);

            stateMock.Verify(state => state.Memory.ReadAbsolute(address), Times.Once());
            stateMock.Verify(state => state.Memory.WriteAbsolute(address, finalValue), Times.Once());
        }

        [Fact]
        public void Execute_AbsoluteXAddress_ReadWritesValue()
        {
            const byte value = 0;
            const ushort address = 0;

            const byte finalValue = 0;

            var stateMock = SetupMock(0x5E);

            _ = stateMock
                .Setup(s => s.Memory.ReadAbsoluteX(address))
                .Returns((false, value));

            this.Subject.Execute(stateMock.Object, value);

            stateMock.Verify(state => state.Memory.ReadAbsoluteX(address), Times.Once());
            stateMock.Verify(state => state.Memory.WriteAbsoluteX(address, finalValue), Times.Once());
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
