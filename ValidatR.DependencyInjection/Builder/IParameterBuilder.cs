namespace ValidatR.DependencyInjection.Builder;

public interface IParameterBuilder<TParameter>
{
    IParameterBuilder<TParameter> AddParameterResolver<TModel>(Expression<Func<TModel, TParameter>> parameterSelector);
}
