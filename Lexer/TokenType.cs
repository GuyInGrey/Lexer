using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Lexer
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TokenType
    {
        String,
        Number,
        Boolean,

        Operator,
        Assignment,

        Keyword,

        OpenParenthesis,
        CloseParenthesis,
        OpenBlock,
        CloseBlock,
        StatementEnding,

        Identifier,
    }
}
