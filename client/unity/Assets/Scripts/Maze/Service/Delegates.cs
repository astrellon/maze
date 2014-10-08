namespace Maze.Service
{
    public delegate void ConnectedCallback();
    public delegate void ResponseCallback(Response response, object rawResult);

    public delegate object CommandHandler(LocalCommand cmd);
}
