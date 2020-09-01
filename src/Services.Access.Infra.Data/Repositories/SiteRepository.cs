using Microsoft.Extensions.Options;
using Services.Access.Domain.Interfaces;
using Services.Access.Domain.Interfaces.Repositories;
using Services.Access.Domain.Interfaces.Services;
using Services.Access.Domain.Model;
using Services.Access.Infra.Data.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Access.Infra.Data.Repositories
{
    public class SiteRepository : BaseCachedRepository<Site>, ISiteRepository
    {
        public SiteRepository(AccessContext context, ICacheService cacheService, IOptions<DataAccessOptions> options)
            : base(context, cacheService, options.Value.SiteTtlMinutes)
        {
        }
        public Site GetSiteByLock(Guid lockId)
        {
            return Get(filter: s => s.Locks.Any(l => l.Id == lockId)).FirstOrDefault();
        }
    }
}
