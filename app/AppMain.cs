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

            var state = Value.Nil;
            for (var i = 0; i < 10; ++i)
            {
                Console.WriteLine($"\n\n======== {i}\n");
                var res = env.Eval("ap ap ap interact galaxy $1 ap ap vec 0 0", state);
                state = res.GetFirst();
                Console.WriteLine(env.Eval("ap multipledraw $1", res.GetSecond()));
            }

            return 0;
        }
    }
}