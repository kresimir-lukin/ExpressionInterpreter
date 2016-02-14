namespace ExpressionInterpreter.Functions
{
    public abstract class Function
    {
        public abstract string Name { get; }
        public abstract uint NumberOfArguments { get; }

        public abstract double Calculate(params double[] parameters);
    }
}