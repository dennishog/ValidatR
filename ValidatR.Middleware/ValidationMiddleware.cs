using Microsoft.AspNetCore.Http;
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
        var model = await ReadBodyFromRequestAndDeserialize(httpContext.Request);

        if (model != null)
        {
            await validator.ValidateAsync(model, CancellationToken.None);
        }

        await _next(httpContext);
    }

    private async Task<TModel?> ReadBodyFromRequestAndDeserialize(HttpRequest request)
    {
        request.EnableBuffering();
        using var streamReader = new StreamReader(request.Body, leaveOpen: true);
        var requestBody = await streamReader.ReadToEndAsync();
        request.Body.Position = 0;

        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var model = JsonSerializer.Deserialize<TModel>(requestBody, options);
            return model;
        }
        catch (Exception)
        {

        }

        return null;
    }
}
