using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAweria.Networking.Encoding
{
    public class PacketHandler
    {
        //Convert from string to byte[]
        public static byte[] GetPacket(string value)
        {
            var buffer = new byte[value.Length];
            buffer = System.Text.Encoding.UTF8.GetBytes(value);

            return buffer;
        }
    }
}
