using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using static Services.Access.Domain.Model.Enums;

namespace Services.Access.Domain.Model
{
    [Serializable]
    public class Lock : BaseDomainModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public LockState State { get; set; }
        public Guid SiteId { get; set; }
        public Site Site { get; set; }
    }
}
