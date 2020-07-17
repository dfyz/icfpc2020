using System;
using System.Linq;
using Xunit;
using app;

namespace Test
{
    public class ModemTest
    {
        [Fact]
        public void TestModulate()
        {
            Assert.Equal(ToBits("010"), Modem.Modulate(new Integer{Val = 0}));
            Assert.Equal(ToBits("01100001"), Modem.Modulate(new Integer{Val = 1}));
            Assert.Equal(ToBits("10100001"), Modem.Modulate(new Integer{Val = -1}));
            Assert.Equal(ToBits("01100010"), Modem.Modulate(new Integer{Val = 2}));
            Assert.Equal(ToBits("10100010"), Modem.Modulate(new Integer{Val = -2}));
            Assert.Equal(ToBits("0111000010000"), Modem.Modulate(new Integer{Val = 16}));
            Assert.Equal(ToBits("1011000010000"), Modem.Modulate(new Integer{Val = -16}));
            Assert.Equal(ToBits("0111011111111"), Modem.Modulate(new Integer{Val = 255}));
            Assert.Equal(ToBits("1011011111111"), Modem.Modulate(new Integer{Val = -255}));
            Assert.Equal(ToBits("011110000100000000"), Modem.Modulate(new Integer{Val = 256}));
            Assert.Equal(ToBits("101110000100000000"), Modem.Modulate(new Integer{Val = -256}));
        }

        private bool[] ToBits(string s) {
            return s.Select(x => x == '1').ToArray();
        }
    }
}
