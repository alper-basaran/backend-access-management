using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Services.Audit.Data;
using Services.Audit.Data.Repositories;
using Services.Audit.Domain.Interfaces;
using Services.Common.Auth;

namespace Services.Audit.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.Configure<MongoOptions>(options => Configuration.GetSection("Mongo").Bind(options));
            services.AddMongoDB(options => Configuration.GetSection("Mongo").Bind(options));
            services.AddControllers();
            services.AddAutoMapper(typeof(Startup));
            services.AddScoped<IEventRepository, EventRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseJwtAuthorization(options =>
            {
                options.Secret = Configuration.GetSection("JwtAuthentication").GetValue<string>("Secret");
            });
            
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
        }
    }
}
