using Cpu.Instructions.Exceptions;
using Cpu.Instructions.Shifts;
using Cpu.States;
using Moq;
using Test.Unit.Cpu.Utils;
using Xunit;

namespace Test.Unit.Cpu.Instructions.Shifts
{
    public sealed record RotateLeftTest : IClassFixture<RotateLeft>
    {
        #region Properties
        private RotateLeft Subject { get; }
        #endregion

        #region Constructors
        public RotateLeftTest(RotateLeft subject)
        {
            this.Subject = subject;
        }
        #endregion

        [Theory]
        [InlineData(0x2A)]
        [InlineData(0x26)]
        [InlineData(0x36)]
        [InlineData(0x2E)]
        [InlineData(0x3E)]
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
            var stateMock = SetupMock(0x00, false);
            _ = Assert.Throws<UnknownOpcodeException>(() => this.Subject.Execute(stateMock.Object, 0));
        }

        [Fact]
        public void Execute_PositiveSixthBit_WritesNegativeFlag()
        {
            const bool isCarry = false;
            const byte value = 0b_0111_1111;
            const byte finalValue = 0b_1111_1110;

            var stateMock = SetupMock(0x2A, isCarry);

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
        public void Execute_PositiveSeventhBit_WritesCarryFlag()
        {
            const bool isCarry = false;
            const byte value = 0b_1011_1101;
            const byte finalValue = 0b_0111_1010;

            var stateMock = SetupMock(0x2A, isCarry);

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
        public void Execute_WithCarry_Rotates()
        {
            const bool isCarry = true;
            const byte value = 0b_1011_1101;
            const byte finalValue = 0b_0111_1011;

            var stateMock = SetupMock(0x2A, isCarry);

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
            const bool isCarry = false;
            const byte value = 0;
            const byte finalValue = 0;

            var stateMock = SetupMock(0x2A, isCarry);

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
            const bool isCarry = false;
            const byte value = 0;
            const byte finalValue = 0;

            var stateMock = SetupMock(0x2A, isCarry);

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
            const bool isCarry = false;
            const byte value = 0;
            const ushort address = 0;
            const byte finalValue = 0;

            var stateMock = SetupMock(0x26, isCarry);

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
            const bool isCarry = false;
            const byte value = 0;
            const ushort address = 0;

            const byte finalValue = 0;

            var stateMock = SetupMock(0x36, isCarry);

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
            const bool isCarry = false;
            const byte value = 0;
            const ushort address = 0;
            const byte finalValue = 0;

            var stateMock = SetupMock(0x2E, isCarry);

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
            const bool isCarry = false;
            const byte value = 0;
            const ushort address = 0;

            const byte finalValue = 0;

            var stateMock = SetupMock(0x3E, isCarry);

            _ = stateMock
                .Setup(s => s.Memory.ReadAbsoluteX(address))
                .Returns(value);

            this.Subject.Execute(stateMock.Object, value);

            stateMock.Verify(state => state.Memory.ReadAbsoluteX(address), Times.Once());
            stateMock.Verify(state => state.Memory.WriteAbsoluteX(address, finalValue), Times.Once());
        }

        private static Mock<ICpuState> SetupMock(byte opcode, bool isCarry)
        {
            var stateMock = TestUtils.GenerateStateMock();

            _ = stateMock
                .Setup(s => s.ExecutingOpcode)
                .Returns(opcode);

            _ = stateMock
                .Setup(s => s.Flags.IsCarry)
                .Returns(isCarry);

            return stateMock;
        }
    }
}
