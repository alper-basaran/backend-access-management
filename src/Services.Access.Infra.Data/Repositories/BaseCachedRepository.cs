using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Services.Access.Domain.Interfaces;
using Services.Access.Domain.Interfaces.Repositories;
using Services.Access.Domain.Interfaces.Services;
using Services.Access.Domain.Model;
using Services.Access.Infra.Data.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Access.Infra.Data.Repositories
{
    public class BaseCachedRepository<T> : ICachedRepository<T> where T: BaseDomainModel
    {
        internal readonly DbContext _context;
        internal readonly DbSet<T> _dbSet;
        private readonly ICacheService _cacheService;
        private readonly int _cacheDuration;

        public BaseCachedRepository(DbContext context, ICacheService cacheService, int ttlMinutes)
        {
            _context = context;
            _dbSet = _context.Set<T>();
            _cacheService = cacheService;
            _cacheService.SetNamespace("CachedEntity");
            _cacheDuration = ttlMinutes;
        }
        
        public T GetByID(Guid id)
        {
            T result = _cacheService.Get<T>(id.ToString());
            if (result != null)
                return result;

            result = _dbSet.Find(id);
            if(result != null)
                _cacheService.Add(id.ToString(), result, _cacheDuration);
            return result;
        }
        public async Task<T> GetByIDAsync(Guid id)
        {
            T result = await _cacheService.GetAsync<T>(id.ToString());
            if (result != null)
                return result;

            result = await _dbSet.FindAsync(id);
            if (result != null)
                await _cacheService.AddAsync(id.ToString(), result, _cacheDuration);
            return result;
        }
        public IEnumerable<T> GetAll()
        {
            return Get();
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await GetAsync();
        }
        public T Delete(Guid id)
        {
            _cacheService.Remove(id.ToString());
            T entityToDelete = _dbSet.Find(id);
            if (entityToDelete == null)
            {
                return null;
            }

            Delete(entityToDelete);
            _context.SaveChanges();
            
            return entityToDelete;
        }
        public async Task<T> DeleteAsync(Guid id)
        {
            await _cacheService.RemoveAsync(id.ToString());
            T entityToDelete = _dbSet.Find(id);
            if (entityToDelete == null)
            {
                return null;
            }

            Delete(entityToDelete);
            await _context.SaveChangesAsync();

            return entityToDelete;
        }
        private void Delete(T entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
        }
        public virtual T Insert(T entityToInsert)
        {
            entityToInsert.Id = Guid.NewGuid();
            entityToInsert.Created = DateTime.UtcNow;
            entityToInsert.Modified = DateTime.UtcNow;

            _dbSet.Add(entityToInsert);
            _context.SaveChanges();

            return entityToInsert;
        }
        public async Task<T> InsertAsync(T entityToInsert)
        {
            entityToInsert.Id = Guid.NewGuid();
            entityToInsert.Created = DateTime.UtcNow;
            entityToInsert.Modified = DateTime.UtcNow;

            await _dbSet.AddAsync(entityToInsert);
            await _context.SaveChangesAsync();

            return entityToInsert;
        }
        public virtual void Update(T entityToUpdate)
        {
            entityToUpdate.Modified = DateTime.UtcNow;
            _cacheService.Remove(entityToUpdate.Id.ToString());
            
            _dbSet.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
            _context.SaveChanges();
        }
        public async Task UpdateAsync(T entityToUpdate)
        {
            entityToUpdate.Modified = DateTime.UtcNow;
            _cacheService.Remove(entityToUpdate.Id.ToString());

            _dbSet.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        private protected IEnumerable<T> Get(Expression<Func<T, bool>> filter = null, string includeProperties = "")
        {
            IQueryable<T> query = _dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return query.ToList();
        }
        private async protected Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> filter = null, string includeProperties = "")
        {
            IQueryable<T> query = _dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return await query.ToListAsync();
        }





      

        

       
    }
}
