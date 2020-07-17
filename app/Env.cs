using System;
using System.Collections.Generic;
using System.Text;

namespace app
{
    public class Env
    {
        public Dictionary<string, Value> Globals { get; private set; }

        private Env()
        {
        }

        public static Env Load(string programText)
        {
            var result = new Env()
            {
                Globals = new Dictionary<string, Value>(),
            };

            var lines = programText.Split("\n");
            foreach (var line in lines)
            {
                var tokens = Tokenize(line);
                var name = tokens[0];
                if (name[0] != ':' || tokens[1] != "=")
                {
                    throw new Exception("Malformed input");
                }

                var value = result.Parse(tokens, 2);
                result.Globals[name] = value;
            }

            return result;
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
                var token = expr[index++];
                return token switch
                {
                    "ap" =>
                        new Application
                        {
                            Func = DoParse(),
                            Argument = DoParse(),
                        },
                    string number when long.TryParse(number, out var numberValue) =>
                        new Integer { Val = numberValue },
                    string variable when variable[0] == ':' =>
                        new Variable
                        {
                            Env = this,
                            Name = variable,
                        },
                    "cons" => Builtins.Cons.Instance,
                    "nil" => Builtins.Nil.Instance,
                    "isnil" => Builtins.IsNil.Instance,
                    "neg" => Builtins.Neg.Instance,
                    "c" => Builtins.C.Instance,
                    "b" => Builtins.B.Instance,
                    "s" => Builtins.S.Instance,
                    "i" => Builtins.I.Instance,
                    "t" => Builtins.T.Instance,
                    "f" => Builtins.F.Instance,
                    "car" => Builtins.Car.Instance,
                    "cdr" => Builtins.Cdr.Instance,
                    "eq" => Builtins.Eq.Instance,
                    "lt" => Builtins.Eq.Instance,
                    "mul" => Builtins.Mul.Instance,
                    "div" => Builtins.Div.Instance,
                    "add" => Builtins.Add.Instance,
                    _ =>
                        throw new Exception($"Unknown token: {token}"),
                };
            }
        }

        private static string[] Tokenize(string line)
        {
            return line.Split(' ');
        }
    }
}
