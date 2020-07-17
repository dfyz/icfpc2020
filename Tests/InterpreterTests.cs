using System;
using System.Linq;
using Xunit;
using app;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Helpers;

namespace Test
{
    public class InterpreterTests
    {
        [Fact]
        public void TestAdd() {
            EvalAssert("3", "ap ap add 1 2");
            EvalAssert("3", "ap ap add 2 1");
            EvalAssert("1", "ap ap add 0 1");
            EvalAssert("5", "ap ap add 2 3");
            EvalAssert("8", "ap ap add 3 5");
        }

        [Fact]
        public void TestAp() {
            EvalAssert("2", "ap inc ap inc 0");
            EvalAssert("3", "ap inc ap inc ap inc 0");
            /*EvalAssert("ap inc ap dec x0   =   x0
            EvalAssert("ap dec ap inc x0   =   x0
            EvalAssert("ap dec ap ap add x0 1   =   x0
            EvalAssert("ap ap add ap ap add 2 3 4   =   9
            EvalAssert("ap ap add 2 ap ap add 3 4   =   9
            EvalAssert("ap ap add ap ap mul 2 3 4   =   10
            EvalAssert("ap ap mul 2 ap ap add 3 4   =   14*/
        }

        [Fact]
        public void TestMul()
        {
            EvalAssert("8", "ap ap mul 4 2");
            EvalAssert("12", "ap ap mul 3 4");
            EvalAssert("-6", "ap ap mul 3 -2");
        }

        [Fact]
        public void TestList()
        {
            EvalAssert("nil", "( )");
            EvalAssert("(0 nil)", "( 0 )");
            EvalAssert("(0 (1 nil))", "( 0 , 1 )");
            EvalAssert("(0 (1 (2 nil)))", "( 0 , 1 , 2 )");
            EvalAssert("(0 (1 (2 (5 nil))))","( 0 , 1 , 2 , 5 )");
        }

        private void EvalAssert(string expected, params string[] program)
        {
            var env = new Env();
            Value result = null;
            foreach (var s in program) {
                result = env.Eval(s);
            }
            Assert.Equal(expected, result.ToString());
        }
    }
}
