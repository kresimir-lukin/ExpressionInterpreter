using System;
using System.Diagnostics.Contracts;

namespace ExpressionInterpreter.Functions
{
    public class NullaryFunction : Function
    {
        protected readonly Func<double> Function;

        public NullaryFunction(string name, Func<double> function)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));
            Contract.Requires(function != null);

            Name = name;
            Function = function;
        }

        public override string Name { get; }
        public override uint NumberOfArguments => 0;

        public override double Calculate(params double[] parameters)
        {
            Contract.Requires(parameters.Length == NumberOfArguments);

            return Function();
        }
    }
}