using Microsoft.Extensions.DependencyInjection;
using ValidatR.DependencyInjection.Builder;
using ValidatR.Enums;

namespace ValidatR.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IParameterBuilder<TParameter> AddValidatR<TParameter>(this IServiceCollection self, Func<string, ValidatorType, TParameter, string> getValidationRuleValueFunc)
    {
        var validator = new Validator<TParameter>(getValidationRuleValueFunc);

        self.AddTransient<IValidator<TParameter>>(x => validator);
        self.AddTransient<IValidator>(x => validator);

        return new ParameterBuilder<TParameter>(validator);
    }
}