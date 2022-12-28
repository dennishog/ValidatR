using ValidatR.Enums;
using ValidatR.Exceptions;

namespace ValidatR.Validators;
public class RequiredValidatorRule<TParameter> : ValidatorRule<TParameter>
{
    public RequiredValidatorRule(Func<string, ValidatorType, TParameter, string> getValueFunc) : base(getValueFunc)
    {
    }

    public override ValidatorType ValidatorType => ValidatorType.Required;

    protected override async Task ValidateAsync<TModel, TValue>(ValidationContext<TModel, TValue> validationContext, string defaultValue, CancellationToken cancellationToken)
    {
        var valueString = Convert.ToString(validationContext.Value);

        var ruleValue = bool.Parse(defaultValue);

        if (string.IsNullOrWhiteSpace(valueString) && ruleValue)
        {
            throw new ValidationException(validationContext.ValidateAttribute, $"Value is required");
        }

        await Task.CompletedTask;
    }
}
