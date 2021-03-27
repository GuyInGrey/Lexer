using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexer
{
    public static class Extensions
    {
        public static bool IsKeyword(this Token t, string keyword) =>
            t.Type == TokenType.Keyword && t.Value == keyword;
    }
}
