using System.Collections.Generic;
using System.Linq;

namespace Lexer
{
    public static class Tokenizer
    {
        public static readonly string[] Operators = new[]
        { "+", "-", "*", "/", "%", "&", "|", "^", "==", ">", "<", ">=", "<=", "!=", "*>", "<*", };

        public static readonly string[] Keywords = new[]
        { "var", "loop", "function", };

        public static readonly string AlphaNumeric = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

        public static List<Token> Tokenize(string code)
        {
            var workingCode = code;
            workingCode = workingCode.Trim();
            var toReturn = new List<Token>();

            var previous = workingCode;
            while (workingCode.Length > 0)
            {
                toReturn.Add(GetNext(workingCode, out workingCode));
                workingCode = workingCode.TrimStart();
                if (previous == workingCode)
                {
                    throw new InvalidCodeException(code, code.Length - workingCode.Length);
                }
            }

            return toReturn;
        }

        public static Token GetNext(string code, out string newCode)
        {
            newCode = code[1..];

            if (code.StartsWith("(")) { return new Token(TokenType.OpenParenthesis, ""); }
            if (code.StartsWith(")")) { return new Token(TokenType.CloseParenthesis, ""); }
            if (code.StartsWith("{")) { return new Token(TokenType.OpenBlock, ""); }
            if (code.StartsWith("}")) { return new Token(TokenType.CloseBlock, ""); }
            if (code.StartsWith("=")) { return new Token(TokenType.Assignment, ""); }

            var k = Keywords.FirstOrDefault(i => code.StartsWith(i + " ") || code.StartsWith(i + "("));
            if (k is not null)
            {
                newCode = code[(k.Length + 1)..];
                return new Token(TokenType.Keyword, k);
            }

            var s = GetStringValue(code, out var newStringCode);
            if (newStringCode is not null)
            {
                newCode = newStringCode;
                return new Token(TokenType.String, s);
            }

            var n = GetNumberValue(code, out var newNumberCode);
            if (newNumberCode is not null)
            {
                newCode = newNumberCode;
                return new Token(TokenType.Number, n.ToString());
            }

            var b = GetBooleanValue(code, out var newBoolCode);
            if (newBoolCode is not null)
            {
                newCode = newBoolCode;
                return new Token(TokenType.Boolean, b.ToString().ToLower());
            }

            if (code.StartsWith(";"))
            {
                newCode = code[1..];
                return new Token(TokenType.StatementEnding, "");
            }

            var op = Operators.FirstOrDefault(i => code.StartsWith(i));
            if (op is not null)
            {
                newCode = code[op.Length..];
                return new Token(TokenType.Operator, op);
            }

            var objectName = GetObjectName(code, out newCode);
            return new Token(TokenType.Identifier, objectName);
        }

        public static string GetStringValue(string code, out string newCode)
        {
            if (!code.StartsWith('"'))
            {
                newCode = null;
                return "";
            }
            code = code[1..];

            var value = "";
            for (var i = 0; i < code.Length; i++)
            {
                if (code[i] == '"' && (i == 0 || code[i-1] != '\\'))
                {
                    newCode = code[(value.Length + 1)..];
                    return value;
                }

                value += code[i];
            }

            newCode = null;
            return null;
        }

        public static float GetNumberValue(string code, out string newCode)
        {
            var num = float.NaN;
            newCode = code;

            for (var i = 0; i < code.Length; i++)
            {
                if (float.TryParse(code.Substring(0, i + 1), out var result))
                {
                    newCode = newCode[1..];
                    num = result;
                }
                else { break; }
            }

            if (float.IsNaN(num))
            {
                newCode = null;
            }
            return num;
        }

        public static bool GetBooleanValue(string code, out string newCode)
        {
            if (code.StartsWith("true") && !AlphaNumeric.Contains(code[4..][0]))
            {
                newCode = code[4..];
                return true;
            }

            if (code.StartsWith("false") && !AlphaNumeric.Contains(code[5..][0]))
            {
                newCode = code[5..];
                return false;
            }

            newCode = null;
            return false;
        }

        public static string GetObjectName(string code, out string newCode)
        {
            while (!AlphaNumeric.Contains(code[0]))
            {
                code = code[1..];
            }

            var name = "";
            while (code.Length > 0 && AlphaNumeric.Contains(code[0]))
            {
                name += code[0];
                code = code[1..];
            }

            newCode = code;
            return name;
        }
    }
}
