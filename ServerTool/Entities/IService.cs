// a.snegovoy@gmail.com

using System;

using ServerTool.Commands;

namespace ServerTool.Entities
{
    internal interface IService
    {
        bool NeedRestart { get; }
        string Name { get; }
        ICommand Status { get; }
        ICommand Restart { get; }

        bool IsNeedRestart( int time )
        {
            var now = DateTime.Now;
            return NeedRestart && now.Hour == time && now.Minute == 0;
        }
    }
}