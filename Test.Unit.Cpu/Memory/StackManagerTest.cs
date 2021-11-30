using Cpu.Memory;
using Cpu.Registers;
using Moq;
using Xunit;

namespace Test.Unit.Cpu.Memory
{
    public sealed record StackManagerTest
    {
        #region Properties
        private Mock<IMemoryManager> MemoryMock { get; }

        private Mock<IRegisterManager> RegisterMock { get; }

        private StackManager Subject { get; }
        #endregion

        #region Constructors
        public StackManagerTest()
        {
            this.MemoryMock = new Mock<IMemoryManager>();
            this.RegisterMock = new Mock<IRegisterManager>();

            this.Subject = new StackManager(this.MemoryMock.Object, this.RegisterMock.Object);
        }
        #endregion

        [Fact]
        public void Push_Executes()
        {
            const byte value = 1;
            const byte pointer = 0x02;
            const byte finalPointer = 0x01;
            const ushort memoryPointer = 0x0102;

            _ = this.RegisterMock
                .SetupSequence(mock => mock.StackPointer)
                .Returns(pointer)
                .Returns(pointer);

            this.Subject.Push(value);

            this.MemoryMock.Verify(mock => mock.WriteAbsolute(memoryPointer, value), Times.Once());
            this.RegisterMock.VerifySet(mock => mock.StackPointer = finalPointer, Times.Once());
        }

        [Fact]
        public void Pull_Executes()
        {
            const byte value = 1;
            const byte pointer = 0x01;
            const byte finalPointer = 0x02;
            const ushort memoryPointer = 0x0102;

            _ = this.MemoryMock
                .Setup(mock => mock.ReadAbsolute(memoryPointer))
                .Returns(value);

            _ = this.RegisterMock
                .Setup(mock => mock.StackPointer)
                .Returns(pointer);

            var result = this.Subject.Pull();

            this.MemoryMock.Verify(mock => mock.ReadAbsolute(memoryPointer), Times.Once());
            this.RegisterMock.VerifySet(mock => mock.StackPointer = finalPointer, Times.Once());

            Assert.Equal(value, result);
        }

        [Fact]
        public void Push_Overflow_Executes()
        {
            const byte value = 1;
            const byte pointer = 0x00;
            const byte finalPointer = 0xFF;
            const ushort memoryPointer = 0x0100;

            _ = this.RegisterMock
                .SetupSequence(mock => mock.StackPointer)
                .Returns(pointer)
                .Returns(pointer);

            this.Subject.Push(value);

            this.MemoryMock.Verify(mock => mock.WriteAbsolute(memoryPointer, value), Times.Once());
            this.RegisterMock.VerifySet(mock => mock.StackPointer = finalPointer, Times.Once());
        }

        [Fact]
        public void Pull_Overflow_Executes()
        {
            const byte value = 1;
            const byte pointer = 0xFF;
            const byte finalPointer = 0x00;
            const ushort memoryPointer = 0x0100;

            _ = this.MemoryMock
                .Setup(mock => mock.ReadAbsolute(memoryPointer))
                .Returns(value);

            _ = this.RegisterMock
                .Setup(mock => mock.StackPointer)
                .Returns(pointer);

            var result = this.Subject.Pull();

            this.MemoryMock.Verify(mock => mock.ReadAbsolute(memoryPointer), Times.Once());
            this.RegisterMock.VerifySet(mock => mock.StackPointer = finalPointer, Times.Once());

            Assert.Equal(value, result);
        }

        [Fact]
        public void Push16_Executes()
        {
            const ushort value = 0xFF00;
            const byte valueLsb = 0x00;
            const byte valueMsb = 0xFF;

            const byte pointerLsb = 0x02;
            const byte pointerMsb = 0x03;

            const byte finalPointer = 0x01;

            const ushort memoryLsb = 0x0102;
            const ushort memoryMsb = 0x0103;

            _ = this.RegisterMock
                .SetupSequence(mock => mock.StackPointer)
                .Returns(pointerMsb)
                .Returns(pointerLsb);

            this.Subject.Push16(value);

            this.MemoryMock.Verify(mock => mock.WriteAbsolute(memoryMsb, valueMsb), Times.Once());
            this.MemoryMock.Verify(mock => mock.WriteAbsolute(memoryLsb, valueLsb), Times.Once());

            this.RegisterMock.VerifySet(mock => mock.StackPointer = finalPointer, Times.Once());
        }

        [Fact]
        public void Pull16_Executes()
        {
            const ushort value = 0xFF00;
            const byte valueLsb = 0x00;
            const byte valueMsb = 0xFF;

            const byte pointerLsb = 0x02;
            const byte pointerMsb = 0x03;

            const byte initialPointer = 0x01;

            const ushort memoryLsb = 0x0102;
            const ushort memoryMsb = 0x0103;

            _ = this.MemoryMock
                .Setup(mock => mock.ReadAbsolute(memoryMsb))
                .Returns(valueMsb);

            _ = this.MemoryMock
                .Setup(mock => mock.ReadAbsolute(memoryLsb))
                .Returns(valueLsb);

            _ = this.RegisterMock
                .SetupSequence(mock => mock.StackPointer)
                .Returns(initialPointer)
                .Returns(pointerLsb)
                .Returns(pointerMsb);

            var result = this.Subject.Pull16();

            this.MemoryMock.Verify(mock => mock.ReadAbsolute(memoryLsb), Times.Once());
            this.MemoryMock.Verify(mock => mock.ReadAbsolute(memoryMsb), Times.Once());

            this.RegisterMock.VerifySet(mock => mock.StackPointer = pointerMsb, Times.Once());

            Assert.Equal(value, result);
        }
    }
}
