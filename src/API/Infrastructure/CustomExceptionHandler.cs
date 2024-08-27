using Infrastructure.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

namespace API.Infrastructure;

public class CustomExceptionHandler : IExceptionHandler
{
    private readonly Dictionary<Type, Func<HttpContext, Exception, Task>> _exceptionHandlers;

    public CustomExceptionHandler()
    {
        _exceptionHandlers = new()
        {
            { typeof(ValidationException), HandleValidationException },
            { typeof(MissingResourceException), HandleMissingResourceException },
            { typeof(FailedFetchException), HandleFailedFetchException },
        };
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var exceptionType = exception.GetType();

        if (_exceptionHandlers.TryGetValue(exceptionType, out Func<HttpContext, Exception, Task>? value))
        {
            await value.Invoke(httpContext, exception);

            return true;
        }

        return false;
    }

    private async Task HandleValidationException(HttpContext context, Exception ex)
    {
        var exception = (ValidationException)ex;

        context.Response.StatusCode = StatusCodes.Status400BadRequest;

        var validationErrors = exception.Errors
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());

        await context.Response.WriteAsJsonAsync(new ValidationProblemDetails(validationErrors)
        {
            Status = StatusCodes.Status400BadRequest,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        });
    }

    private async Task HandleMissingResourceException(HttpContext context, Exception ex)
    {
        var exception = (MissingResourceException)ex;

        context.Response.StatusCode = StatusCodes.Status404NotFound;

        await context.Response.WriteAsJsonAsync(new ProblemDetails()
        {
            Status = StatusCodes.Status404NotFound,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            Title = "Resource not found",
            Detail = exception.Message
        });
    }

    private async Task HandleFailedFetchException(HttpContext context, Exception ex)
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        await context.Response.WriteAsJsonAsync(new ProblemDetails()
        {
            Status = StatusCodes.Status500InternalServerError,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            Title = "Unexpected server error",
            Detail = "An unexpected error occured on the server"
        });
    }
}
