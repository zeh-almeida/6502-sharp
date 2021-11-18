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
        public void PushPull_Stack_Successful()
        {
            const byte value = 1;
            const byte pointer = 2;
            const byte finalPointer = 3;
            const ushort memoryPointer = 0x0102;

            _ = this.MemoryMock
                .Setup(mock => mock.ReadAbsolute(memoryPointer))
                .Returns(value);

            _ = this.RegisterMock
                .SetupSequence(mock => mock.StackPointer)
                .Returns(pointer)
                .Returns(finalPointer);

            this.Subject.Push(value);
            var result = this.Subject.Pull();

            Assert.Equal(value, result);

            this.MemoryMock.Verify(mock => mock.WriteAbsolute(memoryPointer, value), Times.Once());
            this.RegisterMock.VerifySet(mock => mock.StackPointer = finalPointer, Times.Once());

            this.MemoryMock.Verify(mock => mock.ReadAbsolute(memoryPointer), Times.Once());
            this.RegisterMock.VerifySet(mock => mock.StackPointer = pointer, Times.Once());
        }

        [Fact]
        public void PushPull_OverflowStack_Successful()
        {
            const byte value = 1;
            const byte pointer = 255;
            const byte finalPointer = 0;
            const ushort memoryPointer = 0x01FF;

            _ = this.MemoryMock
                .Setup(mock => mock.ReadAbsolute(memoryPointer))
                .Returns(value);

            _ = this.RegisterMock
                .SetupSequence(mock => mock.StackPointer)
                .Returns(pointer)
                .Returns(finalPointer);

            this.Subject.Push(value);
            var result = this.Subject.Pull();

            Assert.Equal(value, result);

            this.MemoryMock.Verify(mock => mock.WriteAbsolute(memoryPointer, value), Times.Once());
            this.RegisterMock.VerifySet(mock => mock.StackPointer = finalPointer, Times.Once());

            this.MemoryMock.Verify(mock => mock.ReadAbsolute(memoryPointer), Times.Once());
            this.RegisterMock.VerifySet(mock => mock.StackPointer = pointer, Times.Once());
        }

        [Fact]
        public void PushPull16_Stack_Successful()
        {
            const byte valueMsb = 0b_1111_1111;
            const byte valueLsb = 0b_0000_0010;
            const ushort value = 0b_1111_1111_0000_0010;

            const byte pointerMsb = 2;
            const byte pointerLsb = 3;

            const ushort memoryPointerMsb = 0x0102;
            const ushort memoryPointerLsb = 0x0103;

            const byte afterPushPointer = 4;
            const byte afterPullPointer = 2;

            _ = this.MemoryMock
                .Setup(mock => mock.ReadAbsolute(memoryPointerMsb))
                .Returns(valueMsb);

            _ = this.MemoryMock
                .Setup(mock => mock.ReadAbsolute(memoryPointerLsb))
                .Returns(valueLsb);

            _ = this.RegisterMock
                .SetupSequence(mock => mock.StackPointer)
                .Returns(pointerMsb)
                .Returns(pointerLsb)
                .Returns(afterPushPointer)
                .Returns(pointerLsb)
                .Returns(pointerMsb);

            this.Subject.Push16(value);
            var result = this.Subject.Pull16();

            Assert.Equal(value, result);

            this.MemoryMock.Verify(mock => mock.WriteAbsolute(memoryPointerMsb, valueMsb), Times.Once());
            this.MemoryMock.Verify(mock => mock.WriteAbsolute(memoryPointerLsb, valueLsb), Times.Once());
            this.RegisterMock.VerifySet(mock => mock.StackPointer = afterPushPointer, Times.Once());

            this.MemoryMock.Verify(mock => mock.ReadAbsolute(memoryPointerMsb), Times.Once());
            this.MemoryMock.Verify(mock => mock.ReadAbsolute(memoryPointerLsb), Times.Once());
            this.RegisterMock.VerifySet(mock => mock.StackPointer = afterPullPointer, Times.Once());
        }
    }
}
