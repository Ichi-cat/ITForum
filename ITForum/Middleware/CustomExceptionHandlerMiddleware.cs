using System.Net;
using System.Text.Json;
using ITForum.Application.Common.Exceptions;
using FluentValidation;
using ITForum.Application.Common.Exceptions.Generals;

namespace ITForum.Api.Middleware
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        public CustomExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context, ILogger<CustomExceptionHandlerMiddleware> logger)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception, logger);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception, ILogger<CustomExceptionHandlerMiddleware> logger)
        {
            var result = String.Empty;
            var code = HttpStatusCode.InternalServerError;
            
            switch (exception)
            {
                case UnauthorizeException unauthorizeException:
                    code = HttpStatusCode.Unauthorized;
                    result = JsonSerializer.Serialize<GeneralExceptionVm>(new GeneralExceptionVm((int)code, unauthorizeException.Message));
                    break;
                case ValidationException validationException:
                    code = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize<GeneralExceptionVm>(new GeneralExceptionVm((int)code, validationException));
                    break;
                case NotFoundException notFoundException:
                    code = HttpStatusCode.NotFound;
                    result = JsonSerializer.Serialize<GeneralExceptionVm>(new GeneralExceptionVm((int)code, notFoundException.Message));
                    break;
                case AuthenticationError authenticationError:
                    code = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize<GeneralExceptionVm>(new GeneralExceptionVm((int)code, authenticationError));
                    break;
                case UploadFileException uploadFileException:
                    code = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize<GeneralExceptionVm>(new GeneralExceptionVm((int)code, uploadFileException.Message));
                    break;
                case ModelValidationException modelValidationException:
                    code = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize<GeneralExceptionVm>(new GeneralExceptionVm((int)code, modelValidationException));
                    break;
                default:
                    result = JsonSerializer.Serialize<GeneralExceptionVm>(new GeneralExceptionVm((int)code, "Internal server error"));
                    logger.LogError(exception.Message);
                    break;
            }
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
