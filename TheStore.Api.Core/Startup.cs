using Admitad.Converters.Workers;
using AdmitadCommon.Types;

using AdmitadSqlData.Helpers;

using Common.Workers;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

using TheStore.Api.Core.Sources.Workers;
using TheStore.Api.Front.Data.Repositories;

using TheStoreRepositoryFromFront = TheStore.Api.Front.Data.Repositories.TheStoreRepository;

namespace TheStore.Api.Core
{
    public class Startup
    {
        public Startup(
            IConfiguration configuration )
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(
            IServiceCollection services )
        {
            var dbSettings = SettingsBuilder.GetDbSettings();
            services.AddControllers();
            services.AddSwaggerGen(
                c => {
                    c.SwaggerDoc(
                        "v1",
                        new OpenApiInfo {
                            Title = "TheStore.Api.Core",
                            Version = "v1"
                        } );
                } );
            var settingsBuilder = new SettingsBuilder( new DbHelper(dbSettings) );
            var settings = settingsBuilder.GetSettings();
            services.AddScoped( provider => settings );
            services.AddSingleton<PriorityQueue>();
            services.AddSingleton<BackgroundWorks>();
            services.AddScoped( r => new TheStoreRepository( dbSettings ) );
            services.AddSingleton(r => new ProductRatingCalculation(new TheStoreRepositoryFromFront(dbSettings), settings.CtrCalculationType) );
            services.AddScoped( r => new DbHelper( dbSettings ) );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env )
        {
            if( env.IsDevelopment() ) {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI( c => c.SwaggerEndpoint( "/swagger/v1/swagger.json", "TheStore.Api.Core v1" ) );
            
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(
                endpoints => {
                    endpoints.MapControllers();
                } );
        }
    }
}