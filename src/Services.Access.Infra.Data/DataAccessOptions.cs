using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Access.Infra.Data
{
    public class DataAccessOptions
    {
        public int LockTtlMinutes { get; set; }
        public int SiteTtlMinutes { get; set; }
        public int PermissionTtlMinutes { get; set; }
    }
}
