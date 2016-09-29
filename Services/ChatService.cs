using ChatAweria.Networking.Client;
using ChatAweria.Networking.Server;
using System;
using System.Net;

namespace ChatAweria.Services
{

    public class ChatService
    {
        private readonly ServerSocket _serverSocket;
        private readonly ClientSocket _clientSocket;


        public ChatService()
        {
            _serverSocket = new ServerSocket();
        }

        public ChatService(ServerSocket serverSocket, ClientSocket clientSocket)
        {
            _serverSocket = serverSocket;
            _clientSocket = clientSocket;
        }

        public void StartServer()
        {
            _serverSocket.Bind(50000);
            _serverSocket.Listen();
            _serverSocket.Accept();
            Console.WriteLine("Server started");

            while (true)
            {
                Console.ReadKey();
            }
        }

        public void StartAndConnectClient(IPAddress ipAddress, int port)
        {
            var client = new ClientSocket();
            client.Connect(ipAddress, port);
            while (true)
            {
                if (client.IsConnected)
                {
                    var msg = Console.ReadLine();

                    if (!msg.Contains("@"))
                    {
                        if (msg.ToLower() == "away")
                        {
                            client.SetClientAway();
                        }
                        if (msg.ToLower() == "online")
                        {
                            client.SetClientOnline();
                        }


                    }

                    client.SendAsync(msg);

                }

            }
        }
    }
}
