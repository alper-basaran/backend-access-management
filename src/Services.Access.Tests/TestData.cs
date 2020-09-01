using Moq;
using Services.Access.Domain.Interfaces.Repositories;
using Services.Access.Domain.Interfaces.Services;
using Services.Access.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Access.Tests
{
    public static class TestData
    {
        public static Guid User1_Id = Guid.NewGuid();
        public static Guid User2_Id = Guid.NewGuid();
        public static Guid Admin_id = Guid.NewGuid();

        public static Guid Site_id = Guid.NewGuid();
        public static Guid Lock1_id = Guid.NewGuid();
        public static Guid Lock2_id = Guid.NewGuid();

        public static Guid Lock1_User1_Permission_Id = Guid.NewGuid();
        public static Guid Site_User2_Permission_Id = Guid.NewGuid();

        public static Site Site = new Site
        {
            Id = Site_id,
            Title = "Test Site",
            Description = "Test Site",
        };

        public static Lock Lock_1 = new Lock
        {
            Id = Lock1_id,
            SiteId = Site_id,
            Title = "Lock 1",
            Description = "Lock 1"
        };
        public static Lock Lock_2 = new Lock
        {
            Id = Lock2_id,
            SiteId = Site_id,
            Title = "Lock 2",
            Description = "Lock 2"
        };

        public static Permission Lock1_User1_Permission = new Permission
        {
            Id = Lock1_User1_Permission_Id,
            UserId = User1_Id,
            ObjectId = Lock1_id,
            PermissionLevel = Enums.PermissionLevel.LockUser,
            PermissionSubject = Enums.PermissionSubjectType.Lock
        };

        public static Permission Site_User2_Permission = new Permission
        {
            Id = Site_User2_Permission_Id,
            UserId = User2_Id,
            ObjectId = Site_id,
            PermissionLevel = Enums.PermissionLevel.SiteUser,
            PermissionSubject = Enums.PermissionSubjectType.Site
        };

        public static Mock<ILockRepository> GetLockRepositoryMock()
        {
            var lockRepositoryMock = new Mock<ILockRepository>();
            var locks = GetLocks();

            lockRepositoryMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(locks);

            lockRepositoryMock.Setup(r => r.GetByIDAsync(Lock1_id))
                .ReturnsAsync(Lock_1);

            lockRepositoryMock.Setup(r => r.GetByIDAsync(Lock2_id))
                .ReturnsAsync(Lock_2);

            lockRepositoryMock.Setup(r => r.GetLocksBySiteAsync(Site_id))
                .ReturnsAsync(locks);
            
            return lockRepositoryMock;
        }
        public static Mock<ISiteRepository> GetSiteRepositoryMock()
        {
            var siteRepositoryMock = new Mock<ISiteRepository>();

            siteRepositoryMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<Site> { Site} );

            siteRepositoryMock.Setup(r => r.GetByIDAsync(Site_id))
                .ReturnsAsync(Site);

            return siteRepositoryMock;
        }
        public static Mock<IDeviceBusService> GetDeviceBusServiceMock()
        {
            var deviceBusServiceMock = new Mock<IDeviceBusService>();

            deviceBusServiceMock.Setup(s => s.SetLockState(It.IsAny<Guid>(), Enums.LockState.Unlocked))
                .Returns(Enums.LockState.Unlocked);

            deviceBusServiceMock.Setup(s => s.GetLockState(It.IsAny<Guid>()))
                .Returns(Enums.LockState.Unlocked);
            
            return deviceBusServiceMock;
        }
        public static Mock<IPermissionRepository> GetPermissionRepositoryMock()
        {
            var permissionRepositoryMock = new Mock<IPermissionRepository>();

            permissionRepositoryMock.Setup(r => r.GetPermissionByUserAsync(User1_Id))
                .ReturnsAsync(new List<Permission> { Lock1_User1_Permission });

            permissionRepositoryMock.Setup(r => r.GetPermissionByUserAsync(User2_Id))
                .ReturnsAsync(new List<Permission> { Site_User2_Permission });

            permissionRepositoryMock.Setup(r => r.GetPermissionsByLockAsync(Lock1_id))
                .ReturnsAsync(new List<Permission> { Lock1_User1_Permission });

            permissionRepositoryMock.Setup(r => r.GetPermissionsByLockAsync(Lock2_id))
                .ReturnsAsync(new List<Permission> { });

            permissionRepositoryMock.Setup(r => r.GetPermissionsBySiteAsync(Site_id))
                .ReturnsAsync(new List<Permission> { Site_User2_Permission });
            
            return permissionRepositoryMock;
        }

        public static Mock<IAuditLoggerService> GetAuditLoggerServiceMock()
        {
            var auditLoggerServiceMock = new Mock<IAuditLoggerService>();

            auditLoggerServiceMock.Setup(s => s.LogEvent(It.IsAny<AuditEvent>()))
                .Returns(Task.CompletedTask);
            
            return auditLoggerServiceMock;
        }
        public static IEnumerable<Lock> GetLocks()
        {
            var locks = new List<Lock>
            {
                Lock_1, Lock_2
            };
            return locks;
        }

        public static IEnumerable<Permission> GetPermissions()
        {
            return new List<Permission>
            {
                Lock1_User1_Permission, Site_User2_Permission
            };
        }
    }
}
