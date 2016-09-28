using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatAweria.Models
{

    /// <summary>
    /// Replisentation if an client
    /// </summary>
    public class ClientModel : BaseModel
    {
        public IPAddress IpAddress { get; set; }
        public int Port { get; set; }
        public IPEndPoint IpEndPoint => new IPEndPoint(IpAddress, Port);
        public string Name { get; set; }
        public Socket ClientSocket { get; set; }

        public ClientModel(Socket client)
        {
            var c = client.RemoteEndPoint as IPEndPoint;
            if (c != null)
            {
                IpAddress = c.Address;
                Port = c.Port;

                Name = GenerateName();
                ClientSocket = client;
            }
        }

    }
}
