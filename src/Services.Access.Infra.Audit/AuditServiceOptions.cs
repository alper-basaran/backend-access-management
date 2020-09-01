using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Access.Infra.Audit
{
    public class AuditServiceOptions
    {
        public string ApiKey { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Endpoint { get; set; }
    }
}
