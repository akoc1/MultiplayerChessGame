using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models;

public class Game
{
    public string GameCode { get; set; }

    public List<GodotClient> Players { get; set; } = new List<GodotClient>();

    public bool IsFull()
    {
        return Players.Count == 2;
    }
}
