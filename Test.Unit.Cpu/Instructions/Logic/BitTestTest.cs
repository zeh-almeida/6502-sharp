using Cpu.Instructions.Exceptions;
using Cpu.Instructions.Logic;
using Cpu.States;
using Moq;
using Test.Unit.Cpu.Utils;
using Xunit;

namespace Test.Unit.Cpu.Instructions.Logic
{
    public sealed record BitTestTest : IClassFixture<BitTest>
    {
        #region Properties
        private BitTest Subject { get; }
        #endregion

        #region Constructors
        public BitTestTest(BitTest subject)
        {
            this.Subject = subject;
        }
        #endregion

        [Theory]
        [InlineData(0x24)]
        [InlineData(0x2C)]
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
            const ushort value = 0b_0000_0011;
            const byte accumulator = 0b_0000_1100;

            var stateMock = SetupMock(0x2C, accumulator);

            this.Subject.Execute(stateMock.Object, value);

            stateMock.Verify(state => state.Registers.Accumulator, Times.Once());
            stateMock.Verify(state => state.Memory.ReadZeroPage(It.IsAny<ushort>()), Times.Never());

            stateMock.VerifySet(state => state.Flags.IsZero = true, Times.Once());
        }

        [Fact]
        public void Execute_ValueSixthBitSet_WritesOverflowFlag()
        {
            const ushort value = 0b_0100_0011;
            const byte accumulator = 0b_0000_0011;

            var stateMock = SetupMock(0x2C, accumulator);

            this.Subject.Execute(stateMock.Object, value);

            stateMock.Verify(state => state.Registers.Accumulator, Times.Once());
            stateMock.Verify(state => state.Memory.ReadZeroPage(It.IsAny<ushort>()), Times.Never());

            stateMock.VerifySet(state => state.Flags.IsOverflow = true, Times.Once());
        }

        [Fact]
        public void Execute_ValueSeventhBitSet_WritesCarryFlag()
        {
            const ushort value = 0b_1000_0011;
            const byte accumulator = 0b_0000_0011;

            var stateMock = SetupMock(0x2C, accumulator);

            this.Subject.Execute(stateMock.Object, value);

            stateMock.Verify(state => state.Registers.Accumulator, Times.Once());
            stateMock.Verify(state => state.Memory.ReadZeroPage(It.IsAny<ushort>()), Times.Never());

            stateMock.VerifySet(state => state.Flags.IsNegative = true, Times.Once());
        }

        [Fact]
        public void Execute_ZeroPageAddress_Compares()
        {
            const byte value = 1;
            const ushort address = 2;
            const byte accumulator = 3;

            var stateMock = SetupMock(0x24, accumulator);

            _ = stateMock
                .Setup(s => s.Memory.ReadZeroPage(address))
                .Returns(value);

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Registers.Accumulator, Times.Once());
            stateMock.Verify(state => state.Memory.ReadZeroPage(address), Times.Once());
        }

        [Fact]
        public void Execute_Absolute_Compares()
        {
            const byte value = 1;

            var stateMock = SetupMock(0x2C, value);

            this.Subject.Execute(stateMock.Object, value);

            stateMock.Verify(state => state.Registers.Accumulator, Times.Once());
            stateMock.Verify(state => state.Memory.ReadZeroPage(It.IsAny<ushort>()), Times.Never());
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
