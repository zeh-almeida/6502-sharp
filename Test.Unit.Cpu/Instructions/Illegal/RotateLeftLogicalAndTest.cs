using Cpu.Instructions.Exceptions;
using Cpu.Instructions.Illegal;
using Cpu.States;
using Moq;
using Test.Unit.Cpu.Utils;
using Xunit;

namespace Test.Unit.Cpu.Instructions.Illegal
{
    public sealed record RotateLeftLogicalAndTest : IClassFixture<RotateLeftLogicalAnd>
    {
        #region Properties
        private RotateLeftLogicalAnd Subject { get; }
        #endregion

        #region Constructors
        public RotateLeftLogicalAndTest(RotateLeftLogicalAnd subject)
        {
            this.Subject = subject;
        }
        #endregion

        [Theory]
        [InlineData(0x27)]
        [InlineData(0x37)]
        [InlineData(0x2F)]
        [InlineData(0x3F)]
        [InlineData(0x3B)]
        [InlineData(0x23)]
        [InlineData(0x33)]
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
            var stateMock = SetupMock(0x00, 0x00, true);
            _ = Assert.Throws<UnknownOpcodeException>(() => this.Subject.Execute(stateMock.Object, 0));
        }

        [Fact]
        public void Zero_WritesZeroFlag()
        {
            const ushort address = 0b_0000_0001;

            const byte value = 0b_0000_0000;
            const byte accumulator = 0b_0000_0000;

            var stateMock = SetupMock(0x2F, accumulator, false);

            _ = stateMock
                .Setup(s => s.Memory.ReadAbsolute(address))
                .Returns(value);

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Registers.Accumulator, Times.Once());
            stateMock.VerifyGet(state => state.Flags.IsCarry, Times.Once());

