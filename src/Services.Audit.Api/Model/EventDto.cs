using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Audit.Api.Model
{
    public class EventDto
    {
        public Guid EventId { get; set; }
        public Guid UserId { get; set; }
        public Guid ObjectId { get; set; }
        public string EventKey { get; set; }
        public DateTime Created { get; set; }

        public override string ToString()
        {
            return $"EventId: {EventId},  UserId: {UserId}, ObjectId: {ObjectId}, EventKey: {EventKey}, Created: {Created}";
        }
    }
}
