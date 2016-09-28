using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ChatAweria.Models;

namespace ChatAweria.Networking.Server
{
    public class ServerManager
    {
        public static ClientModel CreateClient(Socket clientSocket)
        {
            return new ClientModel(clientSocket);
        }

    }
}
