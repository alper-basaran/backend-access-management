using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Access.Api.Model
{
    public class MessageResponse
    {
        public string Message { get; set; }
        public MessageResponse(string message)
        {
            Message = message;
        }
    }
}
