namespace ValidatR.Validators;

using Enums;
using Exceptions;

public class MaxLengthValidatorRule<TParameter> : ValidatorRule<TParameter>
{
    public MaxLengthValidatorRule(Func<string, ValidatorType, TParameter, string> getValueFunc) : base(getValueFunc)
    {
    }

    public override ValidatorType ValidatorType => ValidatorType.MaxLength;

    protected override Task ValidateAsync<TProperty>(TProperty value, string defaultValue, CancellationToken cancellationToken)
    {
        var valueString = Convert.ToString(value);

        if (valueString != null && valueString.Length > int.Parse(defaultValue))
        {

            throw new ValidationException<TProperty>(ValidatorType, value, defaultValue);
        }

        return Task.CompletedTask;
    }
}
