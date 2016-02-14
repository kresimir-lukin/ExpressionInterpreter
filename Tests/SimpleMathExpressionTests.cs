using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressionInterpreter.Tests
{
    [TestClass]
    public class SimpleMathExpressionTests : InterpreterTests
    {
        private const double FirstOperator = 5;
        private const double SecondOperator = 3;

        [TestMethod]
        public void SimpleAddition()
        {
            AreEqual(FirstOperator + SecondOperator, Interpreter.Evaluate($"{FirstOperator} + {SecondOperator}"));
        }

        [TestMethod]
        public void SimpleSubstraction()
        {
            AreEqual(FirstOperator - SecondOperator, Interpreter.Evaluate($"{FirstOperator} - {SecondOperator}"));
        }

        [TestMethod]
        public void SimpleMultipication()
        {
            AreEqual(FirstOperator * SecondOperator, Interpreter.Evaluate($"{FirstOperator} * {SecondOperator}"));
        }

        [TestMethod]
        public void SimpleDivision()
        {
            AreEqual(FirstOperator / SecondOperator, Interpreter.Evaluate($"{FirstOperator} / {SecondOperator}"));
        }

        [TestMethod]
        public void ExpressionWithVariable()
        {
            Interpreter.AddVariable("x", 9);
            AreEqual(4 + Math.Pow(9, 2), Interpreter.Evaluate("4 + pow(x, 2)"));
        }

        [TestMethod]
        public void ExpressionWithAdditionPriority()
        {
            AreEqual(9 * (3 + 2), Interpreter.Evaluate("9 * (3 + 2)"));
        }

        [TestMethod]
        public void ExpressionWithoutPriority()
        {
            AreEqual(9 * 3 + 2, Interpreter.Evaluate("9 * 3 + 2"));
        }
    }
}