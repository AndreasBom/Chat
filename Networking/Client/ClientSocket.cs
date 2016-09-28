using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ChatAweria.Networking.Abstract;
using ChatAweria.Networking.Encoding;

namespace ChatAweria.Networking.Client
{
    public class ClientSocket : NetworkingBase
    {
        private Socket _socket;
        private byte[] _buffer = new byte[1024];

        public ClientSocket()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Connect(IPAddress ipAddress, int port)
        {
            _socket.BeginConnect(new IPEndPoint(ipAddress, port), ConnectCallback, _socket);
        }

        public void SendAsync(string msg)
        {
            var packet = PacketHandler.GetPacket(msg);
            SendAsync(packet, _socket);
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            if (_socket.Connected)
            {
                //var state = (Socket)ar.AsyncState;
                //var handler = state;
                Console.WriteLine("Connected to Server");
                _buffer = new byte[1024];
                _socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, RecivedCallback, _socket);

                //Handle packet

            }
            else
            {
                Console.WriteLine("Could not connect to server");
            }
        }

        protected override void RecivedCallback(IAsyncResult ar)
        {
            var state = (Socket)ar.AsyncState;
            var handler = state;
            int bufLength = state.EndReceive(ar);

            MessageHandler.PrintString(_buffer, bufLength);

        }
    }
}
