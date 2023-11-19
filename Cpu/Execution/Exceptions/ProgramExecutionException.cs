namespace Cpu.Execution.Exceptions;

/// <summary>
/// Represents errors that occur during a program execution.
/// </summary>
/// <remarks>
/// Initializes a new instance of the Cpu.Execution.Exceptions.ProgramExecutionExeption class
/// with a specified error message and a reference to the inner exception that is the cause of this exception.
/// </remarks>
/// <see cref="Cpu.Instructions.Exceptions.UnknownOpcodeException"/>
public sealed class ProgramExecutionException(string message, Exception innerException) : Exception(message, innerException)
{
}
