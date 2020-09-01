using Services.Access.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Access.Domain.Interfaces.Services
{
    public interface ISiteService
    {
        Task<Site> GetSiteOfUserAsync(Guid userId, Guid siteId, bool grantPermission);
        Task<IEnumerable<Site>> GetSitesOfUserAsync(Guid userId, bool grantPermission);
    }
}
