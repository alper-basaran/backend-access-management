using Microsoft.CodeAnalysis.CSharp.Syntax;
using Services.Access.Model;
using Services.Access.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Access.Api.Mappers
{
    public static class PermissionMappers
    {
        public static PermissionDto ToDto (this Permission domainPermission)
        {
            if (domainPermission== null)
                throw new ArgumentNullException($"Permission cannot be null");

            return new PermissionDto
            {
                Id = domainPermission.Id,
                PermissionSubject = domainPermission.PermissionSubject,
                PermissionLevel = domainPermission.PermissionLevel,
                UserId = domainPermission.UserId,
                ObjectId = domainPermission.ObjectId,
                Created = domainPermission.Created,
                Modified = domainPermission.Modified
            };
        }

        public static Permission ToDomain(this PermissionInsertDto dto)
        {
            return new Permission
            {
                PermissionSubject = dto.PermissionSubject,
                PermissionLevel = dto.PermissionLevel,
                UserId = dto.UserId,
                ObjectId = dto.ObjectId
            };
        }
    }
}
