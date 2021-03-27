using System;
using System.Collections.Generic;

namespace Lexer
{
    [Serializable]
    public struct Token
    {
        public TokenType Type;
        public string Value;

        public Token(TokenType type, string value) 
        {
            Type = type;
            Value = value;
        }

        public override string ToString() =>
            $"{Type}{(Value is null || Value == "" ? "" : $" {Value}")}";

        public static List<Token> GetExpGroup(List<Token> tokens, out int numberCut)
        {
            numberCut = 0;
            if (tokens[0].Type != TokenType.OpenParenthesis)
            {
                return tokens;
            }

            var toReturn = new List<Token>();
            var depth = 1;
            for (var i = 0; tokens.Count > 0 && depth > 0; i++)
            {
                var t = tokens[0];
                tokens.RemoveAt(0);
                numberCut++;
                toReturn.Add(t);

                if (t.Type == TokenType.CloseParenthesis)
                {
                    depth--;
                    continue;
                }
                else if (t.Type == TokenType.OpenParenthesis && i != 0)
                {
                    depth++;
                    continue;
                }
            }
            toReturn.RemoveAt(0);
            toReturn.RemoveAt(toReturn.Count - 1);
            return toReturn;
        }

        public static int GetBlockGroup(List<Token> tokens, int index)
        {
            if (tokens[index].Type != TokenType.OpenBlock)
            {
                return tokens.Count - index;
            }

            var count = 0;
            var depth = 1;
            for (var i = index + 1; i < tokens.Count; i++)
            {
                count++;
                if (tokens[i].Type == TokenType.CloseBlock)
                {
                    depth--;
                    if (depth == 0) { break; }
                }
                else if (tokens[i].Type == TokenType.OpenBlock)
                {
                    depth++;
                }
            }

            return count - 1;
        }

        public static int GetStatementLength(List<Token> tokens, int index)
        {
            var count = 0;
            while (tokens[index].Type != TokenType.StatementEnding && index < tokens.Count)
            {
                index++;
                count++;
            }

            return count;
        }
    }
}
