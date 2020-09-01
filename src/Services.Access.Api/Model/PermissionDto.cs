using System;
using static Services.Access.Domain.Model.Enums;

namespace Services.Access.Model
{
    public class PermissionDto
    {
        public Guid Id { get; set; }
        public PermissionSubjectType PermissionSubject { get; set; }
        public PermissionLevel PermissionLevel { get; set; }
        public Guid UserId { get; set; }
        public Guid ObjectId { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
