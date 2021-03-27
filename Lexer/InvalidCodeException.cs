using System;

namespace Lexer
{
    public class InvalidCodeException : Exception
    {
        public string Code;
        public int Index;
        public int Line;

        public InvalidCodeException(string code, int index)
        {
            Code = code;

            var lineNumber = 0;
            while (code.IndexOf("\n") != -1)
            {
                var line = code.Substring(0, code.IndexOf("\n"));
                if (index > line.Length)
                {
                    index -= line.Length;
                    code = code[code.IndexOf("\n")..];
                    lineNumber++;
                    continue;
                }

                Line = lineNumber;
                Index = index;
            }
        }
    }
}
