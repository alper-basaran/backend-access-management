using Services.Access.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Access.Domain.Interfaces.Repositories
{
    public interface ISiteRepository : ICachedRepository<Site>
    {
        Site GetSiteByLock(Guid lockId);
    }
}
