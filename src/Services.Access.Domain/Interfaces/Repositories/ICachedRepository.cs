using Services.Access.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Access.Domain.Interfaces.Repositories
{
    public interface ICachedRepository<T> where T: BaseDomainModel
    {
        T GetByID(Guid id);
        Task<T> GetByIDAsync(Guid id);
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
        T Insert(T obj);
        Task<T> InsertAsync(T obj);
        void Update(T obj);
        Task UpdateAsync(T obj);
        T Delete(Guid id);
        Task<T> DeleteAsync(Guid id);
    }
}
