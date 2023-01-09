using Cpu.Opcodes.Exceptions;
using Xunit;

namespace Test.Unit.Cpu.Opcodes.Exceptions
{
    public sealed record DuplicateOpcodeExceptionTest
    {
        [Fact]
        public void Instantiate_Opcode_IsSet()
        {
            const byte opcode = 0x01;

            var subject = new DuplicateOpcodeException(opcode);

            Assert.Equal(opcode, subject.UnknownOpcode);
            Assert.Equal("OP Code='0x0001' is duplicated", subject.Message);
        }
    }
}
