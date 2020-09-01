using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Common.Auth
{
    public static class JwtMiddlewareExtensions
    {
        public static IApplicationBuilder UseJwtAuthorization(
               this IApplicationBuilder builder, Action<JwtMiddlewareOptions> configureOptions)
        {
            var options = new JwtMiddlewareOptions();
            configureOptions(options);
            return builder.UseMiddleware<JwtMiddleware>(options);
        }
    }
}
