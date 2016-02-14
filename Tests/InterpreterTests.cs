using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressionInterpreter.Tests
{
    public class InterpreterTests
    {
        protected readonly Interpreter Interpreter = new Interpreter();
        protected readonly double Delta = 0.00000001;

        protected void AreEqual(double expected, double actual)
        {
            Assert.AreEqual(expected, actual, Delta);
        }
    }
}