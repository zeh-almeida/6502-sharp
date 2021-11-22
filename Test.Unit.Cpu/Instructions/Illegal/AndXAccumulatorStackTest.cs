using Cpu.Instructions.Exceptions;
using Cpu.Instructions.Illegal;
using Cpu.States;
using Moq;
using Test.Unit.Cpu.Utils;
using Xunit;

namespace Test.Unit.Cpu.Instructions.Illegal
{
    public sealed record AndXAccumulatorStackTest : IClassFixture<AndXAccumulatorStack>
    {
        #region Properties
        private AndXAccumulatorStack Subject { get; }
        #endregion

        #region Constructors
        public AndXAccumulatorStackTest(AndXAccumulatorStack subject)
        {
            this.Subject = subject;
        }
        #endregion

        [Theory]
        [InlineData(0x9B)]
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

        [Theory]
        [InlineData(0x4041, 0x20, 0xEA, 0x20, 0x00)]
        [InlineData(0x4041, 0xFF, 0xEA, 0xEA, 0x40)]
        [InlineData(0x4041, 0xFF, 0xFF, 0xFF, 0x41)]
        [InlineData(0x0040, 0x20, 0xEA, 0x20, 0x00)]
        [InlineData(0x4040, 0xFF, 0xEA, 0xEA, 0x40)]
        public void Value_WriteAbsoluteY(ushort address, byte registerX, byte accumulator, byte stackResult, byte memoryResult)
        {
            var stateMock = SetupMock(accumulator, registerX);

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Registers.IndexX, Times.Once());
            stateMock.Verify(state => state.Registers.Accumulator, Times.Once());

            stateMock.VerifySet(state => state.Registers.StackPointer = stackResult, Times.Once());
            stateMock.Verify(state => state.Memory.WriteAbsoluteY(address, memoryResult), Times.Once());
        }

        private static Mock<ICpuState> SetupMock(byte accumulator, byte registerX)
        {
            var stateMock = TestUtils.GenerateStateMock();

            _ = stateMock
                .Setup(s => s.ExecutingOpcode)
                .Returns(0x9B);

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
