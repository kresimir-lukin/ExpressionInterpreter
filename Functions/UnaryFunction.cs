using System;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ExpressionInterpreter.Functions
{
    public class UnaryFunction : Function
    {
        protected readonly Func<double, double> Function;

        public UnaryFunction(string name, Func<double, double> function)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));
            Contract.Requires(function != null);

            Name = name;
            Function = function;
        }

        public override string Name { get; }
        public override uint NumberOfArguments => 1;

        public override double Calculate(params double[] parameters)
        {
            Contract.Requires(parameters.Length == NumberOfArguments);

            return Function(parameters.First());
        }
    }
}