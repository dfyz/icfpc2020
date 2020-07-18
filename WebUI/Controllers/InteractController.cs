using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using app;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models;

namespace WebUI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InteractController : ControllerBase
    {
        private static readonly Env env;

        static InteractController()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "WebUI.galaxy.txt";

            using var stream = assembly.GetManifestResourceStream(resourceName);
            using var reader = new StreamReader(stream);
            var programText = reader.ReadToEnd();env = Env.Load(programText);
        }

        [HttpGet]
        public string Get()
        {
            return "Hi!";
        }

        [HttpPost]
        public InteractResponse Post(InteractRequest data)
        {
            var state =
                string.IsNullOrEmpty(data.state)
                    ? Value.Nil
                    : Modem.Demodulate(Sender.StringToBits(data.state));
            var coords = new Pair
            {
                First = new Integer {Val = data.point[0]},
                Second = new Integer {Val = data.point[1]},
            };

            var res = env.Eval(
                "ap ap ap interact galaxy $1 $2",
                state,
                coords);
            return new InteractResponse
            {
                state = Sender.BitsToString(Modem.Modulate(res.GetFirst())),
                boards = GetBoard(res.GetSecond()).ToList(),
            };

            IEnumerable<List<int[]>> GetBoard(Value val)
            {
                while (val.Force() != Value.Nil)
                {
                    var board = new Board(val.GetFirst());
                    var pixels = board.Pixels
                        .Select(it => new[] {it.X, it.Y})
                        .ToList();
                    yield return pixels;
                    val = val.GetSecond();
                }
            }
        }
    }
}
