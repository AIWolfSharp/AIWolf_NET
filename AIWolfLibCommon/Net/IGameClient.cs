namespace AIWolf.Common.Net
{
    interface IGameClient
    {
        object Recieve(Packet packet);
    }
}
