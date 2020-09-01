using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;

namespace Services.Common.Auth.Model
{
    public class User
    {
        public Guid Id { get; set; }
        public string[] Roles { get; set; }

        public bool IsAdministrator()
        {
            return Roles.Contains(RoleNames.Administrator);
        }
    }
}
