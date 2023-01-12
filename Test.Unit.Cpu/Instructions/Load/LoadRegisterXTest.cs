using Cpu.Instructions.Exceptions;
using Cpu.Instructions.Load;
using Cpu.States;
using Moq;
using Test.Unit.Cpu.Utils;
using Xunit;

namespace Test.Unit.Cpu.Instructions.Load
{
    public sealed record LoadRegisterXTest : IClassFixture<LoadRegisterX>
    {
        #region Properties
        private LoadRegisterX Subject { get; }
        #endregion

        #region Constructors
        public LoadRegisterXTest(LoadRegisterX subject)
        {
            this.Subject = subject;
        }
        #endregion

        [Theory]
        [InlineData(0xA2)]
        [InlineData(0xA6)]
        [InlineData(0xB6)]
        [InlineData(0xAE)]
        [InlineData(0xBE)]
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
        public void Execute_Equals_WritesZeroFlag()
        {
            const byte value = 0b_0000_0000;
            const byte result = 0b_0000_0000;

            var stateMock = SetupMock(0xA2);

            this.Subject.Execute(stateMock.Object, value);

            stateMock.VerifySet(state => state.Registers.IndexX = result, Times.Once());

            stateMock.VerifySet(state => state.Flags.IsZero = true, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsNegative = false, Times.Once());
        }

        [Fact]
        public void Execute_ResultSeventhBitSet_WritesNegativeFlag()
        {
            const byte value = 0b_1000_0011;
            const byte result = 0b_1000_0011;

            var stateMock = SetupMock(0xA2);

            this.Subject.Execute(stateMock.Object, value);

            stateMock.VerifySet(state => state.Registers.IndexX = result, Times.Once());

            stateMock.VerifySet(state => state.Flags.IsZero = false, Times.Once());
            stateMock.VerifySet(state => state.Flags.IsNegative = true, Times.Once());
        }

        [Fact]
        public void Execute_Immediate_Compares()
        {
            const byte value = 0b_0000_0011;
            const byte result = 0b_0000_0011;

            var stateMock = SetupMock(0xA2);

            this.Subject.Execute(stateMock.Object, value);

            stateMock.VerifySet(state => state.Registers.IndexX = result, Times.Once());
        }

        [Fact]
        public void Execute_ZeroPage_Compares()
        {
            const ushort address = 2;

            const byte value = 0b_0000_0011;
            const byte result = 0b_0000_0011;

            var stateMock = SetupMock(0xA6);

            _ = stateMock
                .Setup(s => s.Memory.ReadZeroPage(address))
                .Returns(value);

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Memory.ReadZeroPage(address), Times.Once());

            stateMock.VerifySet(state => state.Registers.IndexX = result, Times.Once());
        }

        [Fact]
        public void Execute_ZeroPageY_Compares()
        {
            const ushort address = 2;

            const byte value = 0b_0000_0011;
            const byte result = 0b_0000_0011;

            var stateMock = SetupMock(0xB6);

            _ = stateMock
                .Setup(s => s.Memory.ReadZeroPageY(address))
                .Returns(value);

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Memory.ReadZeroPageY(address), Times.Once());
            stateMock.VerifySet(state => state.Registers.IndexX = result, Times.Once());
        }

        [Fact]
        public void Execute_Absolute_Compares()
        {
            const ushort address = 2;

            const byte value = 0b_0000_0011;
            const byte result = 0b_0000_0011;

            var stateMock = SetupMock(0xAE);

            _ = stateMock
                .Setup(s => s.Memory.ReadAbsolute(address))
                .Returns(value);

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Memory.ReadAbsolute(address), Times.Once());

            stateMock.VerifySet(state => state.Registers.IndexX = result, Times.Once());
        }

        [Fact]
        public void Execute_AbsoluteY_Compares()
        {
            const ushort address = 2;

            const byte value = 0b_0000_0011;
            const byte result = 0b_0000_0011;

            var stateMock = SetupMock(0xBE);

            _ = stateMock
                .Setup(s => s.Memory.ReadAbsoluteY(address))
                .Returns((false, value));

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Memory.ReadAbsoluteY(address), Times.Once());
            stateMock.VerifySet(state => state.Registers.IndexX = result, Times.Once());
        }

        private static Mock<ICpuState> SetupMock(byte opcode)
        {
            var stateMock = TestUtils.GenerateStateMock();

            _ = stateMock
                .Setup(s => s.ExecutingOpcode)
                .Returns(opcode);

            return stateMock;
        }
    }
}
