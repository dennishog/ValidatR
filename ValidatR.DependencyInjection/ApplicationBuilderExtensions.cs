using Microsoft.AspNetCore.Builder;
using ValidatR.DependencyInjection.Builder;

namespace ValidatR.DependencyInjection;

public static class ApplicationBuilderExtensions
{
    public static IValidatorBuilder<TParameter> UseValidatR<TParameter>(this IApplicationBuilder self)
    {
        var validator = self.ApplicationServices.GetRequiredService<IValidator<TParameter>>();

        return new ValidatorBuilder<TParameter>(validator);
    }
}
