using System;
using System.Collections.Generic;
using System.Text;

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
    }

    public class Pair : Value
    {
        public Value First { get; set; }
        public Value Second { get; set; }
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
                result = func.Apply(Argument);
            }

            return result;
        }
    }

    public static class Builtins
    {
        public class Nil : Value
        {
            public static Nil Instance => new Nil();

            private Nil()
            {
            }
        }

        public abstract class Func2Value<TFunc> : FuncValue
            where TFunc : Func2Value<TFunc>, new()
        {
            public static TFunc Instance => new TFunc();

            public override Value Apply(Value x0) => new Closure1 { X0 = x0 };

            protected abstract Value Apply(Value x0, Value x1);

            private class Closure1 : FuncValue
            {
                public Value X0 { get; set; }
                public override Value Apply(Value x1) => Instance.Apply(X0, x1);
            }
        }

        public abstract class Func3Value<TFunc> : FuncValue
            where TFunc : Func3Value<TFunc>, new()
        {
            public static TFunc Instance => new TFunc();

            public override Value Apply(Value x0) => new Closure1 { X0 = x0 };

            protected abstract Value Apply(Value x0, Value x1, Value x3);

            private class Closure1 : FuncValue
            {
                public Value X0 { get; set; }

                public override Value Apply(Value x1) =>
                    new Closure2 {X0 = this.X0, X1 = x1};
            }

            private class Closure2 : FuncValue
            {
                public Value X0 { get; set; }
                public Value X1 { get; set; }

                public override Value Apply(Value x2) =>
                    Instance.Apply(X0, X1, x2);
            }
        }

        public class Cons : Func2Value<Cons>
        {
            protected override Value Apply(Value x0, Value x1) =>
                new Pair { First = x0, Second = x1 };
        }

        public class Neg : FuncValue
        {
            public static Neg Instance => new Neg();

            public override Value Apply(Value argument) =>
                new Integer { Val = -((Integer)argument.Force()).Val };
        }

        public class C : Func3Value<C>
        {
            protected override Value Apply(Value x0, Value x1, Value x2) =>
                new Application
                {
                    Func = new Application { Func = x0, Argument = x2 },
                    Argument = x1,
                };
        }

        public class B : Func3Value<B>
        {
            protected override Value Apply(Value x0, Value x1, Value x2) =>
                new Application
                {
                    Func = x0,
                    Argument = new Application { Func = x1, Argument = x2 },
                };
        }

        public class S : Func3Value<S>
        {
            protected override Value Apply(Value x0, Value x1, Value x2) =>
                new Application
                {
                    Func = new Application { Func = x0, Argument = x2 },
                    Argument = new Application { Func = x1, Argument = x2 },
                };
        }
    }
}
