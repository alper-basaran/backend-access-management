using System;
using System.Collections.Generic;
using System.Text;
using static Services.Access.Domain.Model.Enums;

namespace Services.Access.Domain.Model
{
    [Serializable]
    public class Permission : BaseDomainModel
    {
        public PermissionSubjectType PermissionSubject { get; set; }
        public PermissionLevel PermissionLevel { get; set; }
        public Guid UserId { get; set; }
        public Guid ObjectId { get; set; }
    }
}
