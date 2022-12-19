namespace ValidatR.Validators;

using Enums;
using Exceptions;
using ValidatR.Attributes;

public class RegexValidatorRule<TParameter> : ValidatorRule<TParameter>
{
    public RegexValidatorRule(Func<string, ValidatorType, TParameter, string> getValueFunc) : base(getValueFunc)
    {
    }

    public override ValidatorType ValidatorType => ValidatorType.Regex;

    protected override async Task ValidateAsync<TProperty>(ValidateAttribute attribute, TProperty value, string pattern, CancellationToken cancellationToken)
    {
        var regex = new System.Text.RegularExpressions.Regex(pattern);

        var valueString = Convert.ToString(value);

        if (valueString != null && !regex.IsMatch(valueString))
        {
            throw new ValidationException(attribute, $"Value '{value}' is not allowed. Expected to follow pattern '{pattern}'");
        }

        await Task.CompletedTask;
    }
}
