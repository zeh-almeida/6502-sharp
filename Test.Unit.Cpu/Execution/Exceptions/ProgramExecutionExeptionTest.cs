using Cpu.Execution.Exceptions;
using Xunit;

namespace Test.Unit.Cpu.Execution.Exceptions
{
    public sealed record ProgramExecutionExeptionTest
    {
        [Fact]
        public void Instantiate_Message_IsSet()
        {
            const string message = "ERROR";

            var subject = new ProgramExecutionExeption(message, null);

            Assert.Equal(message, subject.Message);
        }
        [Fact]
        public void Instantiate_InnerException_IsSet()
        {
            var exception = new NullReferenceException();

            var subject = new ProgramExecutionExeption(null, exception);

            Assert.Equal(exception, subject.InnerException);
        }
    }
}
