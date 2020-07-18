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
            var state = GetToGalaxy(env);
            Interactive(env, state);
            return 0;
        }

        // iterates start coords to get to galaxy image
        private static Value GetToGalaxy(Env env)
        {
            var path = new[]
            {
                (0, 0),
                (0, 0),
                (0, 0),
                (0, 0),
                (0, 0),
                (0, 0),
                (0, 0),
                (0, 0),
                (8, 4),
                (2, -8),
                (3, 6),
                (0, -14),
                (-4, 10),
                (9, -3),
                (-4, 10),
                (1, 4),
            };
            var state = Value.Nil;
            foreach (var (x, y) in path)
            {
                state = Zoom(env, state, x, y, false);
            }

            return state;
        }

        private static Value Zoom(Env env, Value state, int x, int y, bool print)
        {
            var res = env.Eval(
                "ap ap ap interact galaxy $1 ap ap vec $2 $3",
                state,
                new Integer { Val = x },
                new Integer { Val = y });
            state = res.GetFirst();

            var data = env.Eval("ap multipledraw $1", res.GetSecond());

            if (print)
            {
                Console.WriteLine(data);
            }

            // Dump to file for easier invistigation of large pictures
            File.WriteAllText("last.txt", data.ToString());

            return state;
        }

        private static void Interactive(Env env, Value initState = null)
        {
            initState ??= Value.Nil;
            var state = initState;
            while (true)
            {
                var coords = EnterCoords();
                if (coords == null)
                {
                    Console.WriteLine("\n\nRESTART");
                    state = initState;
                    continue;
                }

                var (x, y) = coords.Value;
                state = Zoom(env, state, x, y, true);
            }

            static (int, int)? EnterCoords()
            {
                while (true)
                {
                    Console.Write("Enter coords (r - restart): ");
                    var coords = Console.ReadLine().Split(' ');
                    if (coords.Length == 2 &&
                        int.TryParse(coords[0], out var x) &&
                        int.TryParse(coords[1], out var y))
                    {
                        return (x, y);
                    }

                    if (coords.Length == 1 && coords[0] == "r")
                    {
                        return null;
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
