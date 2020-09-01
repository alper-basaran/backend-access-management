using System;

namespace Services.Audit.Domain.Model
{
    public class Event
    {
        public Guid EventId { get; set; }
        public Guid UserId { get; set; }
        public Guid ObjectId { get; set; }
        public string EventKey { get; set; }
        public DateTime Created { get; set; }
    }
}
