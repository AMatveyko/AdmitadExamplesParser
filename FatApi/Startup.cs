using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ElasticSearchData;

using FatApi.Workers;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace FatApi
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

            var config = Configuration.GetSection( "Elastic" );
            var url = config.GetChildren().First( c => c.Key == "Url" ).Value;
            services.AddControllers();
            services.AddSwaggerGen(
                c => {
                    c.SwaggerDoc(
                        "v1",
                        new OpenApiInfo {
                            Title = "FatApi",
                            Version = "v1"
                        } );
                } );
            services.AddScoped<IOfferRepository, OfferRepository>( r => new OfferRepository( url ) );
            services.AddScoped<IProductRepository, ProductRepository>( r => new ProductRepository( url ) );
            services.AddScoped<IProductFactory, ProductFactory>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env )
        {
            if( env.IsDevelopment() ) {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI( c => c.SwaggerEndpoint( "/swagger/v1/swagger.json", "FatApi v1" ) );
            }

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