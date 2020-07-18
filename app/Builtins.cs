// <copyright file="Builtins.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

using System;

namespace app
{
    public static class Builtins
    {
        public class Nil : Func1Value<Nil>
        {
            public override Value Apply(Value arg) => T.Instance;

            public override string ToString() => "nil";
        }

        public class IsNil : Func1Value<IsNil>
        {
            public override Value Apply(Value arg) =>
                arg.Force() == Value.Nil
                    ? (Value)T.Instance
                    : F.Instance;
        }

        public abstract class Func1Value<TFunc> : FuncValue
            where TFunc : Func1Value<TFunc>, new()
        {
            public static TFunc Instance { get; } = new TFunc();
        }

        public abstract class Func2Value<TFunc> : FuncValue
            where TFunc : Func2Value<TFunc>, new()
        {
            public static TFunc Instance { get; } = new TFunc();

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
            public static TFunc Instance { get; } = new TFunc();

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

        public class Neg : Func1Value<Neg>
        {
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

        public class I : Func1Value<I>
        {
            public override Value Apply(Value x0) => x0;

            public override string ToString() => "i";
        }

        // t, true, K-combinator
        public class T : Func2Value<T>
        {
            protected override Value Apply(Value x0, Value x1) => x0;

            public override string ToString() => "t";
        }

        // f, false
        public class F : Func2Value<F>
        {
            protected override Value Apply(Value x0, Value x1) => x1;

            public override string ToString() => "f";
        }

        public class Car : Func1Value<Car>
        {
            public override Value Apply(Value pair) =>
                ((Pair)pair.Force()).First;
        }

        public class Cdr : Func1Value<Cdr>
        {
            public override Value Apply(Value pair) =>
                ((Pair)pair.Force()).Second;
        }

        public class Eq : Func2Value<Eq>
        {
            protected override Value Apply(Value x0, Value x1) =>
                ((Integer) x0.Force()).Val == ((Integer) x1.Force()).Val
                    ? (Value) T.Instance
                    : F.Instance;
        }

        public class Lt : Func2Value<Lt>
        {
            protected override Value Apply(Value x0, Value x1) =>
                ((Integer) x0.Force()).Val < ((Integer) x1.Force()).Val
                    ? (Value) T.Instance
                    : F.Instance;
        }

        public class Mul : Func2Value<Mul>
        {
            protected override Value Apply(Value x0, Value x1) =>
                new Integer { Val = checked(((Integer) x0.Force()).Val * ((Integer) x1.Force()).Val) };
        }

        public class Div : Func2Value<Div>
        {
            // div by zero?
            protected override Value Apply(Value x0, Value x1) =>
                new Integer { Val = checked(((Integer) x0.Force()).Val / ((Integer) x1.Force()).Val) };
        }

        public class Add : Func2Value<Add>
        {
            protected override Value Apply(Value x0, Value x1) =>
                new Integer { Val = checked(((Integer) x0.Force()).Val + ((Integer) x1.Force()).Val) };

            public override string ToString() => "add";
        }

        public class Inc : Func1Value<Inc>
        {
            public override Value Apply(Value x) =>
                new Integer { Val = checked(((Integer) x.Force()).Val + 1) };
        }

        public class Dec : Func1Value<Dec>
        {
            public override Value Apply(Value x) =>
                new Integer { Val = checked(((Integer) x.Force()).Val - 1) };
        }

        public class Draw : Func1Value<Draw>
        {
            public override Value Apply(Value x) => new Board(x);
        }
        
        public class MultipleDraw : Func1Value<MultipleDraw>
        {
            public override Value Apply(Value val)
            {
                var head = new Pair();
                var current = head;
                
                while (val.Force() != Value.Nil)
                {
                    var v = (Pair) val.Force();
                    var list = v.First.Force();
                    
                    var next = new Pair { First = new Board(list) };
                    current.Second = next;
                    current = next;
                    
                    val = v.Second.Force();
                }

                current.Second = Value.Nil;
                return head.Second;
            }
        }

        public class Interact : Func1Value<Interact>
        {
            public override Value Apply(Value protocol)
            {
                long flag = 1;
                var state = Value.Nil;
                Value data = Value.Nil;
                Value vector = new Pair
                {
                    First = new Integer { Val = 0},
                    Second = new Integer { Val = 0 },
                };

                while (flag != 0)
                {
                    var app = new Application
                    {
                        Func = new Application
                        {
                            Func = protocol,
                            Argument = state,
                        },
                        Argument = vector,
                    };

                    var t0 = (Pair)app.Force();
                    flag = ((Integer) t0.First.Force()).Val;
                    var t1 = (Pair)t0.Second.Force();
                    state = Modem.Demodulate(Modem.Modulate(t1.First.Force()));
                    var t2 = (Pair)t1.Second.Force();
                    data = t2.First.Force();

                    Console.WriteLine("\n\n\n\n\n\n");
                    Console.WriteLine($"Flag = {flag}");
                    Console.WriteLine(state);
                    Console.WriteLine(data);

                    if (flag != 0)
                    {
                        var reply = Sender.Send(data).Result;
                        Console.WriteLine(reply);
                        vector = reply;
                    }
                }

                return new Pair
                {
                    First = state,
                    Second = data,
                };
            }
        }
    }
}