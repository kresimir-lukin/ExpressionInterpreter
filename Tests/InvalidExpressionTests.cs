using ExpressionInterpreter.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressionInterpreter.Tests
{
    [TestClass]
    public class InvalidExpressionTests : InterpreterTests
    {
        [TestMethod]
        [ExpectedException(typeof(InterpreterException))]
        public void UnclosedFunctionCall()
        {
            Interpreter.Evaluate("2 + pow(3, 8 + 16");
        }

        [TestMethod]
        [ExpectedException(typeof(InterpreterException))]
        public void MutipleConsecutiveOperators()
        {
            Interpreter.Evaluate("2 ++ 4 * 15");
        }

        [TestMethod]
        [ExpectedException(typeof(InterpreterException))]
        public void NonexistingFunction()
        {
            Interpreter.Evaluate("25 + show(15) - 10");
        }

        [TestMethod]
        [ExpectedException(typeof(InterpreterException))]
        public void NonexistingVariable()
        {
            Interpreter.Evaluate("2 + xyz");
        }
    }
}