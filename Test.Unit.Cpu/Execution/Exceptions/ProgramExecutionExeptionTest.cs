using Cpu.Execution.Exceptions;
using Xunit;

namespace Test.Unit.Cpu.Execution.Exceptions;
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type. - Necessary for null tests

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

#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
