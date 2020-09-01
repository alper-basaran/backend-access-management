using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Access.Domain.Model
{
    public class AuditEvent
    {
        public Guid UserId { get; set; }
        public Guid ObjectId { get; set; }
        public string EventKey { get; set; }
    }
    public static class AuditEventKeys
    {
        public const string SuccessLockActivated = "Success_Lock_Activated";
        public const string FailureLockActivated = "Failure_Lock_Activated";
        public const string LockPermissionGranted = "Lock_PermissionGranted";
        public const string SitePermissionGranted = "Site_PermissionGranted";
    }
}
