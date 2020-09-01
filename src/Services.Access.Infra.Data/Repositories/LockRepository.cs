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
using System.Threading.Tasks;

namespace Services.Access.Infra.Data.Repositories
{
    public class LockRepository : BaseCachedRepository<Lock>, ILockRepository
    {
        public LockRepository(AccessContext context, ICacheService cacheService, IOptions<DataAccessOptions> options) 
            : base(context, cacheService, options.Value.LockTtlMinutes)
        {
        }

        public IEnumerable<Lock> GetLocksBySite(Guid siteId)
        {
            return Get(filter: l => l.SiteId == siteId);
        }

        public async Task<IEnumerable<Lock>> GetLocksBySiteAsync(Guid siteId)
        {
            return await GetAsync(filter: l => l.SiteId == siteId);
        }
    }
}
