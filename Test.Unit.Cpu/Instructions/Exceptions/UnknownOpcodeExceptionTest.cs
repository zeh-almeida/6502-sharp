using Cpu.Instructions.Exceptions;
using Xunit;

namespace Test.Unit.Cpu.Instructions.Exceptions
{
    public sealed record UnknownOpcodeExceptionTest
    {
        [Fact]
        public void Instantiate_Opcode_IsSet()
        {
            const byte opcode = 0x01;

            var subject = new UnknownOpcodeException(opcode);

            Assert.Equal(opcode, subject.UnknownOpcode);
        }

        [Fact]
        public void Instantiate_Message_IsSet()
        {
            const byte opcode = 0x01;
            const string message = "ERROR";

            var subject = new UnknownOpcodeException(opcode, message);

            Assert.Equal(opcode, subject.UnknownOpcode);
            Assert.Equal(message, subject.Message);
        }
    }
}
