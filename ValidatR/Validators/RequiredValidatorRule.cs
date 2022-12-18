namespace ValidatR.Validators;

using Enums;
using Exceptions;

public class RequiredValidatorRule<TParameter> : ValidatorRule<TParameter>
{
    public RequiredValidatorRule(Func<string, ValidatorType, TParameter, string> getValueFunc) : base(getValueFunc)
    {
    }

    public override ValidatorType ValidatorType => ValidatorType.Required;

    protected override Task ValidateAsync<TProperty>(PropertyInfo propertyInfo, TProperty value, string defaultValue, CancellationToken cancellationToken)
    {
        var valueString = Convert.ToString(value);

        var ruleValue = bool.Parse(defaultValue);

        if (string.IsNullOrWhiteSpace(valueString) && ruleValue)
        {
            throw new ValidationException<TProperty>(propertyInfo, ValidatorType, value, defaultValue);
        }

        return Task.CompletedTask;
    }
}
