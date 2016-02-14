namespace ExpressionInterpreter.Parser
{
    internal class TokenParserState
    {
        internal TokenType LookaheadToken;
        internal string Lexeme;
        internal string Command;
        internal int CommandIndex;

        internal TokenParserState(string command)
        {
            Command = command;
            CommandIndex = 0;
        }
    }
}