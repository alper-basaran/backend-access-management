using Services.Access.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Access.Domain.Interfaces.Services
{
    public interface IAuditLoggerService
    {
        Task LogEvent(AuditEvent auditEvent);
    }
}
