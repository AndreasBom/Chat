using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatAweria.Networking.Abstract
{
    public abstract class NetworkingBase
    {
        protected ManualResetEvent _allDone = new ManualResetEvent(false);

        protected void SendAsyncTo(byte[] buffer, IPEndPoint endpoint, Socket handler)
        {
            handler.BeginSendTo(buffer, 0, buffer.Length, SocketFlags.None, endpoint, SendCallback, handler);
        }

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

        protected abstract void RecivedCallback(IAsyncResult ar);
    }
}
