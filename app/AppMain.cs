using System.IO;

namespace app
{
    public static class AppMain
    {
        public static int Main(string[] args)
        {
            var programText = File.ReadAllText("../../galaxy.txt");
            var env = Env.Load(programText);
            var galaxy = env.Eval("galaxy");

            return 0;
        }
    }
}