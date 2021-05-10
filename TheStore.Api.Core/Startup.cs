using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Admitad.Converters;

using AdmitadCommon.Entities;
using AdmitadCommon.Helpers;
using AdmitadCommon.Types;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

using TheStore.Api.Core.Sources.Workers;

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
            services.AddTransient( provider => SettingsBuilder.GetSettings() );
            services.AddSingleton<PriorityQueue>();
            services.AddSingleton<BackgroundWorks>();
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