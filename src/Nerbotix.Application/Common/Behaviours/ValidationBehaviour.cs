using FluentValidation;
using MediatR;

namespace Nerbotix.Application.Common.Behaviours;

public class ValidationBehaviour<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request, RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        if (validators.Any())
        {
            var ctx = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(
                validators.Select(v => v.ValidateAsync(ctx, cancellationToken)));
            
            var failures = validationResults
                .Where(v => v.Errors.Count != 0)
                .SelectMany(v => v.Errors)
                .ToList();

            if (failures.Count != 0)
            {
                throw new ValidationException(failures);
            }
        }
        
        return await next();
    }
}