using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Services.Access.Domain.Interfaces.Services;
using Services.Common.Auth.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using static Services.Access.Domain.Model.Enums;

namespace Services.Access.Api.Attributes
{
    //https://stackoverflow.com/questions/1601333/asp-net-mvc-actionfilterattribute-to-validate-post-data/1601572#1601572
    //Check the object  id in the request and the filter to see if the user has permission to access the specified object

    //Shouldn't do this as an action filter, as in the implementation of filter, we'll need the action parameters
    //And this makes it tightly coupled with the action itself and hard to debug & maintain
    
    public class RequiresPermissionAttribute : ActionFilterAttribute
    {
        public PermissionSubjectType Subject { get; set; }
        
        private readonly ILockService _lockService;
        private readonly ISiteService _siteService;
        public RequiresPermissionAttribute(ILockService lockService, ISiteService siteService)
        {
            _lockService = lockService;
            _siteService = siteService;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var requestContext = context.HttpContext;
            var user = requestContext.Items["User"] as User;
            var userId = user.Id;

            //var objectId = context.ActionArguments.TryGetValue("id");
            
            base.OnActionExecuting(context);

        }
    }
}
