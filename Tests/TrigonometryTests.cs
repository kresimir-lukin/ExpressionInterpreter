using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressionInterpreter.Tests
{
    [TestClass]
    public class TrigonometryTests : InterpreterTests
    {
        [TestMethod]
        public void TriangleArea()
        {
            var a = 3.2;
            var b = 4;
            var c = 5;
            var semiperimeter = (a + b + c) / 2;

            Interpreter.AddVariable("semiperimeter", semiperimeter);
            Interpreter.AddVariable("a", a);
            Interpreter.AddVariable("b", b);
            Interpreter.AddVariable("c", c);

            AreEqual(Math.Sqrt(semiperimeter * (semiperimeter - a) * (semiperimeter - b) * (semiperimeter - c)), 
                Interpreter.Evaluate("sqrt(semiperimeter * (semiperimeter - a) * (semiperimeter - b) * (semiperimeter - c))"));
        }

        [TestMethod]
        public void Pitagora()
        {
            Interpreter.AddVariable("a", 3);
            Interpreter.AddVariable("b", 4);

            AreEqual(5, Interpreter.Evaluate("sqrt(pow(a, 2) + pow(b, 2))"));
        }

        [TestMethod]
        public void OddEvenIdentities()
        {
            Interpreter.AddVariable("x", 1.2423);

            AreEqual(Interpreter.Evaluate("sin(negate(x))"), Interpreter.Evaluate("negate(sin(x))"));
            AreEqual(Interpreter.Evaluate("cos(negate(x))"), Interpreter.Evaluate("cos(x)"));
            AreEqual(Interpreter.Evaluate("tan(negate(x))"), Interpreter.Evaluate("negate(tan(x))"));
        }
    }
}