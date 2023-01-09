using Cpu.Opcodes.Exceptions;
using Xunit;

namespace Test.Unit.Cpu.Opcodes.Exceptions
{
    public sealed record MisconfiguredOpcodeExceptionTest
    {
        [Fact]
        public void Instantiate_Opcode_IsSet()
        {
            const string opcode = "TEST";

            var subject = new MisconfiguredOpcodeException(opcode);

            Assert.Equal(opcode, subject.OpcodeName);
            Assert.Equal("Opcode configuration='TEST' is malformed", subject.Message);
        }
    }
}
