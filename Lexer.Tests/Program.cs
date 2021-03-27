using System;
using System.Linq;

using Newtonsoft.Json;

namespace Lexer.Tests
{
    class Program
    {
        static void Main()
        {
            var testCode = @"
var i = 1;
var testText = """";
loop (i < 10)
{
  print(i);
  i = i + 1;
  testText = testText + ""a"";
}
";

            var testCode2 = @"
var i = 1 + 3 * 4;
{ var a = i * 3;}
var b = 4; ; ;;; { };
";

            var tokens = Tokenizer.Tokenize(testCode2);
            var b = Block.Parse(tokens, 0, tokens.Count);

            Console.WriteLine(JsonConvert.SerializeObject(b, Formatting.Indented, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            }));

            Console.Read();
        }
    }
}
