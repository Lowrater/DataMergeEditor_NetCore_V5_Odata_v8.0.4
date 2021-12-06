using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using OdataApi_V5.Contexts;
using OdataApi_V5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OdataApi_V5
{
    public class Startup
    { 
        /// <summary>
        /// Connectionstring dummy for localhost
        /// </summary>
        const string Conn = "Server=localhost;Database=master;Trusted_Connection=true;";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder => { builder.AddDebug(); });

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddLogging(c => c.AddConsole());

            // Database reference
            services.AddDbContext<UsersContext>(options => options.UseSqlServer(Conn).UseLoggerFactory(MyLoggerFactory)
#if DEBUG
                .EnableSensitiveDataLogging(true)
#endif
            );

            // Adds the EDM models and query options for select, expand, filter, order, set & count
            services.AddControllers().AddOData(opt => opt.AddRouteComponents("odata", GetEdmModel())
                                                         .Select()
                                                         .Expand()
                                                         .Filter()
                                                         .OrderBy()
                                                         .SetMaxTop(100)
                                                         .Count()
                                     );

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "OdataApi_V5", Version = "v1" });
            });
        }

        /// <summary>
        /// Describes the structure of data
        /// Based on Entity framework
        /// </summary>
        /// <returns></returns>
        private IEdmModel GetEdmModel()
        {
            // Builder
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();

            // ADding Controllers with  key field in order to get controller
            builder.EntitySet<Users>("Users");

            // returns the edmmodel
            return builder.GetEdmModel();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Use odata route debug, /$odata
            app.UseODataRouteDebug();

            // If you want to use /$openapi, enable the middleware.
            //app.UseODataOpenApi();

            // Add OData /$query middleware
            app.UseODataQueryRequest();

            // Add the OData Batch middleware to support OData $Batch
            app.UseODataBatching();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OdataApi_V5 v1"));

            app.UseRouting();
            //app.UseAuthorization();

            // Test middleware
            app.Use(next => context =>
            {
                var endpoint = context.GetEndpoint();
                if (endpoint == null)
                {
                    return next(context);
                }

                return next(context);
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
