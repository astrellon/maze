namespace Maze.Service
{
    public interface ISendService
    {
        void SendData(object data, ResponseCallback callback = null);
        void SendString(string data, ResponseCallback callback = null);
    }
}
