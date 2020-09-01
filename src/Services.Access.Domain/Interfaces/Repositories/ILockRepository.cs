using Services.Access.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Access.Domain.Interfaces.Repositories
{
    public interface ILockRepository : ICachedRepository<Lock>
    {
        IEnumerable<Lock> GetLocksBySite(Guid siteId);
        Task<IEnumerable<Lock>> GetLocksBySiteAsync(Guid siteId);
    }
}
