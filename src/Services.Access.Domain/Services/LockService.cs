using Services.Access.Domain.Exceptions;
using Services.Access.Domain.Helpers;
using Services.Access.Domain.Interfaces.Repositories;
using Services.Access.Domain.Interfaces.Services;
using Services.Access.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Services.Access.Domain.Model.Enums;

namespace Services.Access.Domain.Services
{
    public class LockService : ILockService
    {
        private readonly ILockRepository _lockRepository;
        private readonly IDeviceBusService _deviceBusService;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IAuditLoggerService _auditLoggerService;

        public LockService(ILockRepository lockRepository,IPermissionRepository permissionRepository,
            IDeviceBusService deviceBusService, IAuditLoggerService auditLoggerService)
        {
            _lockRepository = lockRepository;
            _permissionRepository = permissionRepository;
            _deviceBusService = deviceBusService;
            _auditLoggerService = auditLoggerService;

        }

        public async Task<IEnumerable<Lock>> GetLocksOfUserAsync(Guid userId, bool grantPermission)
        {
            userId.Validate();
            List<Lock> locks;
            if (grantPermission)
            {
                locks = (await _lockRepository.GetAllAsync()).ToList();
            }
            else
            {
                var permissions = (await _permissionRepository.GetPermissionByUserAsync(userId))
               .Where(p => p.PermissionSubject == PermissionSubjectType.Lock)
               .ToList();
                var lockIds = permissions.Select(p => p.ObjectId).ToArray();

                locks = new List<Lock>(lockIds.Length);

                foreach (var id in lockIds)
                {
                    var lockObject = await _lockRepository.GetByIDAsync(id);
                    locks.Add(lockObject);
                }
            }
            return locks;
        }
        public async Task<Lock> GetLockOfUserByIdAsync(Guid userId, Guid lockId, bool grantPermission)
        {
            Lock lockObj;
            userId.Validate();
            lockId.Validate();

            if (grantPermission)
            {
                lockObj = await _lockRepository.GetByIDAsync(lockId);
            }
            else
            {
                if (!(await HasLockPermission(userId, lockId)))
                {
                    throw new InsufficientPermissionException(userId, lockId, PermissionLevel.LockUser);
                }
                lockObj = await _lockRepository.GetByIDAsync(lockId);
            }
            if (lockObj == null)
                    throw new LockNotFoundException(lockId);
            return lockObj;
        }
        public async Task<IEnumerable<Lock>> GetLocksOfUserBySiteAsync(Guid userId, Guid siteId, bool grantPermission)
        {
            userId.Validate();
            siteId.Validate();
            List<Lock> locks = new List<Lock>();
            if (grantPermission)
            {
                locks = (await _lockRepository.GetLocksBySiteAsync(siteId)).ToList();
            }
            else
            {
                var permissions = (await _permissionRepository.GetPermissionByUserAsync(userId))
                .Where(p => p.PermissionSubject == PermissionSubjectType.Site)
                .ToList();
                var siteIds = permissions.Select(p => p.ObjectId).ToArray();

                var lockIds = new List<Guid>();
                foreach (var site in siteIds)
                {
                    var locksOfSite = (await _lockRepository.GetLocksBySiteAsync(site)).ToList();
                    locks.AddRange(locksOfSite);
                }
            }
            return locks;
        }
        public LockState ActivateLock(Guid userId, Guid lockId)
        {
            if (!HasLockPermission(userId, lockId).Result)
            {
                throw new InsufficientPermissionException(userId, lockId, PermissionLevel.LockUser);
            }
            _deviceBusService.SetLockState(lockId, LockState.Unlocked);//Set
            var state = _deviceBusService.GetLockState(lockId);//Confirm
            var evt = new AuditEvent
            {
                ObjectId = lockId,
                UserId = userId
            };
            
            if (state == LockState.Unlocked)
                evt.EventKey = AuditEventKeys.SuccessLockActivated;
            else
                evt.EventKey = AuditEventKeys.FailureLockActivated;

            _auditLoggerService.LogEvent(evt);
            return state;
        }
        private async Task<bool> HasLockPermission(Guid userId, Guid lockId)
        {
            userId.Validate();
            lockId.Validate();

            var lockObject = await _lockRepository.GetByIDAsync(lockId);
            if (lockObject == null)
                throw new LockNotFoundException(lockId);
            
            var hasLockPermission = (await _permissionRepository.GetPermissionsByLockAsync(lockId))
                .Any(p => p.UserId == userId);

            var siteId = lockObject.SiteId;
            
            var hasSitePermission = (await _permissionRepository.GetPermissionsBySiteAsync(siteId))
                .Any(p => p.UserId == userId);

            return hasLockPermission || hasSitePermission;
        }
        private async Task<bool> HasLockAdminPermission(Guid userId, Guid lockId)
        {
            userId.Validate();
            lockId.Validate();

            var lockObject = await _lockRepository.GetByIDAsync(lockId);
            if (lockObject == null)
                throw new LockNotFoundException(lockId);

            var hasLockAdminPermission = (await _permissionRepository.GetPermissionsByLockAsync(lockId))
                .Any(p => p.UserId == userId && p.PermissionLevel == PermissionLevel.LockAdministrator);

            var siteId = lockObject.SiteId;

            var hasSiteAdminPermission = (await _permissionRepository.GetPermissionsBySiteAsync(siteId))
                .Any(p => p.UserId == userId && p.PermissionLevel == PermissionLevel.SiteAdministrator);

            return hasLockAdminPermission || hasSiteAdminPermission;
        }
    }
}
