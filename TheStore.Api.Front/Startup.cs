using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

using TheStore.Api.Front.Data.Repositories;

namespace TheStore.Api.Front
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
                            Title = "TheStore.Api.Front",
                            Version = "v1"
                        } );
                } );
            services.AddTransient( r => 
                new TheStoreRepository( "server=elastic.matveyko.su;user=thestore;password=moonlike-mitts-0Concord;database=theStore;", "10.3.27" ) );
                //new TheStoreRepository( "server=localhost;user=thestore;password=moonlike-mitts-0Concord;database=theStore;", "10.3.27" ) );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env )
        {
            if( env.IsDevelopment() ) {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI( c => c.SwaggerEndpoint( "/swagger/v1/swagger.json", "TheStore.Api.Front v1" ) );
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(
                endpoints => {
                    endpoints.MapControllers();
                } );
            
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
        }
    }
}