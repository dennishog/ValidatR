using ValidatR.DependencyInjection.Builder;
using ValidatR.Providers;
using ValidatR.Resolvers;
using ValidatR.Validators;

namespace ValidatR.DependencyInjection;
public static class ServiceCollectionExtensions
{
    public static IParameterBuilder<TParameter> AddValidatR<TParameter>(this IServiceCollection self)
    {
        self.AddSingleton<IValidatorRuleValueResolver<TParameter>, ValidatorRuleValueResolver<TParameter>>();
        self.AddTransient<IPropertyProvider, PropertyProvider>();
        self.AddTransient<IValidator<TParameter>, Validator<TParameter>>();
        self.AddTransient<IValidator, Validator<TParameter>>();

        self.AddTransient<IValidatorRule<TParameter>, MinLengthValidatorRule<TParameter>>();
        self.AddTransient<IValidatorRule<TParameter>, MaxLengthValidatorRule<TParameter>>();
        self.AddTransient<IValidatorRule<TParameter>, RegexValidatorRule<TParameter>>();
        self.AddTransient<IValidatorRule<TParameter>, RequiredValidatorRule<TParameter>>();

        return new ParameterBuilder<TParameter>(self);
    }
}