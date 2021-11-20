using Cpu.Instructions.Arithmetic;
using Cpu.Instructions.Exceptions;
using Cpu.States;
using Moq;
using Test.Unit.Cpu.Utils;
using Xunit;

namespace Test.Unit.Cpu.Instructions.Arithmetic
{
    public sealed record AddWithCarryTest : IClassFixture<AddWithCarry>
    {
        #region Properties
        private AddWithCarry Subject { get; }
        #endregion

        #region Constructors
        public AddWithCarryTest(AddWithCarry subject)
        {
            this.Subject = subject;
        }
        #endregion

        [Theory]
        [InlineData(0x69)]
        [InlineData(0x65)]
        [InlineData(0x75)]
        [InlineData(0x6D)]
        [InlineData(0x7D)]
        [InlineData(0x79)]
        [InlineData(0x61)]
        [InlineData(0x71)]
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
            var stateMock = SetupMock(0x00, 0);
            _ = Assert.Throws<UnknownOpcodeException>(() => this.Subject.Execute(stateMock.Object, 0));
        }

        [Fact]
        public void Execute_Equals_WritesZeroFlag()
        {
            const byte value = 0b_0000_0000;
            const byte accumulator = 0b_0000_0000;
            const byte result = 0b_0000_0000;

            var stateMock = SetupMock(0x69, accumulator);

            _ = this.Subject.Execute(stateMock.Object, value);

            stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());

