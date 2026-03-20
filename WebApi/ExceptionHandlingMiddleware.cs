using Application.Exceptions;

using Common.Wrappers;

using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebApi;

public class ExceptionHandlingMiddleware(RequestDelegate next, IWebHostEnvironment webHostEnvironment, 
    ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, GetOptions());
        }
    }

    private static JsonSerializerOptions GetOptions()
    {
        return new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex, JsonSerializerOptions options)
    {
        var response = context.Response;

        if (response.HasStarted)
        {
            logger.LogWarning(ex, "Response has already started; skip modifying headers/body.");
            return; // or context.Abort() if appropriate
        }

        response.ContentType = "application/json";
        var responseWrapper = ResponseWrapper.Fail();

        // Only include stack trace in development environment
        if (webHostEnvironment.IsDevelopment())
        {
            responseWrapper.StackTrace = $"{ex.Message}\n{ex.StackTrace}";

            // For development, include inner exception details too
            if (ex.InnerException != null)
            {
                responseWrapper.StackTrace += $"\n\nInner Exception: {ex.InnerException.Message}\n{ex.InnerException.StackTrace}";
            }
        }

        // Log the error regardless of environment
        LogException(ex);

        switch (ex)
        {
            case ConflictException ce:
                response.StatusCode = (int)ce.StatusCode;
                responseWrapper.Messages = ce.ErrorMessages;
                logger.LogWarning(ex, "Conflict exception occurred: {Message}", ce.Message);
                break;

            case NotFoundException nfe:
                response.StatusCode = (int)nfe.StatusCode;
                responseWrapper.Messages = nfe.ErrorMessages;
                logger.LogWarning(ex, "Not found exception occurred: {Message}", nfe.Message);
                break;

            case ForbiddenException fe:
                response.StatusCode = (int)fe.StatusCode;
                responseWrapper.Messages = fe.ErrorMessages;
                logger.LogWarning(ex, "Forbidden exception occurred: {Message}", fe.Message);
                break;

            case IdentityException ie:
                response.StatusCode = (int)ie.StatusCode;
                responseWrapper.Messages = ie.ErrorMessages;
                logger.LogWarning(ex, "Identity exception occurred: {Message}", ie.Message);
                break;

            case UnauthorizedException ue:
                response.StatusCode = (int)ue.StatusCode;
                responseWrapper.Messages = ue.ErrorMessages;
                logger.LogWarning(ex, "Unauthorized exception occurred: {Message}", ue.Message);
                break;

            case ValidationException ve:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                responseWrapper.Messages = ve.ErrorMessages;
                logger.LogWarning(ex, "Validation exception occurred: {Message}", ve.Message);
                break;

            case BadRequestException bre:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                responseWrapper.Messages = bre.ErrorMessages;
                logger.LogWarning(ex, "Bad request exception occurred: {Message}", bre.Message);
                break;

            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                responseWrapper.Messages = ["Something went wrong. Please contact the administrator."];
                logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
                break;
        }

        var result = JsonSerializer.Serialize(responseWrapper, options);

        await response.WriteAsync(result);
    }

    private void LogException(Exception ex)
    {
        var logMessage = $@"Exception occurred:
            Type: {ex.GetType().Name}
            Message: {ex.Message}
            Stack Trace: {ex.StackTrace}";

        if (ex.InnerException != null)
        {
            logMessage += $@"
            Inner Exception:
            Type: {ex.InnerException.GetType().Name}
            Message: {ex.InnerException.Message}
            Stack Trace: {ex.InnerException.StackTrace}";
        }

        // Use appropriate log level based on exception type
        if (ex is ApplicationException || ex is DomainException)
        {
            logger.LogWarning(ex, logMessage);
        }
        else
        {
            logger.LogError(ex, logMessage);
        }
    }
}
