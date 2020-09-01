using Services.Common.Auth.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Audit.Api.Model
{
    public class EventInsertDto
    {
        public Guid UserId { get; set; }
        public Guid ObjectId { get; set; }
        public string EventKey { get; set; }

        public override string ToString()
        {
            return $"Event UserId: {UserId}, ObjectId: {ObjectId}, EventKey: {EventKey}";
        }
    }
}
