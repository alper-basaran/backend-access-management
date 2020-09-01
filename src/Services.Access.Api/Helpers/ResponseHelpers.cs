using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using Services.Access.Model;
using Services.Access.Api.Model;
using Services.Access.Domain.Exceptions;
using Services.Access.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Services.Access.Api.Helpers
{
    public static class ResponseHelpers
    {        
        public static IActionResult NotFoundResponse(string type, Guid id)
        {
            var message = new MessageResponse($"{type} with id '{id}' could not be found");
            return new NotFoundObjectResult(message);
        }
        public static IActionResult NotFoundResponse(string type)
        {
            var message = new MessageResponse($"{type} could not be found");
            return new NotFoundObjectResult(message);
        }

        public static IActionResult BadRequestResult(string field)
        {
            var message = new MessageResponse($"Field {field} value is invalid");
            return new BadRequestObjectResult(message);
        }
        public static IActionResult InternalServerErrorResponse()
        {
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
        public static IActionResult UnauthorizedResponse()
        {
            var message = new MessageResponse($"User does not have permission to perform this request");
            return new UnauthorizedObjectResult(message);
        }
    }
}
