using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Models
{
    public class InteractResponse
    {
        public string state { get; set; }
        public List<List<int[]>> boards { get; set; }
    }
}
