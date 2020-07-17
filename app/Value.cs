using System;
using System.Collections.Generic;
using System.Text;

namespace app
{
    public abstract class Value
    {
        public virtual Value Force(Env env) => this;
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

    public class Variable : Value
    {
        // TODO(lazy): switch to integer if strings are too slow
        public string Name { get; set; }

        public Value Force(Env env) => env.Globals[Name].Force(env);
    }

    public class Application : Value
    {
        private Value result;

        public Value Func { get; set; }
        public Value Argument { get; set; }

        public override Value Force(Env env)
        {
            if (result == null)
            {
                var func = (FuncValue)Func.Force(env);
                result = func.Apply(Argument);
            }

            return result;
        }
    }

    public static class Bulitins
    {

    }
}
