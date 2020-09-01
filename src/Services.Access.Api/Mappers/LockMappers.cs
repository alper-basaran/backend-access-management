using Microsoft.CodeAnalysis.CSharp.Syntax;
using Services.Access.Model;
using Services.Access.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Access.Api.Mappers
{
    public static class LockMappers
    {
        public static LockDto ToDto(this Lock domainLock)
        {
            if (domainLock == null)
                throw new ArgumentNullException($"Lock object cannot be null");

            return new LockDto
            {
                Id = domainLock.Id,
                Title = domainLock.Title,
                Description = domainLock.Description,
                SiteId = domainLock.SiteId,
                Created = domainLock.Created,
                Modified = domainLock.Modified
            };
        }

        public static Lock ToDomain(this LockInsertDto dto)
        {
            return new Lock
            {
                Title = dto.Title,
                Description = dto.Description,
                SiteId = dto.SiteId
            };
        }

        public static void Update(this Lock domainLock, LockUpdateDto dto)
        {
            if(dto == null)
                throw new ArgumentNullException($"Lock object cannot be null");
            
            domainLock.Title = dto.Title;
            domainLock.Description = dto.Description;
            domainLock.SiteId = dto.SiteId;
        }
    }
}
