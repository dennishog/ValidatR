using ValidatR.Resolvers;
using ValidatR.Validators;

namespace ValidatR;
public interface IValidator
{
    Task ValidateAsync<TModel>(TModel model, CancellationToken cancellationToken);
}

public interface IValidator<TParameter> : IValidator
{
    void AddValidator(IValidatorRule<TParameter> validator);
    //void SetValidationRuleValueResolver(Func<string, ValidatorType, TParameter, object> getRuleValidationValue);
    void AddParameterResolver(IParameterResolver<TParameter> parameterResolver);
    Task ValidateAsync<TModel>(TModel model, TParameter parameter, CancellationToken cancellationToken);
}
