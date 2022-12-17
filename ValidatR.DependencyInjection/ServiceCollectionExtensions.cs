using Microsoft.Extensions.DependencyInjection;
using ValidatR.Enums;

namespace ValidatR.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddValidatR<TParameter>(this IServiceCollection self, Func<string, ValidatorType, TParameter, string> getValidationRuleValueFunc)
    {
        self.AddTransient<IValidator<TParameter>>(x => new Validator<TParameter>(getValidationRuleValueFunc));

        return self;
    }
}