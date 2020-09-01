using Services.Access.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static Services.Access.Domain.Model.Enums;

namespace Services.Access.Domain.Interfaces.Services
{
    public interface ILockService
    {
        Task<IEnumerable<Lock>> GetLocksOfUserAsync(Guid userId, bool grantPermission);
        Task<IEnumerable<Lock>> GetLocksOfUserBySiteAsync(Guid userId, Guid siteId, bool grantPermission);
        Task<Lock> GetLockOfUserByIdAsync(Guid userId, Guid lockId, bool grantPermission);
        LockState ActivateLock(Guid userId, Guid lockId);
    }
}
