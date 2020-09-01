using Microsoft.AspNetCore.Mvc;
using Services.Access.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Access.Api.Helpers
{
    public static class ExceptionHelpers
    {
        public static IActionResult Handle(this Exception e)
        {
            if (e is InvalidIdentifierException)
            {
                return ResponseHelpers.BadRequestResult("id");
            }
            else if (e is LockNotFoundException)
            {
                return ResponseHelpers.NotFoundResponse("Lock");
            }
            else if (e is InsufficientPermissionException)
            {
                return ResponseHelpers.UnauthorizedResponse();
            }
            else if (e is SiteNotFoundException)
            {
                return ResponseHelpers.NotFoundResponse("Site");
            }
            else
            {
                return ResponseHelpers.InternalServerErrorResponse();
            }
        }
    }
}
