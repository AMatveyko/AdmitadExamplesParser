using System.Runtime.CompilerServices;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

[assembly:InternalsVisibleTo("AdmitadExamplesParserTests") ]
namespace TheStore.Api.Core
{
    public class Program
    {
        public static void Main(
            string[] args )
        {
            CreateHostBuilder( args ).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(
            string[] args ) =>
            Host.CreateDefaultBuilder( args ).ConfigureWebHostDefaults(
                webBuilder => {
                    webBuilder.UseStartup<Startup>();
                } );
    }
}