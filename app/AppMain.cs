using System;
using System.IO;

namespace app
{
    public static class AppMain
    {
        public static int Main(string[] args)
        {
            var programText = File.ReadAllText("../../../../galaxy.txt");
            var env = Env.Load(
                programText,
                "it1 = ap ap galaxy nil ap ap vec 0 0",
                "data = ap car ap cdr ap cdr it1");
            var data = env.Eval("data");
            var galaxy = env.Eval("ap multipledraw data");

            Console.WriteLine(data);
            Console.WriteLine(galaxy);

            return 0;
        }
    }
}