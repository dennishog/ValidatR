using ValidatR.Enums;
using ValidatR.Exceptions;

namespace ValidatR.Validators;

public class RegexValidatorRule<TParameter> : ValidatorRule<TParameter>
{
    public RegexValidatorRule(Func<string, ValidatorType, TParameter, string> getValueFunc) : base(getValueFunc)
    {
    }

    public override ValidatorType ValidatorType => ValidatorType.Regex;

    protected override Task ValidateAsync<TProperty>(TProperty value, string pattern, CancellationToken cancellationToken)
    {
        var regex = new System.Text.RegularExpressions.Regex(pattern);

        var valueString = Convert.ToString(value);

        if (valueString != null && !regex.IsMatch(valueString))
        {
            throw new ValidationException<TProperty>(ValidatorType, value, pattern);
        }

        return Task.CompletedTask;
    }
}