            stateMock.VerifySet(state => state.Flags.IsZero = true, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsNegative = false, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsOverflow = false, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsCarry = false, Times.Once());
        }

        [Fact]
        public void Execute_WritesNegativeFlag()
        {
            const byte value = 0b_0111_1111;
            const byte accumulator = 0b_1000_0000;
            const byte result = 0b_1111_1111;

            var stateMock = SetupMock(0x69, accumulator);

            _ = this.Subject.Execute(stateMock.Object, value);

            stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());

            stateMock.VerifySet(state => state.Flags.IsZero = false, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsNegative = true, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsOverflow = false, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsCarry = false, Times.Once());
        }

        [Fact]
        public void Execute_WritesOverflowFlag()
        {
            const byte value = 0b_1111_1111;
            const byte accumulator = 0b_1000_0000;
            const byte result = 0b_0111_1111;

            var stateMock = SetupMock(0x69, accumulator);

            _ = this.Subject.Execute(stateMock.Object, value);

            stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());

            stateMock.VerifySet(state => state.Flags.IsZero = false, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsNegative = false, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsOverflow = true, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsCarry = true, Times.Once());
        }

        [Fact]
        public void Execute_WritesCarryFlag()
        {
            const byte value = 0b_1111_1111;
            const byte accumulator = 0b_0000_0001;
            const byte result = 0b_0000_0000;

            var stateMock = SetupMock(0x69, accumulator);

            _ = this.Subject.Execute(stateMock.Object, value);

            stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());

            stateMock.VerifySet(state => state.Flags.IsZero = true, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsNegative = false, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsOverflow = false, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsCarry = true, Times.Once());
        }

        [Fact]
        public void Execute_Carry_Sums()
        {
            const byte value = 0b_0000_0011;
            const byte accumulator = 0b_0000_0001;
            const byte result = 0b_0000_0101;

            var stateMock = SetupMock(0x69, accumulator);

            _ = stateMock
                .Setup(s => s.Flags.IsCarry)
                .Returns(true);

            _ = this.Subject.Execute(stateMock.Object, value);

            stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());
        }

        [Fact]
        public void Execute_BCD_WritesNegativeFlag()
        {
            const byte value = 0b_0100_0000;
            const byte accumulator = 0b_0100_0000;
            const byte result = 0b_1000_0000;

            var stateMock = SetupMock(0x69, accumulator);

            _ = stateMock
                .Setup(s => s.Flags.IsDecimalMode)
                .Returns(true);

            _ = this.Subject.Execute(stateMock.Object, value);

            stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsNegative = true, Times.Once());
        }

        [Fact]
        public void Execute_BCD_WritesCarryFlag()
        {
            const byte value = 0b_0100_0110;
            const byte accumulator = 0b_0101_1000;
            const byte result = 0b_0000_0100;

            var stateMock = SetupMock(0x69, accumulator);

            _ = stateMock
                .Setup(s => s.Flags.IsDecimalMode)
                .Returns(true);

            _ = this.Subject.Execute(stateMock.Object, value);

            stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsCarry = true, Times.Once());
        }

        [Fact]
        public void Execute_BCD_WritesZeroFlag()
        {
            const byte value = 0b_0000_0001;
            const byte accumulator = 0b_1001_1001;
            const byte result = 0b_0000_0000;

            var stateMock = SetupMock(0x69, accumulator);

            _ = stateMock
                .Setup(s => s.Flags.IsDecimalMode)
                .Returns(true);

            _ = this.Subject.Execute(stateMock.Object, value);

            stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsZero = true, Times.Once());
        }

        [Fact]
        public void Execute_BCD_Carry_Sums()
        {
            const byte value = 0b_0000_0001;
            const byte accumulator = 0b_0000_0001;
            const byte result = 0b_0000_0011;

            var stateMock = SetupMock(0x69, accumulator);

            _ = stateMock
                .Setup(s => s.Flags.IsDecimalMode)
                .Returns(true);

            _ = stateMock
                .Setup(s => s.Flags.IsCarry)
                .Returns(true);

            _ = this.Subject.Execute(stateMock.Object, value);

            stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());
        }

        [Fact]
        public void Execute_Immediate_Sums()
        {
            const byte value = 0b_0000_0011;
            const byte accumulator = 0b_0000_0001;
            const byte result = 0b_0000_0100;

            var stateMock = SetupMock(0x69, accumulator);

            _ = this.Subject.Execute(stateMock.Object, value);

            stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());
        }

        [Fact]
        public void Execute_ZeroPage_Sums()
        {
            const ushort address = 2;

            const byte value = 0b_0000_0011;
            const byte accumulator = 0b_0000_0001;
            const byte result = 0b_0000_0100;

            var stateMock = SetupMock(0x65, accumulator);

            _ = stateMock
                .Setup(s => s.Memory.ReadZeroPage(address))
                .Returns(value);

            _ = this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Memory.ReadZeroPage(address), Times.Once());

            stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());
        }

        [Fact]
        public void Execute_ZeroPageX_Sums()
        {
            const ushort address = 2;

            const byte value = 0b_0000_0011;
            const byte accumulator = 0b_0000_0001;
            const byte result = 0b_0000_0100;

            var stateMock = SetupMock(0x75, accumulator);

            _ = stateMock
                .Setup(s => s.Memory.ReadZeroPageX(address))
                .Returns(value);

            _ = this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Memory.ReadZeroPageX(address), Times.Once());
            stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());
        }

        [Fact]
        public void Execute_Absolute_Sums()
        {
            const ushort address = 2;

            const byte value = 0b_0000_0011;
            const byte accumulator = 0b_0000_0001;
            const byte result = 0b_0000_0100;

            var stateMock = SetupMock(0x6D, accumulator);

            _ = stateMock
                .Setup(s => s.Memory.ReadAbsolute(address))
                .Returns(value);

            _ = this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Memory.ReadAbsolute(address), Times.Once());
            stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());
        }

        [Fact]
        public void Execute_AbsoluteX_Sums()
        {
            const ushort address = 2;

            const byte value = 0b_0000_0011;
            const byte accumulator = 0b_0000_0001;
            const byte result = 0b_0000_0100;

            var stateMock = SetupMock(0x7D, accumulator);

            _ = stateMock
                .Setup(s => s.Memory.ReadAbsoluteX(address))
                .Returns(value);

            _ = this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Memory.ReadAbsoluteX(address), Times.Once());
            stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());
        }

        [Fact]
        public void Execute_AbsoluteY_Sums()
        {
            const ushort address = 2;

            const byte value = 0b_0000_0011;
            const byte accumulator = 0b_0000_0001;
            const byte result = 0b_0000_0100;

            var stateMock = SetupMock(0x79, accumulator);

            _ = stateMock
                .Setup(s => s.Memory.ReadAbsoluteY(address))
                .Returns(value);

            _ = this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Memory.ReadAbsoluteY(address), Times.Once());
            stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());
        }

        [Fact]
        public void Execute_IndirectX_Sums()
        {
            const ushort address = 1;

            const byte value = 0b_0000_0011;
            const byte accumulator = 0b_0000_0001;
            const byte result = 0b_0000_0100;

            var stateMock = SetupMock(0x61, accumulator);

            _ = stateMock
                .Setup(s => s.Memory.ReadIndirectX(address))
                .Returns(value);

            _ = this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Memory.ReadIndirectX(address), Times.Once());
            stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());
        }

        [Fact]
        public void Execute_IndirectY_Sums()
        {
            const ushort address = 0;

            const byte value = 0b_0000_0011;
            const byte accumulator = 0b_0000_0001;
            const byte result = 0b_0000_0100;

            var stateMock = SetupMock(0x71, accumulator);

            _ = stateMock
                .Setup(s => s.Memory.ReadIndirectY(address))
                .Returns(value);

            _ = this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Memory.ReadIndirectY(address), Times.Once());
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
