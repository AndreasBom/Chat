using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ChatAweria.Services;

namespace ChatAweria
{
    public class Program
    {

        private static readonly int Port = 50000;
        private static readonly IPAddress IpAddress = IPAddress.Parse("127.0.0.1");

        public static void Main(string[] args)
        {
            var chatService = new ChatService();
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
