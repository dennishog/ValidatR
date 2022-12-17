using Microsoft.AspNetCore.Http;

namespace ValidatR.Middleware;

public class ValidationMiddleware
{
    private readonly RequestDelegate _next;

    public ValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {


        await _next(httpContext);
    }
}
