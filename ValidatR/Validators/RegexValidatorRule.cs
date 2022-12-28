using ValidatR.Enums;
using ValidatR.Exceptions;

namespace ValidatR.Validators;
public class RegexValidatorRule<TParameter> : ValidatorRule<TParameter>
{
    public RegexValidatorRule(Func<string, ValidatorType, TParameter, string> getValueFunc) : base(getValueFunc)
    {
    }

    public override ValidatorType ValidatorType => ValidatorType.Regex;

    protected override async Task ValidateAsync<TModel, TValue>(ValidationContext<TModel, TValue> validationContext, string pattern, CancellationToken cancellationToken)
    {
        var regex = new System.Text.RegularExpressions.Regex(pattern);

        var valueString = Convert.ToString(validationContext.Value);

        if (valueString != null && !regex.IsMatch(valueString))
        {
            throw new ValidationException(validationContext.ValidateAttribute, $"Value '{validationContext.Value}' is not allowed. Expected to follow pattern '{pattern}'");
        }

        await Task.CompletedTask;
    }
}
