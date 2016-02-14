namespace ExpressionInterpreter.Exceptions
{
    public class InvalidNameException : InterpreterException
    {
        public InvalidNameException(string message) : base(message)
        {
        }
    }
}