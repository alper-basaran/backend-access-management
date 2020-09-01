using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Services.Access.Domain;
using Services.Access.Domain.Interfaces;
using Services.Access.Domain.Interfaces.Repositories;
using Services.Access.Domain.Interfaces.Services;
using Services.Access.Domain.Services;
using Services.Access.Infra.Audit;
using Services.Access.Infra.Data;
using Services.Access.Infra.Data.Database;
using Services.Access.Infra.Data.Repositories;
using Services.Access.Infra.RedisCache;
using Services.Common.Auth;

namespace Services.Access
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

            services.Configure<RedisOptions>(options => Configuration.GetSection("Redis").Bind(options));
            services.Configure<AuditServiceOptions>(options => Configuration.GetSection("AuditServiceOptions").Bind(options));

            services.Configure<DataAccessOptions>(options => Configuration.GetSection("DataAccessOptions").Bind(options));

            services.AddControllers()
                .AddJsonOptions(options => 
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            var connectionString = Configuration.GetConnectionString("AccessServiceDb");
            services.AddDbContext<AccessContext>(options => options.UseSqlServer(connectionString));
            
            services.AddScoped<ILockRepository, LockRepository>();
            services.AddScoped<ISiteRepository, SiteRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<ILockService, LockService>();
            services.AddScoped<ISiteService, SiteService>();
            
            services.AddSingleton<IDeviceBusService, DeviceBusService>();
            
            services.AddTransient<ICacheService, RedisCacheService>();
            services.AddScoped<IAuditLoggerService, AuditLoggerService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseJwtAuthorization(options => 
            {
                options.Secret = Configuration.GetSection("JwtAuthentication").GetValue<string>("Secret");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
