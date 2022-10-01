using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using ITForum.Application.Common.Exceptions;



namespace ITForum.Api.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var result = String.Empty;
            var code = HttpStatusCode.InternalServerError;
            
            switch (exception)
            {
                case ValidationException validationException:
                    code = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(validationException.Failures);
                    break;
                case NotFoundException notFoundException:
                    code = HttpStatusCode.NotFound;
                    result = JsonSerializer.Serialize(new { error = notFoundException.Message });
                    break;
                case ForbiddenException forbiddenException:
                    code = HttpStatusCode.Forbidden;
                    //result = JsonSerializer.Serialize();
                    break;
                case UnauthorizedException unauthorizedException:
                    code = HttpStatusCode.Unauthorized;
                    //result = JsonSerializer.Serialize();
                    break;
                default:
                    result = JsonSerializer.Serialize(new { error = exception.Message });
                    break;
            }
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(result);
        }
    }
}
