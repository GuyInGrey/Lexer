using System;

using Newtonsoft.Json;

namespace Lexer
{
    public interface IExecutable
    {
        public void Execute();

        [JsonIgnore]
        public Block Parent { get; set; }
    }
}
