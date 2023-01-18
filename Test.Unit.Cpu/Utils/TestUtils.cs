using Cpu.Flags;
using Cpu.Memory;
using Cpu.Registers;
using Cpu.States;
using Moq;

namespace Test.Unit.Cpu.Utils;

public static class TestUtils
{
    public static Mock<ICpuState> GenerateStateMock()
    {
        var stateMock = new Mock<ICpuState>();

        _ = stateMock
            .SetupAllProperties();

        _ = stateMock
            .Setup(s => s.Flags)
            .Returns(Mock.Of<IFlagManager>());

        _ = stateMock
            .Setup(s => s.Registers)
            .Returns(Mock.Of<IRegisterManager>());

        _ = stateMock
            .Setup(s => s.Memory)
            .Returns(Mock.Of<IMemoryManager>());

        _ = stateMock
            .Setup(s => s.Stack)
            .Returns(Mock.Of<IStackManager>());

        return stateMock;
    }
}
