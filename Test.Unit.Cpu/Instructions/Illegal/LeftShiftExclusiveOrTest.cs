using Cpu.Instructions.Exceptions;
using Cpu.Instructions.Illegal;
using Cpu.States;
using Moq;
using Test.Unit.Cpu.Utils;
using Xunit;

namespace Test.Unit.Cpu.Instructions.Illegal
{
    public sealed record LeftShiftExclusiveOrTest : IClassFixture<LeftShiftExclusiveOr>
    {
        #region Properties
        private LeftShiftExclusiveOr Subject { get; }
        #endregion

        #region Constructors
        public LeftShiftExclusiveOrTest(LeftShiftExclusiveOr subject)
        {
            this.Subject = subject;
        }
        #endregion

        [Theory]
        [InlineData(0x47)]
        [InlineData(0x57)]
        [InlineData(0x4F)]
        [InlineData(0x5F)]
        [InlineData(0x5B)]
        [InlineData(0x43)]
        [InlineData(0x53)]
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
            var stateMock = SetupMock(0x00, 0x00);
            _ = Assert.Throws<UnknownOpcodeException>(() => this.Subject.Execute(stateMock.Object, 0));
        }

        [Fact]
        public void ZeroValue_ZeroAccumulator_WritesZeroFlag()
        {
            const ushort address = 0b_0000_0001;

            const byte value = 0b_0000_0000;
            const byte accumulator = 0b_0000_0000;

            var stateMock = SetupMock(0x4F, accumulator);

            _ = stateMock
                .Setup(s => s.Memory.ReadAbsolute(address))
                .Returns(value);

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Registers.Accumulator, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsZero = true, Times.Once());
        }

        [Fact]
        public void Value_Accumulator_WritesCarryFlag()
        {
            const ushort address = 0b_0000_0001;

            const byte value = 0b_1000_0001;
            const byte accumulator = 0b_0000_0001;

            var stateMock = SetupMock(0x4F, accumulator);

            _ = stateMock
                .Setup(s => s.Memory.ReadAbsolute(address))
                .Returns(value);

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Registers.Accumulator, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsCarry = true, Times.Once());
        }

        [Fact]
        public void Value_Accumulator_WritesNegativeFlag()
        {
            const ushort address = 0b_0000_0001;

            const byte value = 0b_0100_0001;
            const byte accumulator = 0b_0000_0001;

            var stateMock = SetupMock(0x4F, accumulator);

            _ = stateMock
                .Setup(s => s.Memory.ReadAbsolute(address))
                .Returns(value);

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Registers.Accumulator, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsNegative = true, Times.Once());
        }

        [Fact]
        public void Value_Accumulator_ReadWriteZeroPage()
        {
            const ushort address = 0b_0000_0001;

            const byte value = 0b_0100_0001;
            const byte accumulator = 0b_0000_0001;
            const byte result = 0b_1000_0011;

            var stateMock = SetupMock(0x47, accumulator);

            _ = stateMock
                .Setup(s => s.Memory.ReadZeroPage(address))
                .Returns(value);

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Memory.ReadZeroPage(address), Times.Once());
            stateMock.Verify(state => state.Memory.WriteZeroPage(address, result), Times.Once());
        }

        [Fact]
        public void Value_Accumulator_ReadWriteZeroPageX()
        {
            const ushort address = 0b_0000_0001;

            const byte value = 0b_0100_0001;
            const byte accumulator = 0b_0000_0001;
            const byte result = 0b_1000_0011;

            var stateMock = SetupMock(0x57, accumulator);

            _ = stateMock
                .Setup(s => s.Memory.ReadZeroPageX(address))
                .Returns(value);

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Memory.ReadZeroPageX(address), Times.Once());
            stateMock.Verify(state => state.Memory.WriteZeroPageX(address, result), Times.Once());
        }

        [Fact]
        public void Value_Accumulator_ReadWriteIndirectX()
        {
            const ushort address = 0b_0000_0001;

            const byte value = 0b_0100_0001;
            const byte accumulator = 0b_0000_0001;
            const byte result = 0b_1000_0011;

            var stateMock = SetupMock(0x43, accumulator);

            _ = stateMock
                .Setup(s => s.Memory.ReadIndirectX(address))
                .Returns(value);

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Memory.ReadIndirectX(address), Times.Once());
            stateMock.Verify(state => state.Memory.WriteIndirectX(address, result), Times.Once());
        }

        [Fact]
        public void Value_Accumulator_ReadWriteIndirectY()
        {
            const ushort address = 0b_0000_0001;

            const byte value = 0b_0100_0001;
            const byte accumulator = 0b_0000_0001;
            const byte result = 0b_1000_0011;

            var stateMock = SetupMock(0x53, accumulator);

            _ = stateMock
                .Setup(s => s.Memory.ReadIndirectY(address))
                .Returns((false, value));

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Memory.ReadIndirectY(address), Times.Once());
            stateMock.Verify(state => state.Memory.WriteIndirectY(address, result), Times.Once());
        }

        [Fact]
        public void Value_Accumulator_ReadWriteAbsolute()
        {
            const ushort address = 0b_0000_0001;

            const byte value = 0b_0100_0001;
            const byte accumulator = 0b_0000_0001;
            const byte result = 0b_1000_0011;

            var stateMock = SetupMock(0x4F, accumulator);

            _ = stateMock
                .Setup(s => s.Memory.ReadAbsolute(address))
                .Returns(value);

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Memory.ReadAbsolute(address), Times.Once());
            stateMock.Verify(state => state.Memory.WriteAbsolute(address, result), Times.Once());
        }

        [Fact]
        public void Value_Accumulator_ReadWriteAbsoluteX()
        {
            const ushort address = 0b_0000_0001;

            const byte value = 0b_0100_0001;
            const byte accumulator = 0b_0000_0001;
            const byte result = 0b_1000_0011;

            var stateMock = SetupMock(0x5F, accumulator);

            _ = stateMock
                .Setup(s => s.Memory.ReadAbsoluteX(address))
                .Returns((false, value));

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Memory.ReadAbsoluteX(address), Times.Once());
            stateMock.Verify(state => state.Memory.WriteAbsoluteX(address, result), Times.Once());
        }

        [Fact]
        public void Value_Accumulator_ReadWriteAbsoluteY()
        {
            const ushort address = 0b_0000_0001;

            const byte value = 0b_0100_0001;
            const byte accumulator = 0b_0000_0001;
            const byte result = 0b_1000_0011;

            var stateMock = SetupMock(0x5B, accumulator);

            _ = stateMock
                .Setup(s => s.Memory.ReadAbsoluteY(address))
                .Returns((false, value));

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Memory.ReadAbsoluteY(address), Times.Once());
            stateMock.Verify(state => state.Memory.WriteAbsoluteY(address, result), Times.Once());
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
