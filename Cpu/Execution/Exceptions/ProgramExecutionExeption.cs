using System;

namespace Cpu.Execution.Exceptions
{
    /// <summary>
    /// Represents errors that occur during a program execution.
    /// </summary>
    public sealed class ProgramExecutionExeption : Exception
    {
        /// <summary>
        /// Initializes a new instance of the Cpu.Execution.Exceptions.ProgramExecutionExeption class
        /// with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">Message about the program which caused the error</param>
        /// <param name="innerException">Exception which if the cause for the current exception</param>
        /// <see cref="Cpu.Instructions.Exceptions.UnknownOpcodeException"/>
        public ProgramExecutionExeption(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
