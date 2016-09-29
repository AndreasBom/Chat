namespace ChatAweria.Networking.Encoding
{
    public class PacketHandler
    {
        //Convert from string to byte[]
        public static byte[] GetPacketFromString(string value)
        {
            var buffer = new byte[value.Length];
            buffer = System.Text.Encoding.UTF8.GetBytes(value);

            return buffer;
        }
    }
}
