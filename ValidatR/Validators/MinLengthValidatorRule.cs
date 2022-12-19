namespace ValidatR.Validators;

using Enums;
using ValidatR.Attributes;
using ValidatR.Exceptions;

public class MinLengthValidatorRule<TParameter> : ValidatorRule<TParameter>
{
    public MinLengthValidatorRule(Func<string, ValidatorType, TParameter, string> getValueFunc) : base(getValueFunc)
    {
    }

    public override ValidatorType ValidatorType => ValidatorType.MinLength;

    protected override async Task ValidateAsync<TProperty>(ValidateAttribute attribute, TProperty value, string ruleValue, CancellationToken cancellationToken)
    {
        var valueString = Convert.ToString(value);

        if (valueString != null && valueString.Length < int.Parse(ruleValue))
        {

            throw new ValidationException(attribute, $"Value '{value}' should have a minimum length of '{ruleValue}'");
        }

        await Task.CompletedTask;
    }
}
