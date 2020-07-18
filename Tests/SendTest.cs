using System;
using System.Diagnostics;
using System.Linq;
using Xunit;
using app;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Helpers;

namespace Test
{
    public class SendTest
    {
        [Fact]
        public void TestBitsToString()
        {
            var bits = new bool[3];
            bits[0] = true;
            bits[1] = false;
            bits[2] = true;
            Assert.Equal("101", Sender.BitsToString(bits));
        }
        
        [Fact]
        private void TestStringToBits()
        {
            var bits = Sender.StringToBits("101");
            Assert.True(bits[0]);
            Assert.False(bits[1]);
            Assert.True(bits[0]);
        }

        [Fact]
        private void TestSend()
        {
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
            Trace.WriteLine("Hello World");
        }
    }
}