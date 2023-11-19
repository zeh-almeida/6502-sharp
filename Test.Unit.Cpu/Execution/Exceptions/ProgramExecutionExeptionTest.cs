using Cpu.Execution.Exceptions;
using Xunit;

namespace Test.Unit.Cpu.Execution.Exceptions;

public sealed record ProgramExecutionExeptionTest
{
    [Fact]
    public void Instantiate_Message_IsSet()
    {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type. - Necessary for null tests
        const string message = "ERROR";
        var subject = new ProgramExecutionException(message, null);

        Assert.Equal(message, subject.Message);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    }

    [Fact]
    public void Instantiate_InnerException_IsSet()
    {
#pragma warning disable CA2201 // Do not raise reserved exception types - testing inner exception, no real usage
        var exception = new Exception();
        var subject = new ProgramExecutionException(null, exception);

        Assert.Equal(exception, subject.InnerException);
#pragma warning restore CA2201 // Do not raise reserved exception types
    }
}