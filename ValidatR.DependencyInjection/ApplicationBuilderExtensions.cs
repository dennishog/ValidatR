using Microsoft.AspNetCore.Builder;
using ValidatR.DependencyInjection.Builder;
using ValidatR.Resolvers;

namespace ValidatR.DependencyInjection;

public static class ApplicationBuilderExtensions
{
    public static IValidatorBuilder<TParameter> UseValidatR<TParameter>(this IApplicationBuilder self)
    {
        var validatorRuleValueResolver = self.ApplicationServices.GetRequiredService<IValidatorRuleValueResolver<TParameter>>();

        return new ValidatorBuilder<TParameter>(self, validatorRuleValueResolver);
    }
}
