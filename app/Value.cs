using System;
using System.Collections.Generic;
using System.Text;

namespace app
{
    public abstract class Value
    {
        public virtual Value Force() => this;
    }

    public abstract class FuncValue : Value
    {
        public abstract Value Apply(Value argument);
    }

    public class Integer : Value
    {
        public long Val { get; set; }
    }

    public class Pair : Value
    {
        public Value First { get; set; }
        public Value Second { get; set; }
    }

    public class Nil : Value
    {
        public static Nil Instance => new Nil();

        private Nil()
        {
        }
    }

    public class Variable : Value
    {
        private Value value;

        // TODO(lazy): switch to integer if strings are too slow
        public string Name { get; set; }
        public Env Env { get; set; }

        public Value Force()
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
                result = func.Apply(Argument);
            }

            return result;
        }
    }

    public static class Bulitins
    {

    }
}
