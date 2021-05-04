// a.snegovoy@gmail.com

using System.Collections.Generic;

namespace AdmitadCommon.Entities.Api
{
    public abstract class ParallelBackgroundContext : BackgroundBaseContext
    {
        protected ParallelBackgroundContext(
            string id, string name )
            : base( id, name )
        {
            Contexts = new List<BackgroundBaseContext>();
        }
        
        public List<BackgroundBaseContext> Contexts { get; }

        public void AddContext( BackgroundBaseContext addedContext ) => Contexts.Add( addedContext );
    }
}