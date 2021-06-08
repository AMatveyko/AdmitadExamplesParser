// a.snegovoy@gmail.com

using System;

namespace Common.Entities
{
    public sealed class BackgroundWork{
        public BackgroundWork( Action action, string id )
        {
            Action = action;
            Id = id;
        }
        
        public Action Action { get; }
        public string Id { get; }
        
        public void Deconstruct(out Action action, out string id)
        {
            action = Action;
            id = Id;
        }
    }
}