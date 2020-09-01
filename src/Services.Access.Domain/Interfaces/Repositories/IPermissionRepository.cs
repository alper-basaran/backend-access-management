using Services.Access.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Access.Domain.Interfaces.Repositories
{
    public interface IPermissionRepository : ICachedRepository<Permission>
    {
        IEnumerable<Permission> GetPermissionsByLock(Guid lockId);
        Task<IEnumerable<Permission>> GetPermissionsByLockAsync(Guid lockId);
        IEnumerable<Permission> GetPermissionsBySite(Guid siteId);
        Task<IEnumerable<Permission>> GetPermissionsBySiteAsync(Guid siteId);
        IEnumerable<Permission> GetPermissionByUser(Guid userId);
        Task<IEnumerable<Permission>> GetPermissionByUserAsync(Guid userId);
    }
}
