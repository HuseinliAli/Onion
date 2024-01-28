using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CompanyEmloyees.Presentation.ActionFilters
{
    public class ValidationFilterAttribute : IActionFilter
    {
        public ValidationFilterAttribute()
        {
            
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
          
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var action = context.RouteData.Values["action"];
            var controller = context.RouteData.Values["controller"];
            var param = context.ActionArguments
                .SingleOrDefault(x => x.Value.ToString().Contains("Dto")).Value;
            if(param is null)
            {
                context.Result = new BadRequestObjectResult($"Object is null. Controller: {controller}, action: {action}");
                return;
            }
            if (!context.ModelState.IsValid)
                context.Result = new UnprocessableEntityObjectResult(context.ModelState);
        }
    }

    public class ValidateMediaTypeAttribute : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
           
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var acceptHeaderPresent = context.HttpContext.Request.Headers.ContainsKey("Accept");
            
            if (!acceptHeaderPresent) {
                context.Result = new BadRequestObjectResult($"Accept header is missing.");
                return;
            }

            var mediaType = context.HttpContext.Request.Headers["Accept"].FirstOrDefault();

            if(!MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue? outMediaType))
            {
                context.Result = new BadRequestObjectResult("Media type not present. Please add Accept header with the required media type.");
                return;
            }
            context.HttpContext.Items.Add("AcceptHeaderMediaType", outMediaType);

        }
    }
}
