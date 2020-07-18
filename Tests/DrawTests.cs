using System;
using System.Linq;
using Xunit;
using app;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Helpers;

namespace Test
{
    public class DrawTests
    {
        [Fact]
        public void TestDraw() {
            var expected = 
            @"..........
            ..........
            ..........
            ..........
            ";
            EvalAssert(expected, "ap draw ( ap ap vec 5 3 , ap ap vec 6 3 , ap ap vec 4 4 , ap ap vec 6 4 , ap ap vec 4 5 )");
        }
        
        private void EvalAssert(string expected, string program, params string[] startupProgram)
        {
            var env = Env.Load(string.Join("\n", startupProgram));
            var result = env.Eval(program);
            Assert.Equal(expected, result.ToString());
        }
    }
}
