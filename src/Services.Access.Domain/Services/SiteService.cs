using Services.Access.Domain.Interfaces.Repositories;
using Services.Access.Domain.Interfaces.Services;
using Services.Access.Domain.Helpers;
using Services.Access.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static Services.Access.Domain.Model.Enums;
using System.Linq;
using Services.Access.Domain.Exceptions;

namespace Services.Access.Domain.Services
{
    public class SiteService : ISiteService
    {
        private readonly ISiteRepository _siteRepository;
        private readonly IPermissionRepository _permissionRepository;
        public SiteService(ISiteRepository siteRepository, IPermissionRepository permissionRepository)
        {
            _siteRepository = siteRepository;
            _permissionRepository = permissionRepository;
        }
        public async Task<IEnumerable<Site>> GetSitesOfUserAsync(Guid userId, bool grantPermission)
        {
            userId.Validate();

            List<Site> sites = new List<Site>();
            if (grantPermission)
            {
                sites = (await _siteRepository.GetAllAsync()).ToList();
            }
            else
            {
                var permissions = (await _permissionRepository.GetPermissionByUserAsync(userId))
                .Where(p => p.PermissionSubject == PermissionSubjectType.Site)
                .ToList();

                var siteIds = permissions.Select(p => p.ObjectId);

                foreach (var id in siteIds)
                {
                    var site = await _siteRepository.GetByIDAsync(id);
                    sites.Add(site);
                }
            }

            return sites;
        }
        public async Task<Site> GetSiteOfUserAsync(Guid userId, Guid siteId, bool grantPermission)
        {
            userId.Validate();
            siteId.Validate();
            Site site;

            if (grantPermission)
            {
                site = await _siteRepository.GetByIDAsync(siteId);
            }
            else
            {
                if (!(await HasSitePermission(userId, siteId)))
                {
                    throw new InsufficientPermissionException(userId, siteId, PermissionLevel.SiteUser);
                }
                site = await _siteRepository.GetByIDAsync(siteId);
                if (siteId == null)
                    throw new SiteNotFoundException(siteId);
            }

            return site;
        }
        private async Task<bool> HasSitePermission(Guid userId, Guid siteId)
        {
            userId.Validate();
            siteId.Validate();

            var hasPermission = (await _permissionRepository.GetPermissionsBySiteAsync(siteId))
                 .Any(p => p.UserId == userId);

            return hasPermission;
        }
        private async Task<bool> HasSiteAdminPermission(Guid userId, Guid siteId)
        {
            userId.Validate();
            siteId.Validate();

            var hasPermission = (await _permissionRepository.GetPermissionsBySiteAsync(siteId))
                 .Any(p => p.UserId == userId && p.PermissionLevel == PermissionLevel.SiteAdministrator);

            return hasPermission;
        }
    }
}
