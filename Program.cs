using ChatAweria.Services;
using System;
using System.Net;

namespace ChatAweria
{
    public class Program
    {

        private static readonly int Port = 50000;
        private static readonly IPAddress IpAddress = IPAddress.Parse("127.0.0.1");

        public static void Main(string[] args)
        {
            IChatService chatService = new ChatService();

            try
            {
                chatService.StartServer();
            }
            catch (Exception)
            {
                chatService.StartAndConnectClient(IpAddress, Port);
            }

        }
    }
}
