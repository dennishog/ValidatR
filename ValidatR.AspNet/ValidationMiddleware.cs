namespace ValidatR.AspNet;

using ValidatR.Exceptions;

public class ValidationMiddleware<TModel> where TModel : class
{
    private readonly RequestDelegate _next;

    public ValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext, IValidator validator)
    {
        var controllerActionDescriptor = httpContext
        .GetEndpoint()?
        .Metadata
        .GetMetadata<ControllerActionDescriptor>();

        var controllerName = controllerActionDescriptor?.ControllerName;
        var actionName = controllerActionDescriptor?.ActionName;
        var parameters = controllerActionDescriptor?.Parameters;

        if (parameters != null)
        {
            foreach (var parameter in parameters)
            {
                if (!parameter.ParameterType.Equals(typeof(TModel)))
                {
                    continue;
                }

                httpContext.Request.EnableBuffering();
                using var streamReader = new StreamReader(httpContext.Request.Body, leaveOpen: true);
                var requestBody = await streamReader.ReadToEndAsync();
                httpContext.Request.Body.Position = 0;

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var model = JsonSerializer.Deserialize<TModel>(requestBody, options);

                if (model != null)
                {
                    try
                    {
                        await validator.ValidateAsync(model, CancellationToken.None);
                    }
                    catch (AggregateException aggregateException)
                    {
                        var validationExceptions = (IEnumerable<ValidationException>)aggregateException.InnerExceptions.ToList().Select(x => x as ValidationException);
                        var groupedValidationExceptions = validationExceptions.GroupBy(key => key.Attribute.Id);

                        httpContext.Response.Headers.Add("Content-Type", "application/json");
                        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                        var validationProblemDetails = new HttpValidationProblemDetails(groupedValidationExceptions.ToDictionary(key => key.Key, value => value.Select(x => x.ErrorMessage).ToArray()));
                        validationProblemDetails.Status = StatusCodes.Status400BadRequest;
                        await httpContext.Response.WriteAsJsonAsync(validationProblemDetails);
                        await httpContext.Response.CompleteAsync();
                    }
                }
            }
        }

        await _next(httpContext);
    }
}
