using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Access.Domain.Interfaces.Services
{
    public interface ICacheService
    {
        void SetNamespace(string namespaceKey);
        T Get<T>(string key);
        Task<T> GetAsync<T>(string key);
        void Add(string key, object value, int ttl);
        Task AddAsync(string key, object value, int ttl);
        void Remove(string key);
        Task RemoveAsync(string key);
        void ClearNamespace();
        Task ClearNamespaceAsync();
    }
}
