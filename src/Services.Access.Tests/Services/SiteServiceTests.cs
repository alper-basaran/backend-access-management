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
    public class SiteServiceTests
    {
        private ISiteRepository _siteRepository;
        private IPermissionRepository _permissionRepository;

        public SiteServiceTests()
        {
            _permissionRepository = TestData.GetPermissionRepositoryMock().Object;
            _siteRepository = TestData.GetSiteRepositoryMock().Object;
        }
        
        [Fact]
        public async Task Should_Return_SpecifiedSite_If_User_Has_Permission()
        {
            var sut = new SiteService(_siteRepository, _permissionRepository);

            var userId = TestData.User2_Id;
            var siteId = TestData.Site_id;

            var actual = await sut.GetSiteOfUserAsync(userId, siteId, false);
            var expected = TestData.Site;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task Should_Return_Sites_User_Is_Authorized_To()
        {
            var sut = new SiteService(_siteRepository, _permissionRepository);

            var userId = TestData.User2_Id;
            var siteId = TestData.Site_id;

            var actual = await sut.GetSitesOfUserAsync(userId,false);
            var expected = new List<Site> { TestData.Site };

            actual.Should().Equal(expected);
        }
    }
}
