using ValidatR.Enums;

namespace ValidatR.Validators;
public abstract class ValidatorRule<TParameter> : IValidatorRule<TParameter>
{
    public ValidatorRule(Func<string, ValidatorType, TParameter, string> getValueFunc)
    {
        GetValueFunc = getValueFunc;
    }

    public Func<string, ValidatorType, TParameter, string> GetValueFunc { get; init; }
    public abstract ValidatorType ValidatorType { get; }

    public async Task HandleAsync<TModel, TValue>(ValidationContext<TModel, TValue> validationContext, TParameter parameter, CancellationToken cancellationToken)
    {
        if (!validationContext.ValidateAttribute.ValidatorType.HasFlag(ValidatorType))
        {
            return;
        }

        await ValidateAsync(validationContext, GetValueFunc(validationContext.ValidateAttribute.Id, ValidatorType, parameter), cancellationToken);
    }

    protected abstract Task ValidateAsync<TModel, TValue>(ValidationContext<TModel, TValue> validationContext, string validationValue, CancellationToken cancellationToken);
}
