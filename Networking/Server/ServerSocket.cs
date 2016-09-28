using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ChatAweria.Models;
using ChatAweria.Networking.Abstract;
using ChatAweria.Networking.Encoding;

namespace ChatAweria.Networking.Server
{
    public class ServerSocket : NetworkingBase
    {
        private readonly Socket _socket;
        private byte[] _buffer = new byte[1024];
        private List<ClientModel> _clients = new List<ClientModel>(4);
        private ManualResetEvent _allDone = new ManualResetEvent(false);

        public ServerSocket()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Bind(int port)
        {
            _socket.Bind(new IPEndPoint(IPAddress.Any, port));
        }

        public void Listen()
        {
            _socket.Listen(10);
        }

        public void Accept()
        {
            _socket.BeginAccept(AcceptCallback, _socket);
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            var listener = (Socket)ar.AsyncState;
            var handler = listener.EndAccept(ar);
            _buffer = new byte[1024];

            //creates a new ClientModel
            var client = ServerManager.CreateClient(handler);

            // adds the ClientModel to a lists of clients
            _clients.Add(client);

            var welcomeMessage = MessageHandler.GetWelcomeMessage(client, _clients);
            _buffer = PacketHandler.GetPacket(welcomeMessage);

            handler.BeginSend(_buffer, 0, _buffer.Length, SocketFlags.None, SendCallback, handler);

            _buffer = new byte[1024];
            handler.BeginReceive(client.Buffer, 0, client.Buffer.Length, SocketFlags.None, RecivedCallback, handler);
            Accept();
        }

        protected override void RecivedCallback(IAsyncResult ar)
        {
            var listener = (Socket)ar.AsyncState;
            var handler = listener;

            SocketError se;
            var bufferSize = handler.EndReceive(ar, out se);
            if (se != SocketError.Success)
            {
                Console.WriteLine("Socket Recive Error");
            }
            else
            {
                var client = _clients.FirstOrDefault(c => ReferenceEquals(c.IpEndPoint, handler.RemoteEndPoint));
                if (client == null)
                    Console.Write($"Client {handler.RemoteEndPoint} not found");
                //Handle packet
                //Print message on server
                MessageHandler.PrintString(client.Buffer, bufferSize);

                //Send to clients
                foreach (var c in _clients)
                {
                    c.ClientSocket.SendTo(client.Buffer, 0, bufferSize, SocketFlags.None, c.IpEndPoint);
                }

                client.Buffer = new byte[1024];
                handler.BeginReceive(client.Buffer, 0, client.Buffer.Length, SocketFlags.None, RecivedCallback, handler);
                _allDone.Set();
            }

        }
    }
}
