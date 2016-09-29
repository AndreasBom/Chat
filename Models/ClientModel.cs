using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

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
        public bool Away { get; set; } = false;

        public ClientModel(Socket client, IEnumerable<ClientModel> clients)
        {
            var c = client.RemoteEndPoint as IPEndPoint;
            _endPoint = c;
            if (c != null)
            {
                IpAddress = c.Address;
                Port = c.Port;
                Buffer = new byte[1024];

                do
                {
                    Name = GenerateName();
                } while (clients.Any(n => n.Name == Name));

                ClientSocket = client;
            }
        }
    }
}
