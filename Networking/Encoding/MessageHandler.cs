using ChatAweria.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChatAweria.Networking.Encoding
{
    public class MessageHandler
    {
        /// <summary>
        /// 
        /// Prints a string from a byte[]
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="packetLength"></param>
        public static void PrintString(byte[] buffer, int packetLength)
        {
            var msg = GetStringFromPacket(buffer, packetLength);
            Console.WriteLine(msg);
        }

        /// <summary>
        /// 
        /// Converts a string to a byte[]
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="packetLength"></param>
        /// <returns></returns>
        public static string GetStringFromPacket(byte[] buffer, int packetLength)
        {
            byte[] packet = new byte[packetLength];
            Array.Copy(buffer, packet, packet.Length);

            return System.Text.Encoding.UTF8.GetString(packet);
        }

        /// <summary>
        /// 
        /// Returns a string to be used when a client joins the chat
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="clients"></param>
        /// <returns></returns>
        public static string GetWelcomeMessage(ClientModel client, List<ClientModel> clients)
        {
            var instructions = $"\nINSTRUCTIONS" +
                    "\nSend to all: Just type message" +
                    "\nSend To one clients: Yellow-Cow@Message goes here" +
                    "\nSend to many clients: Yellow-Cow,Green-Horse@Message goes here" +
                    "\nYou can type away or online to set your status" +
                    "\n";

            var clientsToShow = clients
                .Where(c => c.Port != client.Port)
                .Select(c => c.Name).ToList();

            if (clientsToShow.Any())
            {
                return
                    $"Welcome {client.Name}. You have port: {client.Port} " +
                    $"\nThese clients are currently connected \n" +
                    string.Join(",\n", clientsToShow.ToArray()) +
                    $"\n{instructions}\n";


            }

            return $"Welcome {client.Name}. You have port: {client.Port} " +
                   $"\nYou are all alone in here" +
                   $"\n{instructions}\n";
        }

        /// <summary>
        /// 
        /// Splits the string at the @-sign.
        /// First part is an string[] with recipietants
        /// Second part is the message
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns>string[] with recipietants, string message</returns>
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
