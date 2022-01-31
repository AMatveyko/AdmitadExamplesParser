// a.snegovoy@gmail.com

using System;

using Common.Api;
using Common.Settings;

using Nest;

namespace Common.Elastic.Workers
{
    public abstract class IndexClientBase
    {
        
        #region Data

        protected readonly ElasticClient Client;
        protected readonly BackgroundBaseContext Context;
        protected readonly ElasticSearchClientSettings Settings;

        #endregion
        
        #region Ctors

        protected IndexClientBase( ElasticSearchClientSettings settings, BackgroundBaseContext context, string indexName )
        {
            var clientSettings =
                new ConnectionSettings( new Uri( settings.ElasticSearchUrl ) )
                    .RequestTimeout(TimeSpan.FromMinutes(20))
                    .DefaultIndex( indexName );
            Client = new ElasticClient( clientSettings );
            Context = context;
            Settings = settings;
            Setup();
        }

        #endregion
        
        #region Initialization

        private void Setup()
        {
            Client.Cluster.PutSettings(
                descriptor => descriptor.Transient( f => f.Add( "script.max_compilations_rate", "10000/1m" ) ) );
        }

        #endregion
        
    }
}