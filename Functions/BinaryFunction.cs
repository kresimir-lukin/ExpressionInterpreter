using System;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ExpressionInterpreter.Functions
{
    public class BinaryFunction : Function
    {
        protected readonly Func<double, double, double> Function;

        public BinaryFunction(string name, Func<double, double, double> function)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));
            Contract.Requires(function != null);

            Name = name;
            Function = function;
        }

        public override string Name { get; }
        public override uint NumberOfArguments => 2;

        public override double Calculate(params double[] parameters)
        {
            Contract.Requires(parameters.Length == NumberOfArguments);

            return Function(parameters.First(), parameters.Last());
        }
    }
}