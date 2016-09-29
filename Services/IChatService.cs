using System.Net;

namespace ChatAweria.Services
{
    public interface IChatService
    {
        void StartServer();
        void StartAndConnectClient(IPAddress ipAddress, int port);
    }
}