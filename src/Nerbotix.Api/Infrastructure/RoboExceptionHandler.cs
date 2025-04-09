using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace Nerbotix.Api.Infrastructure;

public class RoboExceptionHandler : IExceptionHandler
{
    private readonly Dictionary<Type, Func<HttpContext, Exception, Task>> _handlers = new()
    {
        { typeof(ValidationException), HandleValidationException },
        { typeof(UnauthorizedAccessException), HandleUnauthorizedException }
    };

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var type = exception.GetType();

        if (!_handlers.TryGetValue(type, out var handler))
        {
            return false;
        }
        
        await handler.Invoke(httpContext, exception);
        return true;
    }
    
    private static async Task HandleValidationException(HttpContext httpContext, Exception ex)
    {
        var exception = (ValidationException)ex;

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        await httpContext.Response.WriteAsJsonAsync(new
        {
            exception.Errors,
            Status = StatusCodes.Status400BadRequest,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        });
    }
    
    private static Task HandleUnauthorizedException(HttpContext httpContext, Exception ex)
    {
        httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
        
        return Task.CompletedTask;
    }
}