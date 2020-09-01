using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Access.Api.Model
{
    public interface IPaginatedResult
    {
        public string current { get; set; }
        public string First { get; set; }
        public string Last { get; set; }
        public string Next { get; set; }
        public string Previous { get; set; }
    }
}
