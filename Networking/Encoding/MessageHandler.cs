using ChatAweria.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChatAweria.Networking.Encoding
{
    public class MessageHandler
    {
        public static void PrintString(byte[] buffer, int packetLength)
        {
            var msg = GetStringFromPacket(buffer, packetLength);
            Console.WriteLine(msg);
        }

        public static string GetStringFromPacket(byte[] buffer, int packetLength)
        {
            byte[] packet = new byte[packetLength];
            Array.Copy(buffer, packet, packet.Length);

            return System.Text.Encoding.UTF8.GetString(packet);
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

        public static Tuple<string[], string> ExtracyInput(string input)
        {
            var senderMessage = input.Split('@');
            var senders = senderMessage[0].Split(',');
            var returnVal = Tuple.Create<string[], string>(senders, senderMessage[1]);

            return returnVal;
        }

        public static string GetNewClientJoinedMessage(ClientModel client)
        {
            return $"{client.Name} joined the chat";
        }
    }
}
