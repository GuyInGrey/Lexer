using System;
using System.Collections.Generic;

namespace Lexer.Statements
{
    [Serializable]
    public class VariableAssignment : Statement
    {
        public string Identifier;
        public Expression Expression;

        public static VariableAssignment Get(List<Token> t, int index, int count)
        {
            var sub = t.GetRange(index, count);

            if (!(sub[0].IsKeyword("var") || 
                sub[1].Type == TokenType.Identifier ||
                sub[2].Type == TokenType.Assignment))
            {
                return null;
            }

            if (count - 3 <= 0) { return null; }

            return new VariableAssignment()
            {
                Identifier = sub[1].Value,
                Expression = Expression.Parse(t, index + 3, count - 3),
            };
        }
    }
}
