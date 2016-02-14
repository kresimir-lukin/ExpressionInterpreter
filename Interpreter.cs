using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using ExpressionInterpreter.Exceptions;
using ExpressionInterpreter.Functions;
using ExpressionInterpreter.Parser;

namespace ExpressionInterpreter
{
    public class Interpreter
    {
        protected Dictionary<string, double> Variables;
        protected Dictionary<string, Function> Functions;

        public Interpreter()
        {
            Variables = new Dictionary<string, double>();
            Functions = MathDefinitions.GetFunctions().ToDictionary(x => x.Name, x => x);
        }

        protected void CheckNameClushWithFunction(string name)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));
            Contract.Requires<InvalidNameException>(!Functions.ContainsKey(name), "Name clushes with function!");
        }

        public void AddVariable(string name, double value)
        {
            CheckNameClushWithFunction(name);

            Variables[name] = value;
        }

        public void RegisterFunction(Function function)
        {
            Contract.Requires(function != null);
            CheckNameClushWithFunction(function.Name);

            Functions.Add(function.Name, function);
        }

        public double Evaluate(string expression)
        {
            var tokenParser = new TokenParser(Variables, Functions);
            var result = tokenParser.ParseExecute(expression);

            return result;
        }
    }
}