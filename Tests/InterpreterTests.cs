﻿using System;
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
        public void TestCons()
        {
            EvalAssert("42", "ap ap ap cons 42 nil t");
            EvalAssert("nil", "ap ap ap cons 42 nil f");
        }

        [Fact]
        public void TestDiv() {
            EvalAssert("2", "ap ap div 4 2");
            EvalAssert("1", "ap ap div 4 3");
            EvalAssert("1", "ap ap div 4 4");
            EvalAssert("0", "ap ap div 4 5");
            EvalAssert("2", "ap ap div 5 2");
            EvalAssert("-3", "ap ap div 6 -2");
            EvalAssert("-1", "ap ap div 5 -3");
            EvalAssert("-1", "ap ap div -5 3");
            EvalAssert("1", "ap ap div -5 -3");
            EvalAssert("12345678", "ap ap div x0 1", "x0 = 12345678");
        }

        [Fact]
        public void TestEq()
        {
            EvalAssert("t", "ap ap eq x0 x0", "x0 = 42");
            EvalAssert("f", "ap ap eq 0 -2");
            EvalAssert("f", "ap ap eq 0 -1");
            EvalAssert("t", "ap ap eq 0 0");
            EvalAssert("f", "ap ap eq 0 1");
            EvalAssert("f", "ap ap eq 0 2");

            EvalAssert("f", "ap ap eq 1 -1");
            EvalAssert("f", "ap ap eq 1 0");
            EvalAssert("t", "ap ap eq 1 1");
            EvalAssert("f", "ap ap eq 1 2");
            EvalAssert("f", "ap ap eq 1 3");

            EvalAssert("f", "ap ap eq 2 0");
            EvalAssert("f", "ap ap eq 2 1");
            EvalAssert("t", "ap ap eq 2 2");
            EvalAssert("f", "ap ap eq 2 3");
            EvalAssert("f", "ap ap eq 2 4");

            EvalAssert("f", "ap ap eq 19 20");
            EvalAssert("t", "ap ap eq 20 20");
            EvalAssert("f", "ap ap eq 21 20");

            EvalAssert("f", "ap ap eq -19 -20");
            EvalAssert("t", "ap ap eq -20 -20");
            EvalAssert("f", "ap ap eq -21 -20");
        }

        [Fact]
        public void TestI()
        {
            EvalAssert("42", "ap i x0", "x0 = 42");
            EvalAssert("1", "ap i 1");
            EvalAssert("i", "ap i i");
            EvalAssert("add", "ap i add");
            EvalAssert("2", "ap inc' 1", "inc' = ap i ap add 1");
        }

        [Fact]
        public void TestIsNil()
        {
            EvalAssert("t", "ap isnil nil");
            EvalAssert("f", "ap isnil ap ap cons x0 x1", "x0 = 42", "x1 = nil");
            EvalAssert("t", "ap isnil ap cdr ap ap cons 42 nil");
        }

        [Fact]
        public void TestLt()
        {
            EvalAssert("f", "ap ap lt 0 -1");
            EvalAssert("f", "ap ap lt 0 0");
            EvalAssert("t", "ap ap lt 0 1");
            EvalAssert("t", "ap ap lt 0 2");

            EvalAssert("f", "ap ap lt 1 0");
            EvalAssert("f", "ap ap lt 1 1");
            EvalAssert("t", "ap ap lt 1 2");
            EvalAssert("t", "ap ap lt 1 3");

            EvalAssert("f", "ap ap lt 2 1");
            EvalAssert("f", "ap ap lt 2 2");
            EvalAssert("t", "ap ap lt 2 3");
            EvalAssert("t", "ap ap lt 2 4");

            EvalAssert("t", "ap ap lt 19 20");
            EvalAssert("f", "ap ap lt 20 20");
            EvalAssert("f", "ap ap lt 21 20");

            EvalAssert("f", "ap ap lt -19 -20");
            EvalAssert("f", "ap ap lt -20 -20");
            EvalAssert("t", "ap ap lt -21 -20");
        }

        [Fact]
        public void TestMul()
        {
            EvalAssert("8", "ap ap mul 4 2");
            EvalAssert("12", "ap ap mul 3 4");
            EvalAssert("-6", "ap ap mul 3 -2");
        }

        [Fact]
        public void TestNeg()
        {
            EvalAssert("0", "ap neg 0");
            EvalAssert("-1", "ap neg 1");
            EvalAssert("1", "ap neg -1");
            EvalAssert("-2", "ap neg 2");
            EvalAssert("2", "ap neg -2");
        }

        [Fact]
        public void TestNil()
        {
            EvalAssert("t", "ap nil x0", "x0 = 42");
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
