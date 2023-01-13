﻿using Cpu.Instructions.Branches;
using Cpu.States;
using Moq;
using Test.Unit.Cpu.Utils;
using Xunit;

namespace Test.Unit.Cpu.Instructions.Branches
{
    public sealed record BranchNegativeTest : IClassFixture<BranchNegative>
    {
        #region Properties
        private BranchNegative Subject { get; }
        #endregion

        #region Constructors
        public BranchNegativeTest(BranchNegative subject)
        {
            this.Subject = subject;
        }
        #endregion

        [Theory]
        [InlineData(0x30)]
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
        public void Execute_FlagIsTrue_WritesProgramCounter()
        {
            const ushort value = 248;
            const ushort counter = 8;
            const ushort result = 0;

            var stateMock = SetupMock(true, counter);

            this.Subject.Execute(stateMock.Object, value);

            stateMock.Verify(state => state.Registers.ProgramCounter, Times.Once());
            stateMock.VerifySet(state => state.Registers.ProgramCounter = result, Times.Once());
        }

        [Fact]
        public void Execute_FlagIsFalse_NoOp()
        {
            const ushort value = 1;
            const ushort counter = 0;

            var stateMock = SetupMock(false, counter);

            this.Subject.Execute(stateMock.Object, value);

            stateMock.Verify(state => state.Registers.ProgramCounter, Times.Never());
            stateMock.VerifySet(state => state.Registers.ProgramCounter = It.IsAny<ushort>(), Times.Never());
        }

        private static Mock<ICpuState> SetupMock(bool isFlag, ushort counter)
        {
            var stateMock = TestUtils.GenerateStateMock();

            _ = stateMock
                .Setup(state => state.Flags.IsNegative)
                .Returns(isFlag);

            _ = stateMock
                .Setup(state => state.Registers.ProgramCounter)
                .Returns(counter);

            return stateMock;
        }
    }
}
