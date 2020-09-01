using Services.Access.Api.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace Services.Access.Model
{
    public class LockUpdateDto
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [ValidGuid(required: false)]
        public Guid SiteId { get; set; }
    }
}
