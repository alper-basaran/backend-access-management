using Microsoft.Extensions.Options;
using Services.Access.Domain.Interfaces;
using Services.Access.Domain.Interfaces.Repositories;
using Services.Access.Domain.Interfaces.Services;
using Services.Access.Domain.Model;
using Services.Access.Infra.Data.Database;
using System;
using System.Collections.Generic;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using static Services.Access.Domain.Model.Enums;

namespace Services.Access.Infra.Data.Repositories
{
    public class PermissionRepository : BaseCachedRepository<Permission>, IPermissionRepository
    {
        public PermissionRepository(AccessContext context, ICacheService cacheService, IOptions<DataAccessOptions> options) 
            : base(context, cacheService, options.Value.PermissionTtlMinutes)
        {
        }

        public IEnumerable<Permission> GetPermissionByUser(Guid userId)
        {
            return Get(filter: p => p.UserId == userId);
        }

        public async Task<IEnumerable<Permission>> GetPermissionByUserAsync(Guid userId)
        {
            return await GetAsync(filter: p => p.UserId == userId);
        }

        public IEnumerable<Permission> GetPermissionsByLock(Guid lockId)
        {
            return Get(filter: p => p.PermissionSubject == PermissionSubjectType.Lock && p.ObjectId == lockId);
        }

        public async Task<IEnumerable<Permission>> GetPermissionsByLockAsync(Guid lockId)
        {
            return await GetAsync(filter: p => p.PermissionSubject == PermissionSubjectType.Lock && p.ObjectId == lockId);
        }

        public IEnumerable<Permission> GetPermissionsBySite(Guid siteId)
        {
            return Get(filter: p => p.PermissionSubject == PermissionSubjectType.Site && p.ObjectId == siteId);
        }

        public async Task<IEnumerable<Permission>> GetPermissionsBySiteAsync(Guid siteId)
        {
            return await GetAsync(filter: p => p.PermissionSubject == PermissionSubjectType.Site && p.ObjectId == siteId);
        }
    }
}
