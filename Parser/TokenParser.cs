using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ExpressionInterpreter.Exceptions;
using ExpressionInterpreter.Functions;

namespace ExpressionInterpreter.Parser
{
    internal class TokenParser
    {
        protected Dictionary<string, double> Variables;
        protected Dictionary<string, Function> Functions;
        protected char DecimalSeparator;
        private TokenParserState _parserState;

        internal TokenParser(Dictionary<string, double> variables, Dictionary<string, Function> functions)
        {
            Variables = variables;
            Functions = functions;
            DecimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.First();
        }

        internal double ParseExecute(string expression)
        {
            _parserState = new TokenParserState(expression);
            _parserState.LookaheadToken = GetNextToken();

            var result = ParseExpression();

            if (_parserState.LookaheadToken != TokenType.End)
                throw new InterpreterException("Token end expected");

            return result;
        }

        protected TokenType GetNextToken()
        {
            _parserState.Lexeme = string.Empty;

            while (_parserState.CommandIndex < _parserState.Command.Length)
            {
                var ch = _parserState.Command[_parserState.CommandIndex++];
                if (ch == Constants.CWhitespace || ch == Constants.CTab)
                    continue;

                if (ch >= Constants.CZero && ch <= Constants.CNine || 
                    (ch == Constants.CMinus && _parserState.Command[_parserState.CommandIndex] >= Constants.CZero 
                                            && _parserState.Command[_parserState.CommandIndex] <= Constants.CNine 
                                            && _parserState.LookaheadToken == TokenType.Operator))
                {
                    if (ch == Constants.CMinus)
                    {
                        _parserState.Lexeme += Constants.CMinus;
                        ch = _parserState.Command[_parserState.CommandIndex];
                        if (_parserState.CommandIndex != _parserState.Command.Length)
                            _parserState.CommandIndex++;
                    }

                    while (ch >= Constants.CZero && ch <= Constants.CNine)
                    {
                        _parserState.Lexeme += ch;

                        if (_parserState.CommandIndex >= _parserState.Command.Length)
                        {
                            _parserState.CommandIndex++;
                            break;
                        }

                        ch = _parserState.Command[_parserState.CommandIndex++];
                    }

                    _parserState.CommandIndex--;
                    if (ch == DecimalSeparator)
                    {
                        _parserState.Lexeme += DecimalSeparator;
                        ch = _parserState.Command[++_parserState.CommandIndex];

                        while (ch >= Constants.CZero && ch <= Constants.CNine)
                        {
                            _parserState.Lexeme += ch;

                            _parserState.CommandIndex++;
                            if (_parserState.CommandIndex >= _parserState.Command.Length)
                            {
                                _parserState.CommandIndex++;
                                break;
                            }

                            ch = _parserState.Command[_parserState.CommandIndex];
                        }
                    }

                    return TokenType.Number;
                }

                if ((ch >= Constants.Ca && ch <= Constants.Cz) || (ch >= Constants.CA && ch <= Constants.CZ))
                {
                    while ((ch >= Constants.Ca && ch <= Constants.Cz) || (ch >= Constants.CA && ch <= Constants.CZ))
                    {
                        _parserState.Lexeme += ch;

                        if (_parserState.CommandIndex >= _parserState.Command.Length)
                        {
                            _parserState.CommandIndex++;
                            break;
                        }

                        ch = _parserState.Command[_parserState.CommandIndex++];
                    }
                    _parserState.CommandIndex--;

                    return Functions.ContainsKey(_parserState.Lexeme) 
                        ? TokenType.Function 
                        : TokenType.Variable;
                }

                if (ch == Constants.CPlus || ch == Constants.CMinus || ch == Constants.CMultiply || ch == Constants.CDivide || 
                    ch == Constants.CLeftParenthesis || ch == Constants.CRightParenthesis || ch == Constants.CArgumentSeparator)
                {
                    _parserState.Lexeme = ch.ToString();
                    return TokenType.Operator;
                }

                return TokenType.Error;
            }

            return TokenType.End;
        }

        protected void Match(TokenType token)
        {
            if (_parserState.LookaheadToken != token)
                throw new InterpreterException($"Token {token} expected");

            _parserState.LookaheadToken = GetNextToken();
        }

        private void Match(TokenType token, string lexeme)
        {
            if (_parserState.LookaheadToken != token || _parserState.Lexeme != lexeme)
                throw new InterpreterException($"Token {token} with {lexeme} expected");

            _parserState.LookaheadToken = GetNextToken();
        }

        protected double ParseExpression()
        {
            var value = ParseMember();

            if (_parserState.LookaheadToken == TokenType.Operator && _parserState.Lexeme == Constants.Plus)
            {
                Match(TokenType.Operator, Constants.Plus);
                value += ParseExpression();
            }

            if (_parserState.LookaheadToken == TokenType.Operator && _parserState.Lexeme == Constants.Minus)
            {
                Match(TokenType.Operator, Constants.Minus);
                value -= ParseExpression();
            }

            return value;
        }

        private double ParseMember()
        {
            var value = ParseFactor();

            if (_parserState.LookaheadToken == TokenType.Operator && _parserState.Lexeme == Constants.Multiply)
            {
                Match(TokenType.Operator, Constants.Multiply);
                value *= ParseMember();
            }

            if (_parserState.LookaheadToken == TokenType.Operator && _parserState.Lexeme == Constants.Divide)
            {
                Match(TokenType.Operator, Constants.Divide);

                var divisor = ParseMember();
                if (divisor == 0)
                    throw new DivideByZeroException();

                value /= divisor;
            }

            return value;
        }

        private double ParseFactor()
        {
            double value;

            if (_parserState.LookaheadToken == TokenType.Number)
            {
                value = double.Parse(_parserState.Lexeme);
                _parserState.LookaheadToken = GetNextToken();
            }
            else if (_parserState.LookaheadToken == TokenType.Operator && _parserState.Lexeme == Constants.LeftParenthesis)
            {
                Match(TokenType.Operator, Constants.LeftParenthesis);
                value = ParseExpression();
                Match(TokenType.Operator, Constants.RightParenthesis);
            }
            else if (_parserState.LookaheadToken == TokenType.Function)
            {
                var function = Functions[_parserState.Lexeme];

                Match(TokenType.Function);
                Match(TokenType.Operator, Constants.LeftParenthesis);

                var parameters = new double[function.NumberOfArguments];

                for (var i = 1; i <= function.NumberOfArguments; i++)
                {
                    parameters[i - 1] = ParseExpression();

                    if (i != function.NumberOfArguments)
                        Match(TokenType.Operator, Constants.ArgumentSeparator);
                }

                value = function.Calculate(parameters);
                Match(TokenType.Operator, Constants.RightParenthesis);
            }
            else if (_parserState.LookaheadToken == TokenType.Variable)
            {
                if (!Variables.ContainsKey(_parserState.Lexeme))
                    throw new InterpreterException("Variable missing");

                value = Variables[_parserState.Lexeme];
                Match(TokenType.Variable);
            }
            else
                throw new InterpreterException("Factor missing");

            return value;
        }
    }
}