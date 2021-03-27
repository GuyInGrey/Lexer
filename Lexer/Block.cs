using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace Lexer
{
    [Serializable]
    public class Block : IExecutable
    {
        [JsonIgnore]
        public Block Parent { get; set; }

        public List<IExecutable> Children = new();

        public void Execute() => Children.ForEach(c => c.Execute());

        public static Block Parse(List<Token> tokens, int index, int count)
        {
            var startingIndex = index;
            var root = new Block();

            while (index < startingIndex + count)
            {
                int length;
                if (tokens[index].Type == TokenType.OpenBlock)
                {
                    length = Token.GetBlockGroup(tokens, index);
                    var b = Parse(tokens, index + 1, length);
                    b.Parent = root;
                    index += length + 2;
                    if (b.Children.Count == 0) { continue; }
                    root.Children.Add(b);
                    continue;
                }

                length = Token.GetStatementLength(tokens, index);
                if (length == 0)
                {
                    index++;
                    continue;
                }
                var s = GetNext(tokens, index, length);
                s.Parent = root;
                root.Children.Add(s);
                index += length + 1;
            }

            return root;
        }

        private static Statement GetNext(List<Token> tokens, int index, int length)
        {
            foreach (var t in Statement.StatementTypes)
            {
                var m = t.GetMethod("Get");
                var r = m?.Invoke(null, new object[] { tokens, index, length });
                if (r is null) { continue; }
                return (Statement)r;
            }

            throw new InvalidCodeException("", 0);
        }
    }
}
