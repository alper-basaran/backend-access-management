using Services.Access.Domain.Exceptions;
using Services.Access.Domain.Interfaces.Repositories;
using Services.Access.Domain.Interfaces.Services;
using Services.Access.Domain.Model;
using Services.Access.Domain.Services;
using System;
using System.Collections.Generic;
using System.Security;
using System.Threading.Tasks;
using Moq;
using Xunit;
using static Services.Access.Domain.Model.Enums;
using Microsoft.VisualBasic;
using FluentAssertions.Collections;
using FluentAssertions;
using NUnit.Framework.Internal;

namespace Services.Access.Tests
{
    public class LockServiceTests
    {
        private ILockRepository _lockRepository;
        private IDeviceBusService _deviceBusService;
        private IPermissionRepository _permissionRepository;
        private IAuditLoggerService _auditLoggerService;

        public LockServiceTests()
        {
            var locks = TestData.GetLocks();

            _lockRepository = TestData.GetLockRepositoryMock().Object;
            _deviceBusService = TestData.GetDeviceBusServiceMock().Object;
            _permissionRepository = TestData.GetPermissionRepositoryMock().Object;
            _auditLoggerService = TestData.GetAuditLoggerServiceMock().Object;
        }
        
        [Fact]
        public void Should_ThrowExcetion_When_Unauthorized_User_Tries_Access()
        {
            var sut = new LockService(_lockRepository, _permissionRepository
                , _deviceBusService, _auditLoggerService);
            
            var userId = TestData.User1_Id;
            var lockId = TestData.Lock2_id;

            Assert.Throws<InsufficientPermissionException>(() => sut.ActivateLock(userId, lockId));
        }

        [Fact]
        public void Should_Open_Door_If_User_Has_Permission()
        {
            var sut = new LockService(_lockRepository, _permissionRepository
               , _deviceBusService, _auditLoggerService);

            var userId = TestData.User1_Id;
            var lockId = TestData.Lock1_id;

            var expected = LockState.Unlocked;
            var actual = sut.ActivateLock(userId, lockId);
            
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void Should_Return_Only_Authorized_Locks()
        {
            var sut = new LockService(_lockRepository, _permissionRepository
               , _deviceBusService, _auditLoggerService);

            var userId = TestData.User1_Id;

            var expected = new List<Lock> { TestData.Lock_1 };
            var actual = await sut.GetLocksOfUserAsync(userId, false);

            actual.Should().Equal(expected);
        }
        [Fact]
        public async void Should_Return_Only_LocksOf_Authorized_Sites()
        {
            var sut = new LockService(_lockRepository, _permissionRepository
               , _deviceBusService, _auditLoggerService);

            var userId = TestData.User2_Id;
            var siteId = TestData.Site_id;

            var expected = TestData.GetLocks();
            var actual = await sut.GetLocksOfUserBySiteAsync(userId, siteId, false);

            actual.Should().Equal(expected);
        }
    }
}
