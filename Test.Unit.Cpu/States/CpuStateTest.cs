using Cpu.Flags;
using Cpu.Memory;
using Cpu.Registers;
using Cpu.States;
using Moq;
using System;
using Xunit;

namespace Test.Unit.Cpu.States
{
    public sealed record CpuStateTest
    {
        #region Properties
        private CpuState Subject { get; }

        private Mock<IFlagManager> FlagMock { get; }

        private Mock<IStackManager> StackMock { get; }

        private Mock<IMemoryManager> MemoryMock { get; }

        private Mock<IRegisterManager> RegisterMock { get; }
        #endregion

        #region Constructors
        public CpuStateTest()
        {
            this.FlagMock = new Mock<IFlagManager>();
            this.StackMock = new Mock<IStackManager>();
            this.MemoryMock = new Mock<IMemoryManager>();
            this.RegisterMock = new Mock<IRegisterManager>();

            this.Subject = new CpuState(
                this.FlagMock.Object,
                this.StackMock.Object,
                this.MemoryMock.Object,
                this.RegisterMock.Object);
        }
        #endregion

        [Fact]
        public void ReadWrite_Opcode_Successful()
        {
            const byte opcode = 0x7F;
            const byte finalOpcode = 0x80;

            this.Subject.ExecutingOpcode = opcode;

            Assert.Equal(opcode, this.Subject.ExecutingOpcode);

            this.Subject.ExecutingOpcode++;
            Assert.Equal(finalOpcode, this.Subject.ExecutingOpcode);
        }

        [Fact]
        public void Property_RegisterManager_Exists()
        {
            Assert.NotNull(this.Subject.Registers);
        }

        [Fact]
        public void Property_MemoryManager_Exists()
        {
            Assert.NotNull(this.Subject.Memory);
        }

        [Fact]
        public void Property_StackManager_Exists()
        {
            Assert.NotNull(this.Subject.Stack);
        }

        [Fact]
        public void Property_FlagManager_Exists()
        {
            Assert.NotNull(this.Subject.Flags);
        }

        #region Save/Load
        [Fact]
        public void Save_Returns_CurrentState()
        {
            const byte flagState = 0b_0101_0101;

            var registerState = new byte[] {
                0b_0000_0001,
                0b_0000_0010,
                0b_0000_0100,
                0b_0000_1000,
                0b_0001_0000,
                0b_0010_0000,
            };

            var memoryState = new byte[ushort.MaxValue];
            memoryState[0] = 0b_0100_0000;

            var expected = new byte[ushort.MaxValue + 7];
            expected[0] = 0b_0101_0101;
            expected[1] = 0b_0000_0001;
            expected[2] = 0b_0000_0010;
            expected[3] = 0b_0000_0100;
            expected[4] = 0b_0000_1000;
            expected[5] = 0b_0001_0000;
            expected[6] = 0b_0010_0000;
            expected[7] = 0b_0100_0000;

            _ = this.FlagMock
                .Setup(mock => mock.Save())
                .Returns(flagState);

            _ = this.RegisterMock
                .Setup(mock => mock.Save())
                .Returns(registerState);

            _ = this.MemoryMock
                .Setup(mock => mock.Save())
                .Returns(memoryState);

            var result = this.Subject.Save();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Load_WritesMemory_NewState()
        {
            const byte flagState = 0b_0101_0101;

            var registerState = new byte[] {
                0b_0000_0001,
                0b_0000_0010,
                0b_0000_0100,
                0b_0000_1000,
                0b_0001_0000,
                0b_0010_0000,
            };

            var memoryState = new byte[ushort.MaxValue + 1];
            memoryState[0] = 0b_0100_0000;

            var expected = new byte[ushort.MaxValue + 8];
            expected[0] = 0b_0101_0101;
            expected[1] = 0b_0000_0001;
            expected[2] = 0b_0000_0010;
            expected[3] = 0b_0000_0100;
            expected[4] = 0b_0000_1000;
            expected[5] = 0b_0001_0000;
            expected[6] = 0b_0010_0000;
            expected[7] = 0b_0100_0000;

            this.Subject.Load(expected);

            this.FlagMock.Verify(mock => mock.Load(flagState), Times.Exactly(1));
            this.RegisterMock.Verify(mock => mock.Load(registerState), Times.Exactly(1));
            this.MemoryMock.Verify(mock => mock.Load(memoryState), Times.Exactly(1));
        }

        [Fact]
        public void Load_Null_Throws()
        {
            _ = Assert.Throws<ArgumentNullException>(() => this.Subject.Load(null));
        }

        [Theory]
        [InlineData(ushort.MaxValue + 8 + 1)]
        [InlineData(ushort.MaxValue + 8 - 1)]
        public void Load_WrongLength_Throws(int length)
        {
            var memory = new byte[length];

            _ = Assert.Throws<ArgumentOutOfRangeException>(() => this.Subject.Load(memory));
        }
        #endregion
    }
}
