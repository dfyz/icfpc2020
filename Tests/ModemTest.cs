using System;
using System.Linq;
using Xunit;
using app;

namespace Test
{
    public class ModemTest
    {
        class TestCase {
            public string Ans { get; set; }
            public Value Val { get; set; }
        };

        TestCase[] Cases = new[] {
            new TestCase{Ans = "010", Val = new Integer{Val = 0}},
            new TestCase{Ans = "01100001", Val = new Integer{Val = 1}},
            new TestCase{Ans = "10100001", Val = new Integer{Val = -1}},
            new TestCase{Ans = "01100010", Val = new Integer{Val = 2}},
            new TestCase{Ans = "10100010", Val = new Integer{Val = -2}},
            new TestCase{Ans = "0111000010000", Val = new Integer{Val = 16}},
            new TestCase{Ans = "1011000010000", Val = new Integer{Val = -16}},
            new TestCase{Ans = "0111011111111", Val = new Integer{Val = 255}},
            new TestCase{Ans = "1011011111111", Val = new Integer{Val = -255}},
            new TestCase{Ans = "011110000100000000", Val = new Integer{Val = 256}},
            new TestCase{Ans = "101110000100000000", Val = new Integer{Val = -256}},
            new TestCase{Ans = "00", Val = Builtins.Nil.Instance},
            new TestCase{Ans = "110000", Val = new Pair {First = Builtins.Nil.Instance, Second = Builtins.Nil.Instance,}},
            new TestCase{Ans = "1101000", Val = new Pair {
                First = new Integer{Val = 0},
                Second = Builtins.Nil.Instance,
            }},
            new TestCase{Ans = "110110000101100010", Val = new Pair {
                First = new Integer{Val = 1},
                Second = new Integer{Val = 2},
            }},
            new TestCase{Ans = "1101100001110110001000", Val = new Pair {
                First = new Integer{Val = 1},
                Second = new Pair {
                    First = new Integer{Val = 2},
                    Second = Builtins.Nil.Instance,
                },
            }},
            new TestCase{Ans = "1101100001111101100010110110001100110110010000", Val = new Pair {
                First = new Integer{Val = 1},
                Second = new Pair {
                    First = new Pair {
                        First = new Integer{Val = 2},
                        Second = new Pair {
                            First = new Integer{Val = 3},
                            Second = Builtins.Nil.Instance,
                        },
                    },
                    Second = new Pair {
                        First = new Integer{Val = 4},
                        Second = Builtins.Nil.Instance,
                    },
                },
            }}
        };

        [Fact]
        public void TestModulate()
        {
            foreach (var cs in Cases) {
                Assert.Equal(ToBits(cs.Ans), Modem.Modulate(cs.Val));
            }
        }

        public void TestDemodulate()
        {
            // TODO
        }

        private bool[] ToBits(string s) {
            return s.Select(x => x == '1').ToArray();
        }
    }
}
