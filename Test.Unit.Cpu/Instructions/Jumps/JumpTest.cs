using Cpu.Instructions.Exceptions;
using Cpu.Instructions.Jumps;
using Cpu.States;
using Moq;
using Test.Unit.Cpu.Utils;
using Xunit;

namespace Test.Unit.Cpu.Instructions.Jumps
{
    public sealed record JumpTest
    {
        #region Properties
        private Jump Subject { get; }
        #endregion

        #region Constructors
        public JumpTest()
        {
            this.Subject = new Jump();
        }
        #endregion

        [Theory]
        [InlineData(0x4C)]
        [InlineData(0x6C)]
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
        public void Execute_ImmediateValue_WritesProgramCounter()
        {
            const ushort value = 1;
            var stateMock = SetupMock(0x4C);

            this.Subject.Execute(stateMock.Object, value);

            stateMock.VerifySet(state => state.Registers.ProgramCounter = value, Times.Once());
        }

        [Fact]
        public void Execute_AbsoluteValue_WritesProgramCounter()
        {
            const ushort address = 1;
            const byte value = 1;
            var stateMock = SetupMock(0x6C);

            _ = stateMock
                .Setup(state => state.Memory.ReadAbsolute(address))
                .Returns(value);

            this.Subject.Execute(stateMock.Object, address);

            stateMock.VerifySet(state => state.Registers.ProgramCounter = value, Times.Once());
        }

        private static Mock<ICpuState> SetupMock(byte opcode)
        {
            var stateMock = TestUtils.GenerateStateMock();

            _ = stateMock
                .Setup(state => state.ExecutingOpcode)
                .Returns(opcode);

            return stateMock;
        }
    }
}
