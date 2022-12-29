using ValidatR.Resolvers;

namespace ValidatR.DependencyInjection.Builder;
internal class ParameterBuilder<TParameter> : IParameterBuilder<TParameter>
{
    private readonly IServiceCollection _services;

    public ParameterBuilder(IServiceCollection services)
    {
        _services = services;
    }

    public IParameterBuilder<TParameter> AddParameterResolver<TModel>(Expression<Func<TModel, TParameter>> parameterSelector)
    {
        _services.AddTransient<IParameterResolver<TParameter>>(x => new ParameterResolver<TModel, TParameter>(parameterSelector));

        return this;
    }
}
