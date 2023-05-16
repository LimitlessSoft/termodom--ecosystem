using Infrastructure.Framework.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api
{
    public class APIResponseFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var content = context.Result as ObjectResult;

            if (content?.Value is IAPIResponse)
            {
                context.HttpContext.Response.StatusCode = (int)((IAPIResponse)content.Value).StatusCode;
            }
        }
    }
}
