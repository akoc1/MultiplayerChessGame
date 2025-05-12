using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models
{
    public class GodotClient
    {
        public string Id { get; set; }
        public TcpClient TcpClient { get; set; }

        public GodotClient(TcpClient client, string id)
        {
            Id = id;
            TcpClient = client;
        }
    }
}
