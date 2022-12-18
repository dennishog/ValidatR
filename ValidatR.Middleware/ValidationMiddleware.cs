using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Text.Json;

namespace ValidatR.Middleware;

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
                    await validator.ValidateAsync(model, CancellationToken.None);
                }
            }
        }

        await _next(httpContext);
    }
}
