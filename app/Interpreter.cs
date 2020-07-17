using System;
using System.Collections.Generic;
using System.Text;

namespace app
{
    class Interpreter
    {
        // A hack: Using thread-local storage to deliver current interpreter context to all funcs
        [ThreadStatic] private static Interpreter current;

        public static Interpreter Current => current;

        public Interpreter(Program program)
        {

        }

        public void Run()
        {

        }
    }
}
