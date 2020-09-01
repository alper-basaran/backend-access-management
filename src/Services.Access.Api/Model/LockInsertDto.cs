using Services.Access.Api.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace Services.Access.Model
{
    public class LockInsertDto
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [ValidGuid(required: true)]
        public Guid SiteId { get; set; }
    }
}
