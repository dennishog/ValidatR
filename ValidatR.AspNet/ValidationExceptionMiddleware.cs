using Microsoft.Extensions.Logging;
using ValidatR.Exceptions;

namespace ValidatR.AspNet;

public class ValidationExceptionMiddleware
{
    private readonly ILogger<ValidationExceptionMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ValidationExceptionMiddleware(RequestDelegate next, ILogger<ValidationExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        if (exception is AggregateException aggregateException && aggregateException.InnerExceptions.Any(ex => ex is ValidationException))
        {
            _logger.LogError(exception, "Validation error occurred");

            var validationExceptions = (IEnumerable<ValidationException>)aggregateException.InnerExceptions.ToList().Select(x => x as ValidationException);
            var groupedValidationExceptions = validationExceptions.GroupBy(key => key.Attribute.Id);

            httpContext.Response.Headers.Add("Content-Type", "application/json");
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            var validationProblemDetails = new HttpValidationProblemDetails(groupedValidationExceptions.ToDictionary(key => key.Key, value => value.Select(x => x.ErrorMessage).ToArray()))
            {
                Status = StatusCodes.Status400BadRequest
            };
            await httpContext.Response.WriteAsJsonAsync(validationProblemDetails);
            await httpContext.Response.CompleteAsync();
        }
        else
        {
            throw exception;
        }
    }
}
