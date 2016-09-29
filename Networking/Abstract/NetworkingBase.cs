using System;
using System.Net.Sockets;
using System.Threading;

namespace ChatAweria.Networking.Abstract
{
    public abstract class NetworkingBase
    {
        protected ManualResetEvent _allDone = new ManualResetEvent(false);

        protected void SendAsync(byte[] buffer, Socket handler)
        {
            handler.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, SendCallback, handler);
        }

        protected void SendCallback(IAsyncResult ar)
        {
            var handler = (Socket)ar.AsyncState;
            handler.EndSend(ar);
            _allDone.Set();
        }

        public abstract void RecivedCallback(IAsyncResult ar);
    }
}
