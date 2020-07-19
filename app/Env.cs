#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;

namespace app
{
    public class Env
    {
        public Dictionary<string, Value> Globals { get; } = new Dictionary<string, Value>();

        public static Env Load(string programText, params string[] extra)
        {
            var result = new Env();
            var lines = programText.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            lines = lines.Concat(extra).ToArray();

            // small cheat: put entries for all vars declared in program into globals beforehand
            // this allows us to complain about unknown vars at compile time
            foreach (var line in lines)
            {
                var tokens = Tokenize(line);
                result.Globals.Add(tokens[0], Value.Nil);
            }

            foreach (var line in lines)
            {
                var tokens = Tokenize(line);
                var name = tokens[0];
                if (tokens[1] != "=")
                {
                    throw new Exception("Malformed input");
                }

                var value = result.Parse(tokens, 2);
                result.Globals[name] = value;
            }

            return result;
        }

        public Value Eval(string expression, params Value[] args)
        {
            for (var i = 0; i < args.Length; ++i)
            {
                Globals["$" + (i + 1).ToString()] = args[i];
            }

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
                    "cons" => Builtins.Cons.Instance,
                    "vec" => Builtins.Cons.Instance,
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
                    "lt" => Builtins.Lt.Instance,
                    "mul" => Builtins.Mul.Instance,
                    "div" => Builtins.Div.Instance,
                    "add" => Builtins.Add.Instance,
                    "inc" => Builtins.Inc.Instance,
                    "dec" => Builtins.Dec.Instance,
                    "draw" => Builtins.Draw.Instance,
                    "multipledraw" => Builtins.MultipleDraw.Instance,
                    "send" => Builtins.Send.Instance,
                    "interact" => Builtins.Interact.Instance,
                    string number when long.TryParse(number, out var numberValue) =>
                        new Integer { Val = numberValue },
                    "(" => ParseList(),
                    string variable =>
                        // everything else is a variable
                        new Symbol
                        {
                            Env = this,
                            Name = Globals.ContainsKey(variable)
                                ? variable
                                : throw new Exception($"Unknown variable: {variable}"),
                        },
                    _ =>
                        throw new Exception($"Unknown token: {token}"),
                };
            }

            Value ParseList()
            {
                var head = new Pair();
                var current = head;
                while (expr[index] != ")")
                {
                    var next = new Pair { First = DoParse() };
                    current.Second = next;
                    current = next;

                    // hacky
                    if (expr[index] == ",")
                    {
                        ++index;
                    }
                }

                ++index;
                current.Second = Value.Nil;
                return head.Second;
            }
        }

        private static string[] Tokenize(string line)
        {
            return line.Split(' ');
        }
    }
}
