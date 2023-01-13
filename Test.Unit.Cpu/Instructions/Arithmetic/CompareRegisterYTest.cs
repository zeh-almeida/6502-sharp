using Cpu.Instructions.Arithmetic;
using Cpu.Instructions.Exceptions;
using Cpu.States;
using Moq;
using Test.Unit.Cpu.Utils;
using Xunit;

namespace Test.Unit.Cpu.Instructions.Arithmetic
{
    public sealed record CompareRegisterYTest : IClassFixture<CompareRegisterY>
    {
        #region Properties
        private CompareRegisterY Subject { get; }
        #endregion

        #region Constructors
        public CompareRegisterYTest(CompareRegisterY subject)
        {
            this.Subject = subject;
        }
        #endregion

        [Theory]
        [InlineData(0xC0)]
        [InlineData(0xC4)]
        [InlineData(0xCC)]
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
            const byte registerY = 0b_0000_0000;

            var stateMock = SetupMock(0xC0, registerY);

            this.Subject.Execute(stateMock.Object, value);

            stateMock.VerifySet(state => state.Flags.IsZero = true, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsNegative = false, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsCarry = true, Times.Once());
        }

        [Fact]
        public void Execute_ResultSeventhBitSet_WritesNegativeFlag()
        {
            const byte value = 0b_0000_0001;
            const byte registerY = 0b_1000_0001;

            var stateMock = SetupMock(0xC0, registerY);

            this.Subject.Execute(stateMock.Object, value);

            stateMock.VerifySet(state => state.Flags.IsZero = false, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsNegative = true, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsCarry = true, Times.Once());
        }

        [Fact]
        public void Execute_ValueLessEqualToAccumulator_WritesCarryFlag()
        {
            const byte value = 0b_0000_0001;
            const byte registerY = 0b_0000_0001;

            var stateMock = SetupMock(0xC0, registerY);

            this.Subject.Execute(stateMock.Object, value);

            stateMock.VerifySet(state => state.Flags.IsZero = false, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsNegative = false, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsCarry = true, Times.Once());
        }

        [Fact]
        public void Execute_Immediate_Compares()
        {
            const byte value = 0b_0000_0011;
            const byte registerY = 0b_0000_0001;

            var stateMock = SetupMock(0xC0, registerY);

            this.Subject.Execute(stateMock.Object, value);

            stateMock.Verify(state => state.Memory.ReadZeroPage(It.IsAny<ushort>()), Times.Never());
            stateMock.Verify(state => state.Memory.ReadAbsolute(It.IsAny<ushort>()), Times.Never());
        }

        [Fact]
        public void Execute_ZeroPage_Compares()
        {
            const ushort address = 2;

            const byte value = 0b_0000_0011;
            const byte registerY = 0b_0000_0001;

            var stateMock = SetupMock(0xC4, registerY);

            _ = stateMock
                .Setup(s => s.Memory.ReadZeroPage(address))
                .Returns(value);

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Memory.ReadZeroPage(address), Times.Once());
        }

        [Fact]
        public void Execute_Absolute_Compares()
        {
            const ushort address = 2;

            const byte value = 0b_0000_0011;
            const byte registerY = 0b_0000_0001;

            var stateMock = SetupMock(0xCC, registerY);

            _ = stateMock
                .Setup(s => s.Memory.ReadAbsolute(address))
                .Returns(value);

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Memory.ReadAbsolute(address), Times.Once());
        }

        private static Mock<ICpuState> SetupMock(byte opcode, byte registerY)
        {
            var stateMock = TestUtils.GenerateStateMock();

            _ = stateMock
                .Setup(s => s.ExecutingOpcode)
                .Returns(opcode);

            _ = stateMock
                .Setup(s => s.Registers.IndexY)
                .Returns(registerY);

            return stateMock;
        }
    }
}
