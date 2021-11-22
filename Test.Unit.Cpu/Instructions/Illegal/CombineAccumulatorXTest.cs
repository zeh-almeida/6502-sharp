using Cpu.Instructions.Exceptions;
using Cpu.Instructions.Illegal;
using Cpu.States;
using Moq;
using Test.Unit.Cpu.Utils;
using Xunit;

namespace Test.Unit.Cpu.Instructions.Illegal
{
    public sealed record CombineAccumulatorXTest : IClassFixture<CombineAccumulatorX>
    {
        #region Properties
        private CombineAccumulatorX Subject { get; }
        #endregion

        #region Constructors
        public CombineAccumulatorXTest(CombineAccumulatorX subject)
        {
            this.Subject = subject;
        }
        #endregion

        [Theory]
        [InlineData(0x87)]
        [InlineData(0x97)]
        [InlineData(0x8F)]
        [InlineData(0x83)]
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
            var stateMock = SetupMock(0x00, 0x00, 0x00);
            _ = Assert.Throws<UnknownOpcodeException>(() => this.Subject.Execute(stateMock.Object, 0));
        }

        [Fact]
        public void Value_ReadWriteZeroPage()
        {
            const ushort address = 0b_0000_0001;

            const byte registerX = 0b_0000_0001;
            const byte accumulator = 0b_0000_0001;
            const byte result = 0b_0000_0001;

            var stateMock = SetupMock(0x87, accumulator, registerX);

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Memory.WriteZeroPage(address, result), Times.Once());
        }

        [Fact]
        public void Value_ReadWriteZeroPageX()
        {
            const ushort address = 0b_0000_0001;

            const byte registerX = 0b_0000_0001;
            const byte accumulator = 0b_0000_0001;
            const byte result = 0b_0000_0001;

            var stateMock = SetupMock(0x97, accumulator, registerX);

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Memory.WriteZeroPageX(address, result), Times.Once());
        }

        [Fact]
        public void Value_ReadWriteIndirectX()
        {
            const ushort address = 0b_0000_0001;

            const byte registerX = 0b_0000_0001;
            const byte accumulator = 0b_0000_0001;
            const byte result = 0b_0000_0001;

            var stateMock = SetupMock(0x83, accumulator, registerX);

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Memory.WriteIndirectX(address, result), Times.Once());
        }

        [Fact]
        public void Value_ReadWriteAbsolute()
        {
            const ushort address = 0b_0000_0001;

            const byte registerX = 0b_0000_0001;
            const byte accumulator = 0b_0000_0001;
            const byte result = 0b_0000_0001;

            var stateMock = SetupMock(0x8F, accumulator, registerX);

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Memory.WriteAbsolute(address, result), Times.Once());
        }

        private static Mock<ICpuState> SetupMock(byte opcode, byte accumulator, byte registerX)
        {
            var stateMock = TestUtils.GenerateStateMock();

            _ = stateMock
                .Setup(s => s.ExecutingOpcode)
                .Returns(opcode);

            _ = stateMock
                .Setup(s => s.Registers.Accumulator)
                .Returns(accumulator);

            _ = stateMock
                .Setup(s => s.Registers.IndexX)
                .Returns(registerX);

            return stateMock;
        }
    }
}
