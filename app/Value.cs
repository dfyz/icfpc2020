using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

#nullable enable

namespace app
{
    public abstract class Value
    {
        public static Value Nil => Builtins.Nil.Instance;

        public virtual Value Force() => this;
    }

    public abstract class FuncValue : Value
    {
        public abstract Value Apply(Value argument);
    }

    public class Integer : Value
    {
        public long Val { get; set; }

        public override string ToString() => Val.ToString();
    }

    public class Pair : Builtins.Func1Value<Pair>
    {
        public Value First { get; set; }
        public Value Second { get; set; }

        public override string ToString() => $"({First.Force()} {Second.Force()})";

        public override Value Apply(Value x) =>
            new Application {
                Func = new Application {
                    Func = x,
                    Argument = First,
                },
                Argument = Second,
            };
    }

    public static class ValueExtensions
    {
        public static Value GetFirst(this Value pair) => ((Pair) pair.Force()).First.Force();
        public static Value GetSecond(this Value pair) => ((Pair) pair.Force()).Second.Force();
        public static long GetInt(this Value integer) => ((Integer) integer.Force()).Val;
    }

    public class Variable : Value
    {
        private Value value;

        // TODO(lazy): switch to integer if strings are too slow
        public string Name { get; set; }
        public Env Env { get; set; }

        public override Value Force()
        {
            if (value == null)
            {
                value = Env.Globals[Name].Force();
            }

            return value;
        }
    }

    public class Application : Value
    {
        private Value result;

        public Value Func { get; set; }
        public Value Argument { get; set; }

        public override Value Force()
        {
            if (result == null)
            {
                var func = (FuncValue)Func.Force();
                result = func.Apply(Argument).Force();
            }

            return result;
        }
    }

    // A very hacky way to represent board.
    public class Board : Value
    {
        public HashSet<(int X, int Y)> Pixels { get; set; } = new HashSet<(int, int)>();

        public Board(Value val)
        {
            while (val.Force() != Value.Nil)
            {
                var point = val.GetFirst();
                    
                var x = (int)point.GetFirst().GetInt();
                var y = (int)point.GetSecond().GetInt();
                Pixels.Add((x, y));

                val = val.GetSecond();
            }
        }

        public override string ToString()
        {
            if (Pixels.Count == 0)
            {
                return "<empty-board>";
            }

            var minX = Pixels.Min(it => it.X) - 1;
            var minY = Pixels.Min(it => it.Y) - 1;
            var maxX = Pixels.Max(it => it.X) + 2;
            var maxY = Pixels.Max(it => it.Y) + 2;

            var sb = new StringBuilder();
            for(var y = minY; y < maxY; ++y)
            {
                for (var x = minX; x < maxX; ++x)
                {
                    sb.Append(Pixels.Contains((x, y)) ? "#" : ".");
                }

                sb.Append("\n");
            }

            return sb.ToString();
        }
    }
}
