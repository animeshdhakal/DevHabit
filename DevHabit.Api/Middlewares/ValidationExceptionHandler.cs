using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DevHabit.Api.Middlewares;

public class ValidationExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is ValidationException validationException)
        {
            var context = new ProblemDetailsContext()
            {
                HttpContext = httpContext,
                Exception = exception,
                ProblemDetails = new ProblemDetails()
                {
                    Detail = "One or more validation errors occurred.",
                    Status = StatusCodes.Status400BadRequest,
                }
            };

            var errors = validationException.Errors.GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key.ToLowerInvariant(), g => g.Select(x => x.ErrorMessage));

            context.ProblemDetails.Extensions.TryAdd("errors", errors);

            return await problemDetailsService.TryWriteAsync(context);
        }

        return false;
    }
}