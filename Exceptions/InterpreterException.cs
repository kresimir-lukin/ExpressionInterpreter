using System;

namespace ExpressionInterpreter.Exceptions
{
    public class InterpreterException : Exception
    {
        public InterpreterException(string message) : base(message)
        {
        }
    }
}