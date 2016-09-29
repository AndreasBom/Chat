using System;
using System.Net;

namespace ChatAweria.Networking.Client
{
    public interface IClientSocket
    {
        bool IsConnected { get; set; }
        void Connect(IPAddress ipAddress, int port);
        void SendAsync(string msg);
        void SetClientAway();
        void SetClientOnline();
        void RecivedCallback(IAsyncResult ar);
    }
}