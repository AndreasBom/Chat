using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using ChatAweria.Networking.Client;
using ChatAweria.Networking.Server;

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
                var msg = Console.ReadLine();
                client.SendAsync(msg);
            }
        }
    }
}
