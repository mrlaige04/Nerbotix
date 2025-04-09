using ErrorOr;
using Microsoft.AspNetCore.Identity;

namespace Nerbotix.Application.Common.Extensions;

public static class IdentityResultExtensions
{
    public static ErrorOr<Success> ToErrorOr(this IdentityResult error, string? errorCode, string? errorDescription)
    {
        var errors = error.Errors
            .ToDictionary(e => e.Code, object (e) => e.Description);

        var code = string.IsNullOrEmpty(errorCode)
            ? "Something went wrong"
            : errorCode;
        
        var description = string.IsNullOrEmpty(errorDescription)
            ? "Something went wrong"
            : errorDescription;
        
        return Error.Failure(code, description, errors);
    }
    
    public static ErrorOr<T> ToErrorOr<T>(this IdentityResult error, string? errorCode, string? errorDescription)
    {
        var errors = error.Errors
            .ToDictionary(e => e.Code, object (e) => e.Description);

        var code = string.IsNullOrEmpty(errorCode)
            ? "Something went wrong"
            : errorCode;
        
        var description = string.IsNullOrEmpty(errorDescription)
            ? "Something went wrong"
            : errorDescription;
        
        return Error.Failure(code, description, errors);
    }
}