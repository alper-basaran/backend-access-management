using Services.Access.Domain.Interfaces.Repositories;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Linq;
using Services.Access.Domain.Interfaces.Services;
using Services.Access.Domain;
using Microsoft.Extensions.Logging;
using Services.Access.Domain.Exceptions;
using System.Threading.Tasks;
using StackExchange.Redis.KeyspaceIsolation;

namespace Services.Access.Infra.RedisCache
{
    public class RedisCacheService : ICacheService
    {
        private readonly ILogger<RedisCacheService> _logger;
        private readonly RedisOptions _redisOptions;
        
        private static IDatabase _cache;
        private static IConnectionMultiplexer _connectionMultiplexer;
        private string _namespacePrefix;
        public RedisCacheService(IOptions<RedisOptions> options, ILogger<RedisCacheService> logger)
        {
            _logger = logger;
            _redisOptions = options.Value;

            var redisConfig = new ConfigurationOptions
            {
                Password = _redisOptions.Password
            };
            foreach (var endPoint in _redisOptions.Endpoints)
            {
                redisConfig.EndPoints.Add(endPoint.Host, endPoint.Port);
            }

            var mux = GetConnectionMultiplexerInstance(redisConfig);
            _cache = mux.GetDatabase();
            
        }
        public void SetNamespace(string namespaceKey)
        {
            _namespacePrefix = namespaceKey;
            _cache = _connectionMultiplexer.GetDatabase().WithKeyPrefix(namespaceKey + ":");
        }
        public T Get<T>(string key)
        {
            var redisResult = _cache.StringGet(key);
            T objResult = FromByteArray<T>(redisResult);

            if (objResult == null)
                _logger.LogInformation($"Item with {key} is not found on redis cache.");

            return objResult;
        }
        public async Task<T> GetAsync<T>(string key)
        {
            var redisResult = await _cache.StringGetAsync(key);
            T objResult = FromByteArray<T>(redisResult);

            if (objResult == null)
                _logger.LogInformation($"Item with {key} is not found on redis cache.");

            return objResult;
        }
        public void Add(string key, object value, int ttl)
        {
            var timespan = TimeSpan.FromMinutes(ttl);
            var status = _cache.StringSet(key, ObjectToByteArray(value), timespan);
            if (!status)
            {
                throw new CacheProviderException($"Failed to insert object '{value}' into redis host {_cache.IdentifyEndpoint()}");
            }
            _logger.LogInformation($"Item with {key} is added with status {status}");
        }
        public async Task AddAsync(string key, object value, int ttl)
        {
            var timespan = TimeSpan.FromMinutes(ttl);
            var status = await _cache.StringSetAsync(key, ObjectToByteArray(value), timespan);
            if (!status)
            {
                throw new CacheProviderException($"Failed to insert object '{value}' into redis host {_cache.IdentifyEndpoint()}");
            }
            _logger.LogInformation($"Item with {key} is added with status {status}");
        }
        public void Remove(string key)
        {
            var status = _cache.KeyDelete(key);
            if(status)
                _logger.LogInformation($"Item with {key} is deleted with in Redis database");
            else
                _logger.LogInformation($"Item with {key} was not found");
        }
        public async Task RemoveAsync(string key)
        {
            var status = await _cache.KeyDeleteAsync(key);
            if (status)
                _logger.LogInformation($"Item with {key} is deleted with in Redis database");
            else
                _logger.LogInformation($"Item with {key} was not found");
        }
        public void ClearNamespace()
        {
            foreach (var endpoint in _connectionMultiplexer.GetEndPoints())
            {
                var server = _connectionMultiplexer.GetServer(endpoint);
                var keys = server.Keys(pattern: _namespacePrefix + "*").ToArray();
                _cache.KeyDelete(keys);
            }
        }
        public async Task ClearNamespaceAsync()
        {
            foreach (var endpoint in _connectionMultiplexer.GetEndPoints())
            {
                var server = _connectionMultiplexer.GetServer(endpoint);
                var keys = server.Keys(pattern: _namespacePrefix + "*").ToArray();
                await _cache.KeyDeleteAsync(keys);
            }
        }
        private static T FromByteArray<T>(byte[] data)
        {
            if (data == null)
                return default(T);

            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream(data))
            {
                object obj = bf.Deserialize(ms);
                return (T)obj;
            }
        }
        private static byte[] ObjectToByteArray(object obj)
        {
            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
        private IConnectionMultiplexer GetConnectionMultiplexerInstance(ConfigurationOptions redisConfig)
        {
            if (_connectionMultiplexer != null)
                return _connectionMultiplexer;
            object _lock = new object();

            lock (_lock)
            {
                if (_connectionMultiplexer == null)
                    _connectionMultiplexer = ConnectionMultiplexer.Connect(redisConfig);
            }
            return _connectionMultiplexer;
        }

    }
}
