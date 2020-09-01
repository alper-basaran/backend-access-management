using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using Services.Access.Model;
using Services.Access.Api.Model;
using Services.Access.Domain.Model;
using Services.Common.Auth.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Access.Api.Helpers
{
    public static class RequestHelpers
    {
        public static User GetCurrentUser(this HttpRequest request)
        {
            var user = request.HttpContext.Items["User"] as User;
            return user;
        }
    }
}
