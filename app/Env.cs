using System;
using System.Collections.Generic;
using System.Text;

namespace app
{
    public class Env
    {
        public Dictionary<string, Value> Globals { get; set; }

        public void Load(string programText)
        {
            var lines = programText.Split("\n");
            foreach (var line in lines)
            {
                var tokens = Tokenize(line);
                var name = tokens[0];
                if (name[0] != ':' || tokens[1] != "=")
                {
                    throw new Exception("Malformed input");
                }
            }
        }

        public Value Eval(string expression)
        {
            var tokens = Tokenize(expression);
            return Parse(tokens, 0).Force();
        }

        private Value Parse(string[] expr, int start)
        {
            int index = start;
            return DoParse();

            Value DoParse()
            {
                return null;
            }
        }

        private string[] Tokenize(string line)
        {
            return line.Split(' ');
        }
    }
}
