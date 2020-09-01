using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static Services.Access.Domain.Model.Enums;

namespace Services.Access.Domain.Interfaces.Services
{
    public interface IDeviceBusService
    {
        LockState GetLockState(Guid id);
        LockState SetLockState(Guid id, LockState state);
        SiteState GetSiteState(Guid id);
        SiteState SetSiteState(Guid id, SiteState state);
    }
}
