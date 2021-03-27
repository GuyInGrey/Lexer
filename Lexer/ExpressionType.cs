using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Lexer
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ExpressionType
    {
        String,
        Number,
        Boolean,
        Identifier,
        FunctionCall,

        Multiple,
        Invalid,
    }
}
