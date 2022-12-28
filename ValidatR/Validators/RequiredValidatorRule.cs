using ValidatR.Enums;
using ValidatR.Exceptions;

namespace ValidatR.Validators;
public class RequiredValidatorRule<TParameter> : ValidatorRule<TParameter, bool>
{
    public RequiredValidatorRule(Func<string, TParameter, bool> getValueFunc) : base(getValueFunc)
    {
    }

    public override ValidatorType ValidatorType => ValidatorType.Required;

    protected override async Task ValidateAsync<TModel, TValue>(ValidationContext<TModel, TValue> validationContext, bool validationValue, CancellationToken cancellationToken)
    {
        var valueString = Convert.ToString(validationContext.Value);

        if (string.IsNullOrWhiteSpace(valueString) && validationValue)
        {
            throw new ValidationException(validationContext.ValidateAttribute, $"Value is required");
        }

        await Task.CompletedTask;
    }
}
