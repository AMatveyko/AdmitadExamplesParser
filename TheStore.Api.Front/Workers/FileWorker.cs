// a.snegovoy@gmail.com

using System.IO;
using System.Linq;

using Microsoft.AspNetCore.Mvc;

using NLog;

using Web.Common.Entities.Enums;
using Web.Common.Entities.Responses;

namespace TheStore.Api.Front.Workers
{
    internal static class FileWorker
    {
        
        private static readonly Logger LoggerError = LogManager.GetLogger( "ReadFileError" );
        
        public static IActionResult GetPageStatistics()
        {
            const string path = "utils_v2/top_listings_full.csv";
            //const string path = @"o:\AdmitadExamplesParser\TheStore.Api.Front\bin\Debug\netcoreapp3.1\top_listings_full.csv";
            var response = new PagesStatisticsResponse { Error = ErrorCode.Ok.ToString() };
            if( File.Exists( path ) == false ) {
                response.Error = ErrorCode.Error.ToString();
                response.ErrorMessage = $"{path} NotFound";
                LoggerError.Error( response.ErrorMessage );
                return new JsonResult( response );
            }

            response.Lines = File.ReadAllLines( path ).ToList();
            return new JsonResult( response );
        } 
    }
}