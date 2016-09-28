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
        private IPEndPoint _endPoint;
        public IPAddress IpAddress { get; set; }
        public int Port { get; set; }
        public IPEndPoint IpEndPoint => _endPoint;
        public string Name { get; set; }
        public Socket ClientSocket { get; set; }
        public byte[] Buffer { get; set; }

        public ClientModel(Socket client)
        {
            var c = client.RemoteEndPoint as IPEndPoint;
            _endPoint = c;
            if (c != null)
            {
                IpAddress = c.Address;
                Port = c.Port;
                Buffer = new byte[1024];

                Name = GenerateName();
                ClientSocket = client;
            }
        }
    }
}
