using Microsoft.CodeAnalysis.CSharp.Syntax;
using Services.Access.Model;
using Services.Access.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Access.Api.Mappers
{
    public static class SiteMappers
    {
        public static SiteDto ToDto(this Site domainSite)
        {
            if (domainSite == null)
                throw new ArgumentNullException($"Domain object cannot be null");

            return new SiteDto
            {
                Id = domainSite.Id,
                Title = domainSite.Title,
                Description = domainSite.Description,
                Created = domainSite.Created,
                Modified = domainSite.Modified
            };
        }

        public static Site ToDomain(this SiteUpsertDto dto)
        {
            return new Site 
            {
                Title = dto.Title,
                Description = dto.Description
            };
        }

        public static void Update(this Site site, SiteUpsertDto dto)
        {
            if(dto == null)
                throw new ArgumentNullException($"Dto object cannot be null");
            if (site == null)
                throw new ArgumentNullException($"Domain object cannot be null");
            
            site.Title = dto.Title;
            site.Description = dto.Description;
        }
    }
}
