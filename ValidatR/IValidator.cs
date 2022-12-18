using ValidatR.Resolvers;

namespace ValidatR;

public interface IValidator
{
    Task ValidateAsync<TModel>(TModel model, CancellationToken cancellationToken);
}

public interface IValidator<TParameter> : IValidator
{
    void AddParameterResolver(IParameterResolver<TParameter> parameterResolver);
    Task ValidateAsync<TModel>(TModel model, TParameter parameter, CancellationToken cancellationToken);
}
