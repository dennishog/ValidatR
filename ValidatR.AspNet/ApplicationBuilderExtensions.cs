namespace ValidatR.AspNet;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseValidatorMiddleware<TModel>(this IApplicationBuilder self) where TModel : class
    {
        return self
            .UseMiddleware<ValidationExceptionMiddleware>()
            .UseMiddleware<ValidationMiddleware<TModel>>();
    }
}
