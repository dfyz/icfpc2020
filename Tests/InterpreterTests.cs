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
            EvalAssert("14", "ap ap mul 2 ap ap add 3 4");
            EvalAssert("9", "ap ap add ap ap add 2 3 4");
            EvalAssert("9", "ap ap add 2 ap ap add 3 4");
            EvalAssert("10", "ap ap add ap ap mul 2 3 4");

            EvalAssert("42", "ap inc ap dec x0", "x0 = 42");
            EvalAssert("42", "ap dec ap inc x0", "x0 = 42");
            EvalAssert("42", "ap dec ap ap add x0 1", "x0 = 42");

            EvalAssert("43", "ap inc' 42", "inc' = ap add 1");
            EvalAssert("41", "ap dec' 42", "dec' = ap add ap neg 1");
        }

        [Fact]
        public void TestB()
        {
            EvalAssert("42", "ap ap ap b inc dec x0", "x0 = 42");
        }

        [Fact]
        public void TestC()
        {
            EvalAssert("3", "ap ap ap c add 1 2");
            EvalAssert("8", "ap ap ap c div 10 88");
        }


        [Fact]
        public void TestCar()
        {
            EvalAssert("42", "ap car ap ap cons x0 x1", "x0 = 42", "x1 = nil");

            EvalAssert("42", "ap car x2", "x2 = ap ap cons 42 nil");
            EvalAssert("42", "ap x2 t", "x2 = ap ap cons 42 nil");
        }

        [Fact]
        public void TestCdr()
        {
            EvalAssert("nil", "ap cdr ap ap cons x0 x1", "x0 = 42", "x1 = nil");

            EvalAssert("nil", "ap cdr x2", "x2 = ap ap cons 42 nil");
            EvalAssert("nil", "ap x2 f", "x2 = ap ap cons 42 nil");
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

        
        [Fact(Skip="The world is not ready yet")]
        public void TestCheckerBoard()
        {
            var env = Env.Load("checkerboard = ap ap s ap ap b s ap ap c ap ap b c ap ap b ap c ap c ap ap s ap ap b s ap ap b ap b ap ap s i i lt eq ap ap s mul i nil ap ap s ap ap b s ap ap b ap b cons ap ap s ap ap b s ap ap b ap b cons ap c div ap c ap ap s ap ap b b ap ap c ap ap b b add neg ap ap b ap s mul div ap ap c ap ap b b checkerboard ap ap c add 2");
            //var res7 = env.Eval("ap ap checkerboard 7 0");
            //Assert.Equal("hi", res7.ToString());
        }


        private void EvalAssert(string expected, string program, params string[] startupProgram)
        {
            var env = Env.Load(string.Join("\n", startupProgram));
            var result = env.Eval(program);
            Assert.Equal(expected, result.ToString());
        }
    }
}
