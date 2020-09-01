using System;
using static Services.Access.Domain.Model.Enums;

namespace Services.Access.Model
{
    public class SiteDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public SiteState State { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
