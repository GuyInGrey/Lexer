using System;
using System.Collections.Generic;
using System.Linq;

namespace Lexer
{
    [Serializable]
    public class Expression
    {
        public ExpressionType Type;

        public string Content;

        // These 3 are only used if Type == Multiple
        public Expression Left;
        public string Operator;
        public Expression Right;

        public override string ToString() => ToString(0);

        public string ToString(int depth) 
        {
            var space = new string(' ', depth * 2);

            if (Type == ExpressionType.Multiple)
            {
                var left = Left.ToString(depth + 1);
                var right = Right.ToString(depth + 1);
                return $"{left}\n{space}{Operator}\n{right}";
            }
            else if (Type == ExpressionType.String)
            {
                return $"{space}\"{Content}\"";
            }
            else
            {
                return $"{space}{Content.ToLower()}";
            }
        }

        public static Expression Parse(List<Token> tokens, int index, int length)
        {
            if (length == 1)
            {
                return new Expression()
                {
                    Type =
                    tokens[index].Type == TokenType.String ? ExpressionType.String :
                    tokens[index].Type == TokenType.Boolean ? ExpressionType.Boolean :
                    tokens[index].Type == TokenType.Number ? ExpressionType.Number :
                    tokens[index].Type == TokenType.Identifier ? ExpressionType.Identifier : ExpressionType.Invalid,
                    Content = tokens[index].Value,
                };
            }

            var toReturn = new Expression()
            {
                Type = ExpressionType.Multiple,
            };

            if (tokens[index].Type == TokenType.OpenParenthesis)
            {
                var leftList = Token.GetExpGroup(tokens.GetRange(index, tokens.Count - index), out var cut);
                toReturn.Left = Parse(tokens, index + 1, leftList.Count);
                index += cut;
                length -= cut;
            }
            else
            {
                toReturn.Left = Parse(tokens, index, 1);
                index++;
                length--;
            }

            if (tokens[index].Type != TokenType.Operator)
            {
                return new Expression() { Type = ExpressionType.Invalid, };
            }

            toReturn.Operator = tokens[index].Value;
            index++;
            length--;

            if (tokens[index].Type == TokenType.OpenParenthesis)
            {
                var rightList = Token.GetExpGroup(tokens.GetRange(index, tokens.Count - index), out _);
                toReturn.Right = Parse(tokens, index + 1, rightList.Count);
            }
            else
            {
                toReturn.Right = Parse(tokens, index, length);
            }

            return toReturn;
        }
    }
}
