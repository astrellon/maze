using System;

namespace Maze.Service
{
    public Service OwnerService { get; protected set; }
    public CommandProcessor Processor
    {
        get
        {
            return OwnerService.LocalCommandProcessor;
        }
    }
    public class CommandHandlers(Service service)
    {
        OwnerService = service;

        Processor.AddHandler("info", InfoHandler);
    }

    public object InfoHandler(LocalCommand cmd)
    {
        return null;
    }
}
