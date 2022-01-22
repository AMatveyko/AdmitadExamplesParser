// a.snegovoy@gmail.com

using System.Linq;
using System.Threading.Tasks;

using AdmitadSqlData.Helpers;

using Common.Api;
using Common.Elastic.Workers;
using Common.Workers;

namespace WorkWithTags
{
    internal static class TagsStatistics
    {
        public static void GetStatistics()
        {
            var dbSettings = SettingsBuilder.GetDbSettings();
            var db = new DbHelper( dbSettings );
            // var tagsIds = db.GetTags().Where( t => t.AddDate == 20220122 ).Select( t => t.Id );
            var tagsIds = db.GetTags().Select( t => t.Id );
            var indexSettings = new SettingsBuilder( db ).GetSettings().ElasticSearchClientSettings;
            var index = IndexClient.CreateTagsWorker( indexSettings, new BackgroundBaseContext( "", "" ) );
            var tasks = tagsIds.Select( t => Task.Factory.StartNew( () => index.GetProductsCountWithTag( t ) ) )
                .ToArray();
            Task.WaitAll( tasks );
            var result = tasks.Select( t => t.Result ).ToList();
        }
    }
}