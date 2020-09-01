using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services.Common.Auth.Model;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Based on: https://github.com/cornflourblue/aspnet-core-3-jwt-authentication-api
namespace Services.Common.Auth
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JwtMiddlewareOptions _options;

        private const string UserIdClaimType = "sub";
        private const string UserRoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";

        public JwtMiddleware(RequestDelegate next, JwtMiddlewareOptions options)
        {
            _next = next;
            _options = options;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                AttachUserToContext(context, token);

            await _next(context);
        }

        private void AttachUserToContext(HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_options.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                
                var userId = Guid.Parse(jwtToken.Claims.First(c => c.Type.Equals(UserIdClaimType)).Value);
                var roles = jwtToken.Claims.Where(c => c.Type.Equals(UserRoleClaimType)).Select(r => r.Value).ToArray();

                var user = new User { Id = userId };
                
                if (roles?.Any() == true)
                {
                    user.Roles = roles;   
                }

                context.Items["User"] = user;
            }
            catch(Exception e)
            {

            }
        }
    }
}