            stateMock.VerifySet(state => state.Flags.IsZero = true, Times.Once());
        }

        [Fact]
        public void Value_WritesCarryFlag()
        {
            const ushort address = 0b_0000_0001;

            const byte value = 0b_1000_0001;
            const byte accumulator = 0b_0000_0001;

            var stateMock = SetupMock(0x2F, accumulator, false);

            _ = stateMock
                .Setup(s => s.Memory.ReadAbsolute(address))
                .Returns(value);

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Registers.Accumulator, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsCarry = true, Times.Once());

            stateMock.VerifySet(state => state.Flags.IsCarry = true, Times.Once());
        }

        [Fact]
        public void Value_WritesNegativeFlag()
        {
            const ushort address = 0b_0000_0001;

            const byte value = 0b_0100_0000;
            const byte accumulator = 0b_1000_0000;

            var stateMock = SetupMock(0x2F, accumulator, false);

            _ = stateMock
                .Setup(s => s.Memory.ReadAbsolute(address))
                .Returns(value);

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Registers.Accumulator, Times.Once());
            stateMock.VerifyGet(state => state.Flags.IsCarry, Times.Once());

            stateMock.VerifySet(state => state.Flags.IsNegative = true, Times.Once());
        }

        [Fact]
        public void Value_ReadsCarryFlag()
        {
            const ushort address = 0b_0000_0001;

            const byte value = 0b_0100_0000;
            const byte accumulator = 0b_1000_0001;
            const byte result = 0b_1000_0001;

            var stateMock = SetupMock(0x2F, accumulator, true);

            _ = stateMock
                .Setup(s => s.Memory.ReadAbsolute(address))
                .Returns(value);

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Registers.Accumulator, Times.Once());
            stateMock.VerifyGet(state => state.Flags.IsCarry, Times.Once());

            stateMock.VerifySet(state => state.Flags.IsNegative = true, Times.Once());
            stateMock.VerifySet(state => state.Registers.Accumulator = result, Times.Once());
        }

        [Fact]
        public void Value_ReadWriteZeroPage()
        {
            const ushort address = 0b_0000_0001;

            const byte value = 0b_0000_0001;
            const byte accumulator = 0b_0000_0010;
            const byte result = 0b_0000_0010;

            var stateMock = SetupMock(0x27, accumulator, false);

            _ = stateMock
                .Setup(s => s.Memory.ReadZeroPage(address))
                .Returns(value);

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Memory.ReadZeroPage(address), Times.Once());
            stateMock.Verify(state => state.Memory.WriteZeroPage(address, result), Times.Once());
        }

        [Fact]
        public void Value_ReadWriteZeroPageX()
        {
            const ushort address = 0b_0000_0001;

            const byte value = 0b_0000_0001;
            const byte accumulator = 0b_0000_0010;
            const byte result = 0b_0000_0010;

            var stateMock = SetupMock(0x37, accumulator, false);

            _ = stateMock
                .Setup(s => s.Memory.ReadZeroPageX(address))
                .Returns(value);

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Memory.ReadZeroPageX(address), Times.Once());
            stateMock.Verify(state => state.Memory.WriteZeroPageX(address, result), Times.Once());
        }

        [Fact]
        public void Value_ReadWriteIndirectX()
        {
            const ushort address = 0b_0000_0001;

            const byte value = 0b_0000_0001;
            const byte accumulator = 0b_0000_0010;
            const byte result = 0b_0000_0010;

            var stateMock = SetupMock(0x23, accumulator, false);

            _ = stateMock
                .Setup(s => s.Memory.ReadIndirectX(address))
                .Returns(value);

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Memory.ReadIndirectX(address), Times.Once());
            stateMock.Verify(state => state.Memory.WriteIndirectX(address, result), Times.Once());
        }

        [Fact]
        public void Value_ReadWriteIndirectY()
        {
            const ushort address = 0b_0000_0001;

            const byte value = 0b_0000_0001;
            const byte accumulator = 0b_0000_0010;
            const byte result = 0b_0000_0010;

            var stateMock = SetupMock(0x33, accumulator, false);

            _ = stateMock
                .Setup(s => s.Memory.ReadIndirectY(address))
                .Returns(value);

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Memory.ReadIndirectY(address), Times.Once());
            stateMock.Verify(state => state.Memory.WriteIndirectY(address, result), Times.Once());
        }

        [Fact]
        public void Value_ReadWriteAbsolute()
        {
            const ushort address = 0b_0000_0001;

            const byte value = 0b_0000_0001;
            const byte accumulator = 0b_0000_0010;
            const byte result = 0b_0000_0010;

            var stateMock = SetupMock(0x2F, accumulator, false);

            _ = stateMock
                .Setup(s => s.Memory.ReadAbsolute(address))
                .Returns(value);

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Memory.ReadAbsolute(address), Times.Once());
            stateMock.Verify(state => state.Memory.WriteAbsolute(address, result), Times.Once());
        }

        [Fact]
        public void Value_ReadWriteAbsoluteX()
        {
            const ushort address = 0b_0000_0001;

            const byte value = 0b_0000_0001;
            const byte accumulator = 0b_0000_0010;
            const byte result = 0b_0000_0010;

            var stateMock = SetupMock(0x3F, accumulator, false);

            _ = stateMock
                .Setup(s => s.Memory.ReadAbsoluteX(address))
                .Returns(value);

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Memory.ReadAbsoluteX(address), Times.Once());
            stateMock.Verify(state => state.Memory.WriteAbsoluteX(address, result), Times.Once());
        }

        [Fact]
        public void Value_ReadWriteAbsoluteY()
        {
            const ushort address = 0b_0000_0001;

            const byte value = 0b_0000_0001;
            const byte accumulator = 0b_0000_0010;
            const byte result = 0b_0000_0010;

            var stateMock = SetupMock(0x3B, accumulator, false);

            _ = stateMock
                .Setup(s => s.Memory.ReadAbsoluteY(address))
                .Returns(value);

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Memory.ReadAbsoluteY(address), Times.Once());
            stateMock.Verify(state => state.Memory.WriteAbsoluteY(address, result), Times.Once());
        }

        private static Mock<ICpuState> SetupMock(byte opcode, byte accumulator, bool isCarry)
        {
            var stateMock = TestUtils.GenerateStateMock();

            _ = stateMock
                .Setup(s => s.ExecutingOpcode)
                .Returns(opcode);

            _ = stateMock
                .Setup(s => s.Registers.Accumulator)
                .Returns(accumulator);

            _ = stateMock
                .Setup(s => s.Flags.IsCarry)
                .Returns(isCarry);

            return stateMock;
        }
    }
}
