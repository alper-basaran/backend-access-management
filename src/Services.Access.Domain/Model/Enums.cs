using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Access.Domain.Model
{
    public class Enums
    {
        public enum LockState
        {
            Undefined,
            Offline,
            Unlocked,
            Locked,
            Error
        }
        public enum SiteState
        {
            Undefined,
            Online,
            Offline,
            Error
        }
        public enum PermissionSubjectType
        {
            Undefined,
            Lock,
            Site
        }
        public enum PermissionLevel
        {
            LockUser,
            LockAdministrator,
            SiteUser,
            SiteAdministrator,
            SystemAdministrator
        }
    }
}
