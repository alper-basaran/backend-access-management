using System;
using System.ComponentModel.DataAnnotations;

namespace Services.Access.Model
{
    public class SiteUpsertDto
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
