using System;
using System.Globalization;
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
        public bool[,] Pixels { get; set; } = new bool[20, 20];

        public Board(Value val)
        {
            while (val.Force() != Value.Nil)
            {
                var v = (Pair) val.Force();
                var point = (Pair) v.First.Force();
                    
                var x = 10 + ((Integer) point.First.Force()).Val;
                var y = 10 + ((Integer) point.Second.Force()).Val;

                if (y >= 0 && x >= 0 && y < Pixels.GetLength(0) && x < Pixels.GetLength(1))
                {
                    Pixels[y, x] = true;
                }

                val = v.Second.Force();
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder(30 * 30);
            for(var i = 0; i < Pixels.GetLength(0); i++)
            {
                for(var j = 0; j < Pixels.GetLength(1); j++)
                    sb.Append(Pixels[i, j] ? "#" : ".");
                sb.Append("\n");
            }
            return sb.ToString();
        }
    }
}
