namespace ValidatR.Validators;

using Enums;
using Exceptions;

public class MinLengthValidatorRule<TParameter> : ValidatorRule<TParameter>
{
    public MinLengthValidatorRule(Func<string, ValidatorType, TParameter, string> getValueFunc) : base(getValueFunc)
    {
    }

    public override ValidatorType ValidatorType => ValidatorType.MinLength;

    protected override Task ValidateAsync<TProperty>(PropertyInfo propertyInfo, TProperty value, string ruleValue, CancellationToken cancellationToken)
    {
        var valueString = Convert.ToString(value);

        if (valueString != null && valueString.Length < int.Parse(ruleValue))
        {

            throw new ValidationException<TProperty>(propertyInfo, ValidatorType, value, ruleValue);
        }

        return Task.CompletedTask;
    }
}
