using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatAweria.Models;

namespace ChatAweria.Networking.Encoding
{
    public class MessageHandler
    {
        public static void PrintString(byte[] buffer, int packetLength)
        {
            byte[] packet = new byte[packetLength];
            Array.Copy(buffer, packet, packet.Length);

            var msg = System.Text.Encoding.UTF8.GetString(packet);
            Console.WriteLine(msg);
        }

        public static string GetWelcomeMessage(ClientModel client, List<ClientModel> clients)
        {

            var clientsToShow = clients
                .Where(c => c.Port != client.Port)
                .Select(c => c.Name).ToList();

            if (clientsToShow.Any())
            {
                return
                    $"Welcome {client.Name}. You have port: {client.Port} \nThese clients are currently connected \n" +
                    string.Join(",\n", clientsToShow.ToArray());
            }

            return $"Welcome {client.Name}. You have port: {client.Port} \nYou are all alone in here";
        }
    }
}
