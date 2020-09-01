using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;

namespace Services.Audit.Data
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMongoDB(this IServiceCollection services, Action<MongoOptions> configureOptions)
        {
            var options = new MongoOptions();
            configureOptions(options);
            services.AddSingleton<IDatabaseInitializer, MongoInitializer>();
            
            services.AddSingleton<MongoClient>(c => 
            {
                var connectionString = $"mongodb://{options.User}:{options.Password}@{options.Host}:{options.Port}";
            
                return new MongoClient(connectionString);
            });
            services.AddScoped<IMongoDatabase>(c => 
            {
                var client = c.GetService<MongoClient>();
                var db =  client.GetDatabase(options.Database);
                c.GetService<IDatabaseInitializer>().Initialize();
                return db;
            });
        }        
    }
}