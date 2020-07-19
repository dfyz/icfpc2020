using System;
using System.Collections.Generic;

namespace app
{
    class Program
    {
        static void Main(string[] args)
        {
            var mode = args[0];
            var payload = args[1];
            if (mode == "mod") {
                Console.WriteLine(Sender.BitsToString(Modem.Modulate(ParseData(payload))));
            } else if (mode == "dem") {
                Console.WriteLine(Modem.Demodulate(Sender.StringToBits(payload)));
            } else {
                throw new Exception($"Unknown mode {mode}");
            }
        }

        private static Value ParseData(string data) {
            var stack = new Stack<Value>();
            var idx = 0;
            while (idx < data.Length) {
                var ch = data[idx];
                if (char.IsWhiteSpace(ch)) {
                    ++idx;
                    continue;
                }

                if (ch == '(') {
                    stack.Push(new Pair{});
                    ++idx;
                } else if (ch == ')') {
                    var cdr = stack.Pop();
                    var car = stack.Pop();
                    var cons = (Pair)stack.Pop();
                    cons.First = car;
                    cons.Second = cdr;
                    stack.Push(cons);
                    ++idx;
                } else if (IsNumberChar(ch)) {
                    var num = 0L;
                    var sign = 1L;
                    while (idx < data.Length && IsNumberChar(data[idx])) {
                        if (data[idx] == '-') {
                            sign = -1L;
                        } else {
                            num = (num * 10 + (data[idx] - '0'));
                        }
                        ++idx;
                    }
                    num *= sign;
                    stack.Push(new Integer{Val = num});
                } else if (ch == 'n') {
                    if (ch + 2 >= data.Length || data[idx + 1] != 'i' || data[idx + 2] != 'l') {
                        throw new Exception("nil expected");
                    }
                    stack.Push(Builtins.Nil.Instance);
                    idx += 3;
                } else {
                    throw new Exception($"Unexpected {ch}");
                }
            }

            return stack.Pop();
        }

        private static bool IsNumberChar(char ch) {
            return ch == '-' || char.IsDigit(ch);
        }
    }
}
