// a.snegovoy@gmail.com

using ServerTool.Entities;

namespace ServerTool.Commands
{
    internal interface ICommand
    {
        CommandStatus Status { get; }
        string Execute();
    }
}