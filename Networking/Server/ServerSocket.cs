using ChatAweria.Models;
using ChatAweria.Networking.Abstract;
using ChatAweria.Networking.Encoding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace ChatAweria.Networking.Server
{
    public class ServerSocket : NetworkingBase
    {
        private readonly Socket _socket;
        private byte[] _buffer = new byte[1024];
        private readonly List<ClientModel> _clients = new List<ClientModel>();


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
            var client = new ClientModel(handler);

            // adds the ClientModel to a lists of clients
            _clients.Add(client);

            var welcomeMessage = MessageHandler.GetWelcomeMessage(client, _clients);
            _buffer = PacketHandler.GetPacket(welcomeMessage);

            //Send welcome message
            handler.BeginSend(_buffer, 0, _buffer.Length, SocketFlags.None, SendCallback, handler);

            //Notify all clients that a new client arrived
            NewClientArrived(client);

            _buffer = new byte[1024];
            handler.BeginReceive(client.Buffer, 0, client.Buffer.Length, SocketFlags.None, RecivedCallback, handler);
            Accept();
        }

        private void NewClientArrived(ClientModel client)
        {
            var newClientMsg = MessageHandler.GetNewClientJoinedMessage(client);
            foreach (var c in _clients)
            {
                if (c.IpEndPoint != client.IpEndPoint)
                {
                    c.ClientSocket.SendTo(PacketHandler.GetPacket(newClientMsg), 0, PacketHandler.GetPacket(newClientMsg).Length, SocketFlags.None, c.IpEndPoint);
                }

            }
        }

        protected override void RecivedCallback(IAsyncResult ar)
        {
            var listener = (Socket)ar.AsyncState;
            var handler = listener;

            SocketError se;
            var bufferSize = handler.EndReceive(ar, out se);
            var client = _clients.FirstOrDefault(c => ReferenceEquals(c.IpEndPoint, handler.RemoteEndPoint));
            if (se != SocketError.Success)
            {
                Console.WriteLine($"Socket Recive Error. {client.Name} left the chat");
                ClientLeftChat(client);
            }
            else
            {
                if (client == null)
                {
                    Console.Write($"Client {handler.RemoteEndPoint} not found");
                }
                else
                {
                    //Handle packet
                    //Print full message (and recipietants) on server
                    MessageHandler.PrintString(client.Buffer, bufferSize);

                    var msg = MessageHandler.GetStringFromPacket(client.Buffer, bufferSize);


                    //Prepare and send to clients
                    //if msg contains '@' => extract recipietants and format message
                    IEnumerable<ClientModel> clients = _clients;

                    if (msg.Contains("@"))
                    {
                        var extractedMsg = MessageHandler.ExtracyInput(msg);
                        var recipietantsList = extractedMsg.Item1.ToList();
                        clients = GetRecipietentClients(recipietantsList);
                        msg = $"{client.Name}: " + extractedMsg.Item2;
                        SendMessageToClients(client, msg, clients);
                    }
                    else
                    {
                        //send to all clients or is it a command (away, online)
                        var isMsgCommand = IsMsgACommand(msg, client);

                        if (!isMsgCommand)
                        {
                            msg = $"{client.Name}: " + msg;
                            SendMessageToClients(client, msg, clients);
                        }
                    }

                    client.Buffer = new byte[1024];
                    handler.BeginReceive(client.Buffer, 0, client.Buffer.Length, SocketFlags.None, RecivedCallback, handler);
                    _allDone.Set();
                }
            }

        }

        private void ClientLeftChat(ClientModel client)
        {
            SendMessageToClients(client, $"{client.Name} left the chat", _clients);

            _clients.Remove(client);
            client?.ClientSocket.Shutdown(SocketShutdown.Both);
            client?.ClientSocket.Close();
        }

        private static bool IsMsgACommand(string msg, ClientModel client)
        {
            switch (msg)
            {
                case "away":
                    client.Away = true;
                    return true; //Yes it is a command
                case "online":
                    client.Away = false;
                    return true; //Yes it is a command
                default:
                    return false; //No it is not a command

            }
        }

        private static void SendMessageToClients(ClientModel client, string msg, IEnumerable<ClientModel> clients)
        {
            foreach (var c in clients)
            {
                if (!ReferenceEquals(c.IpEndPoint, client.IpEndPoint) && c.Away == false)
                {
                    c.ClientSocket.SendTo(PacketHandler.GetPacket(msg), 0, PacketHandler.GetPacket(msg).Length, SocketFlags.None, c.IpEndPoint);
                }
            }
        }

        private IEnumerable<ClientModel> GetRecipietentClients(IEnumerable<string> recipietantsList)
        {
            var clients = from r in recipietantsList
                          from c in _clients
                          where r == c.Name && c.Away == false
                          select c;
            return clients;
        }
    }
}
