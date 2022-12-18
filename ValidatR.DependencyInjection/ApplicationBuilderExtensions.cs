using Microsoft.AspNetCore.Builder;
using ValidatR.Enums;

namespace ValidatR.DependencyInjection;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseValidatR<TParameter>(this IApplicationBuilder self, Func<string, ValidatorType, TParameter, string> getRuleValidationValue)
    {
        var validator = self.ApplicationServices.GetRequiredService<IValidator<TParameter>>();

        validator.SetValidationRuleValueResolver(getRuleValidationValue);

        return self;
    }
}
