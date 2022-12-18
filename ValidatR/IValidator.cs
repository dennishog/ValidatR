namespace ValidatR;

using Resolvers;
using ValidatR.Enums;

public interface IValidator
{
    Task ValidateAsync<TModel>(TModel model, CancellationToken cancellationToken);
}

public interface IValidator<TParameter> : IValidator
{
    void SetValidationRuleValueResolver(Func<string, ValidatorType, TParameter, string> getRuleValidationValue);
    void AddParameterResolver(IParameterResolver<TParameter> parameterResolver);
    Task ValidateAsync<TModel>(TModel model, TParameter parameter, CancellationToken cancellationToken);
}
