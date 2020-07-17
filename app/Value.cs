using System;
using System.Collections.Generic;
using System.Text;

namespace app
{
    public abstract class Value
    {
        public virtual Value Apply(Value val) =>
            throw new InvalidOperationException();
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

    public class UserFunc : Value
    {

    }

    public static class BulitinFunctions
    {
        
    }
}
