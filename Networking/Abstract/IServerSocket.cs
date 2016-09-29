namespace ChatAweria.Networking.Abstract
{
    public interface IServerSocket
    {
        void Bind(int port);
        void Listen();
        void Accept();
    }
}