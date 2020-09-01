using System;
using System.Collections.Generic;
using System.Text;
using static Services.Access.Domain.Model.Enums;

namespace Services.Access.Domain.Model
{
    [Serializable]
    public class Site : BaseDomainModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public SiteState State { get; set; }
        public IEnumerable<Lock> Locks { get; set; }
    }
}
