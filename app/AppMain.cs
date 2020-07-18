using System;
using System.IO;

namespace app
{
    public static class AppMain
    {
        public static int Main(string[] args)
        {
            var programText = File.ReadAllText("../../../../galaxy.txt");
            var env = Env.Load(programText);
            
            // Iterate00(env);
            Interactive(env);
            return 0;
        }

        private static void Interactive(Env env)
        {
            var state = Value.Nil;
            for (var i = 0;; ++i)
            {
                var (x, y) = EnterCoords();

                var res = env.Eval(
                    "ap ap ap interact galaxy $1 ap ap vec $2 $3",
                    state,
                    new Integer { Val = x },
                    new Integer { Val = y });
                state = res.GetFirst();
                Console.WriteLine(env.Eval("ap multipledraw $1", res.GetSecond()));
            }

            (int, int) EnterCoords()
            {
                while (true)
                {
                    Console.Write("Enter coords: ");
                    var coords = Console.ReadLine().Split(' ');
                    if (coords.Length == 2 &&
                        int.TryParse(coords[0], out var x) &&
                        int.TryParse(coords[1], out var y))
                    {
                        return (x, y);
                    }
                }
            }
        }

        private static void Iterate00(Env env)
        {
            var state = Value.Nil;
            for (var i = 0; i < 10; ++i)
            {
                Console.WriteLine($"\n\n======== {i}\n");
                var res = env.Eval("ap ap ap interact galaxy $1 ap ap vec 0 0", state);
                state = res.GetFirst();
                Console.WriteLine(env.Eval("ap multipledraw $1", res.GetSecond()));
            }
        }
    }
}