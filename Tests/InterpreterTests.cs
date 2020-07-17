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
        public void TestMul()
        {
            EvalAssert("8", "ap ap mul 4 2");
            EvalAssert("12", "ap ap mul 3 4");
            EvalAssert("-6", "ap ap mul 3 -2");
        }

        private void EvalAssert(string expected, string program)
        {
            var env = new Env();
            var result = env.Eval(program);
            Assert.Equal(expected, result.ToString());
        }
    }
}
