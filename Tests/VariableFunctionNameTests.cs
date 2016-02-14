using System;
using ExpressionInterpreter.Exceptions;
using ExpressionInterpreter.Functions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressionInterpreter.Tests
{
    [TestClass]
    public class VariableFunctionNameTests : InterpreterTests
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void VariableNameCollidesWithFunctionName()
        {
            Interpreter.AddVariable(nameof(Math.Pow).ToLower(), 2);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void FunctionNameCollidesWithFunctionName()
        {
            Interpreter.RegisterFunction(new NullaryFunction(nameof(Math.Min).ToLower(), () => 0));
        }

        [TestMethod]
        public void ReAddingVariableWithSameName()
        {
            const string variableName = "show";

            Interpreter.AddVariable(variableName, 1);
            Interpreter.AddVariable(variableName, 2);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void ReRegisteringFunctionWithSameName()
        {
            const string functionName = "show";

            Interpreter.RegisterFunction(new NullaryFunction(functionName, () => 0));
            Interpreter.RegisterFunction(new NullaryFunction(functionName, () => 1));
        }
    }
}