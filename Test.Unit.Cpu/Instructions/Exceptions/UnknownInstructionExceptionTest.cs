using Cpu.Instructions.Exceptions;
using Xunit;

namespace Test.Unit.Cpu.Instructions.Exceptions;

public sealed record UnknownInstructionExceptionTest
{
    [Fact]
    public void Instantiate_Opcode_IsSet()
    {
        const string opcode = "TEST";

        var subject = new UnknownInstructionException(opcode);

        Assert.Equal(opcode, subject.InstructionName);
    }

    [Fact]
    public void Instantiate_Message_IsSet()
    {
        const string opcode = "TEST";
        const string message = "ERROR";

        var subject = new UnknownInstructionException(opcode, message);

        Assert.Equal(opcode, subject.InstructionName);
        Assert.Equal($"{message}, Instruction=TEST", subject.Message);
    }
}
