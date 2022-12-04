using ITForum.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ITForum.Api.Additional
{
    public class ValidationFilterAttribute : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                throw new ModelValidationException(context.ModelState.Values.SelectMany(v => v.Errors));
            }
        }
        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
