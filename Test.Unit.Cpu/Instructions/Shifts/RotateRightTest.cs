using Cpu.Instructions.Exceptions;
using Cpu.Instructions.Shifts;
using Cpu.States;
using Moq;
using Test.Unit.Cpu.Utils;
using Xunit;

namespace Test.Unit.Cpu.Instructions.Shifts
{
    public sealed record RotateRightTest : IClassFixture<RotateRight>
    {
        #region Properties
        private RotateRight Subject { get; }
        #endregion

        #region Constructors
        public RotateRightTest(RotateRight subject)
        {
            this.Subject = subject;
        }
        #endregion

        [Theory]
        [InlineData(0x6A)]
        [InlineData(0x66)]
        [InlineData(0x76)]
        [InlineData(0x6E)]
        [InlineData(0x7E)]
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
            var stateMock = SetupMock(0x00, false);
            _ = Assert.Throws<UnknownOpcodeException>(() => this.Subject.Execute(stateMock.Object, 0));
        }

        [Fact]
        public void Execute_PositiveCarry_WritesNegativeFlag()
        {
            const bool isCarry = true;
            const byte value = 0b_1111_1110;
            const byte finalValue = 0b_1111_1111;

            var stateMock = SetupMock(0x6A, isCarry);

            _ = stateMock
                .Setup(s => s.Registers.Accumulator)
                .Returns(value);

            this.Subject.Execute(stateMock.Object, value);

            stateMock.Verify(state => state.Registers.Accumulator, Times.Once());
            stateMock.VerifySet(state => state.Registers.Accumulator = finalValue, Times.Once());

            stateMock.VerifySet(state => state.Flags.IsNegative = true, Times.Once());
        }

        [Fact]
        public void Execute_PositiveZeroBit_WritesCarryFlag()
        {
            const bool isCarry = false;
            const byte value = 0b_0111_1101;
            const byte finalValue = 0b_0011_1110;

            var stateMock = SetupMock(0x6A, isCarry);

            _ = stateMock
                .Setup(s => s.Registers.Accumulator)
                .Returns(value);

            this.Subject.Execute(stateMock.Object, value);

            stateMock.Verify(state => state.Registers.Accumulator, Times.Once());
            stateMock.VerifySet(state => state.Registers.Accumulator = finalValue, Times.Once());

            stateMock.VerifySet(state => state.Flags.IsCarry = true, Times.Once());
        }

        [Fact]
        public void Execute_Zero_WritesZeroFlag()
        {
            const bool isCarry = false;
            const byte value = 0;
            const byte finalValue = 0;

            var stateMock = SetupMock(0x6A, isCarry);

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

            var stateMock = SetupMock(0x6A, isCarry);

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

            var stateMock = SetupMock(0x66, isCarry);

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

            var stateMock = SetupMock(0x76, isCarry);

            _ = stateMock
                .Setup(s => s.Memory.ReadZeroPage(address))
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

            var stateMock = SetupMock(0x6E, isCarry);

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

            var stateMock = SetupMock(0x7E, isCarry);

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
