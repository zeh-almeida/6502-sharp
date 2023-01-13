using Cpu.Instructions.Exceptions;
using Cpu.Instructions.Illegal;
using Cpu.States;
using Moq;
using Test.Unit.Cpu.Utils;
using Xunit;

namespace Test.Unit.Cpu.Instructions.Illegal
{
    public sealed record LoadAccumulatorXTest : IClassFixture<LoadAccumulatorX>
    {
        #region Properties
        private LoadAccumulatorX Subject { get; }
        #endregion

        #region Constructors
        public LoadAccumulatorXTest(LoadAccumulatorX subject)
        {
            this.Subject = subject;
        }
        #endregion

        [Theory]
        [InlineData(0xA7)]
        [InlineData(0xB7)]
        [InlineData(0xA3)]
        [InlineData(0xB3)]
        [InlineData(0xAF)]
        [InlineData(0xBF)]
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
        public void Execution_WritesZeroFlag()
        {
            const ushort address = 0b_0000_0001;
            const byte value = 0b_0000_0000;

            var stateMock = SetupMock(0xA3);

            _ = stateMock
                .Setup(s => s.Memory.ReadAbsolute(address))
                .Returns(value);

            this.Subject.Execute(stateMock.Object, address);

            stateMock.VerifySet(state => state.Flags.IsZero = true, Times.Once());

            stateMock.VerifySet(state => state.Registers.IndexX = value, Times.Once());
            stateMock.VerifySet(state => state.Registers.Accumulator = value, Times.Once());
        }

        [Fact]
        public void Execution_WritesNegativeFlag()
        {
            const ushort address = 0b_0000_0001;
            const byte value = 0b_1000_0110;

            var stateMock = SetupMock(0xAF);

            _ = stateMock
                .Setup(s => s.Memory.ReadAbsolute(address))
                .Returns(value);

            this.Subject.Execute(stateMock.Object, address);

            stateMock.VerifySet(state => state.Flags.IsNegative = true, Times.Once());

            stateMock.VerifySet(state => state.Registers.IndexX = value, Times.Once());
            stateMock.VerifySet(state => state.Registers.Accumulator = value, Times.Once());
        }

        [Fact]
        public void Execution_ReadZeroPage()
        {
            const ushort address = 0b_0000_0001;
            const byte value = 0b_0000_0010;

            var stateMock = SetupMock(0xA7);

            _ = stateMock
                .Setup(s => s.Memory.ReadZeroPage(address))
                .Returns(value);

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Memory.ReadZeroPage(address), Times.Once());

            stateMock.VerifySet(state => state.Registers.IndexX = value, Times.Once());
            stateMock.VerifySet(state => state.Registers.Accumulator = value, Times.Once());
        }

        [Fact]
        public void Execution_ReadZeroPageY()
        {
            const ushort address = 0b_0000_0001;
            const byte value = 0b_0000_0010;

            var stateMock = SetupMock(0xB7);

            _ = stateMock
                .Setup(s => s.Memory.ReadZeroPageY(address))
                .Returns(value);

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Memory.ReadZeroPageY(address), Times.Once());

            stateMock.VerifySet(state => state.Registers.IndexX = value, Times.Once());
            stateMock.VerifySet(state => state.Registers.Accumulator = value, Times.Once());
        }

        [Fact]
        public void Execution_ReadIndirectX()
        {
            const ushort address = 0b_0000_0001;
            const byte value = 0b_0000_0010;

            var stateMock = SetupMock(0xA3);

            _ = stateMock
                .Setup(s => s.Memory.ReadIndirectX(address))
                .Returns(value);

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Memory.ReadIndirectX(address), Times.Once());

            stateMock.VerifySet(state => state.Registers.IndexX = value, Times.Once());
            stateMock.VerifySet(state => state.Registers.Accumulator = value, Times.Once());
        }

        [Fact]
        public void Execution_ReadIndirectY()
        {
            const ushort address = 0b_0000_0001;
            const byte value = 0b_0000_0010;

            var stateMock = SetupMock(0xB3);

            _ = stateMock
                .Setup(s => s.Memory.ReadIndirectY(address))
                .Returns((false, value));

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Memory.ReadIndirectY(address), Times.Once());

            stateMock.VerifySet(state => state.Registers.IndexX = value, Times.Once());
            stateMock.VerifySet(state => state.Registers.Accumulator = value, Times.Once());
        }

        [Fact]
        public void Execution_ReadAbsolute()
        {
            const ushort address = 0b_0000_0001;
            const byte value = 0b_0000_0010;

            var stateMock = SetupMock(0xAF);

            _ = stateMock
                .Setup(s => s.Memory.ReadAbsolute(address))
                .Returns(value);

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Memory.ReadAbsolute(address), Times.Once());

            stateMock.VerifySet(state => state.Registers.IndexX = value, Times.Once());
            stateMock.VerifySet(state => state.Registers.Accumulator = value, Times.Once());
        }

        [Fact]
        public void Execution_ReadAbsoluteY()
        {
            const ushort address = 0b_0000_0001;
            const byte value = 0b_0000_0010;

            var stateMock = SetupMock(0xBF);

            _ = stateMock
                .Setup(s => s.Memory.ReadAbsoluteY(address))
                .Returns((false, value));

            this.Subject.Execute(stateMock.Object, address);

            stateMock.Verify(state => state.Memory.ReadAbsoluteY(address), Times.Once());

            stateMock.VerifySet(state => state.Registers.IndexX = value, Times.Once());
            stateMock.VerifySet(state => state.Registers.Accumulator = value, Times.Once());
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
