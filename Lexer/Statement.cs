using System;
using System.Collections.Generic;
using System.Reflection;

using Newtonsoft.Json;

namespace Lexer
{
    [Serializable]
    public class Statement : IExecutable
    {
        [JsonIgnore]
        public Block Parent { get; set; }

        public string Type => GetType().Name;

        public void Execute() => ExecuteStatement();

        public virtual void ExecuteStatement() { }

        public static List<Type> StatementTypes;
        static Statement()
        {
            StatementTypes = new List<Type>();
            var getTypes = new[] { typeof(List<Token>), typeof(int), typeof(int) };
            var s = typeof(Statement);

            foreach (var t in Assembly.GetExecutingAssembly().GetTypes())
            {
                var m = t.GetMethod("Get", getTypes);
                if (s.IsAssignableFrom(t) && 
                    m is not null && 
                    m.IsStatic) 
                { 
                    StatementTypes.Add(t); 
                }
            }
        }
    }
}
