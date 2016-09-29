using ChatAweria.Services;
using System.Diagnostics;
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
            var noOfInstances = Process.GetProcessesByName("ChatAweria").Length;

            //Start server
            if (noOfInstances == 0)
            {
                chatService.StartServer();
            }
            //Start client
            else
            {
                chatService.StartAndConnectClient(IpAddress, Port);
            }
        }
    }
}
