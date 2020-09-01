using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Access.Domain.Model
{
    [Serializable]
    public abstract class BaseDomainModel
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
