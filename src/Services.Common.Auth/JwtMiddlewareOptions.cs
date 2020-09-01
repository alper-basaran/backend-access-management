using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Common.Auth
{
    public class JwtMiddlewareOptions
    {
        public string Secret { get; set; }
    }
}
