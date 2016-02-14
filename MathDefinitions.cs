using System;
using System.Collections.Generic;
using ExpressionInterpreter.Functions;

namespace ExpressionInterpreter
{
    internal class MathDefinitions
    {
        private static double Pi() => Math.PI;
        private static double E() => Math.E;
        private static double Negate(double x) => -x;

        private static readonly Func<double>[] NullaryFunctions =
        {
            Pi,
            E
        };

        private static readonly Func<double, double>[] UnaryFunctions =
        {
            Math.Abs,
            Math.Acos,
            Math.Asin,
            Math.Atan,
            Math.Ceiling,
            Math.Cos,
            Math.Cosh,
            Math.Exp,
            Math.Floor,
            Math.Log10,
            Math.Round,
            Math.Sin,
            Math.Sinh,
            Math.Sqrt,
            Math.Tan,
            Math.Tanh,
            Math.Truncate,
            Negate,
        };

        private static readonly Func<double, double, double>[] BinaryFunctions =
        {
            Math.Atan2,
            Math.Log,
            Math.Max,
            Math.Min,
            Math.Pow,
        };

        internal static IEnumerable<Function> GetFunctions()
        {
            foreach (var nullaryFunction in NullaryFunctions)
                yield return new NullaryFunction(nullaryFunction.Method.Name.ToLower(), nullaryFunction);

            foreach (var unaryFunction in UnaryFunctions)
                yield return new UnaryFunction(unaryFunction.Method.Name.ToLower(), unaryFunction);

            foreach (var binaryFunction in BinaryFunctions)
                yield return new BinaryFunction(binaryFunction.Method.Name.ToLower(), binaryFunction);
        }
    }
}