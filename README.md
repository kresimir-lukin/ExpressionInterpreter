# ExpressionInterpreter
Simple LL1 parser implemented in C# for evaluating expressions with functions and variables support

Usage
------

```csharp
var interpreter = new Interpreter();

interpreter.RegisterFunction(new UnaryFunction("square", x => x * x));
interpreter.AddVariable("a", 3);
interpreter.AddVariable("b", 4);

var c = interpreter.Evaluate("sqrt(square(a) + square(b))");
```
