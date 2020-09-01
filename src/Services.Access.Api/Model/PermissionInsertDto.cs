using Microsoft.AspNetCore.Mvc;
using Services.Access.Api.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using static Services.Access.Domain.Model.Enums;

namespace Services.Access.Model
{
    public class PermissionInsertDto
    {
        [Required]
        public PermissionSubjectType PermissionSubject { get; set; }
        [Required]
        public PermissionLevel PermissionLevel { get; set; }
        [ValidGuid(required: true)]
        public Guid UserId { get; set; }
        [ValidGuid(required: true)]
        public Guid ObjectId { get; set; }
    }
}
