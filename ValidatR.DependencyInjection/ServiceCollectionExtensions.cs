
using ValidatR.DependencyInjection.Builder;

namespace ValidatR.DependencyInjection;
public static class ServiceCollectionExtensions
{
    public static IParameterBuilder<TParameter> AddValidatR<TParameter>(this IServiceCollection self)
    {
        var validator = new Validator<TParameter>();

        self.AddTransient<IValidator<TParameter>>(x => validator);
        self.AddTransient<IValidator>(x => validator);

        return new ParameterBuilder<TParameter>(validator);
    }
}