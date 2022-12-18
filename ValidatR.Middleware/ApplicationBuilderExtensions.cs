namespace ValidatR.Middleware;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseValidatorMiddleware<TModel>(this IApplicationBuilder self) where TModel : class
    {
        return self.UseMiddleware<ValidationMiddleware<TModel>>();
    }
}
