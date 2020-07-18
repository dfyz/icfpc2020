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
                "res = ap interact galaxy",
                "data = ap cdr res");
            //var data = env.Eval("data");
            var galaxy = env.Eval("ap multipledraw data");

            Console.WriteLine(galaxy);

            return 0;
        }
    }
}