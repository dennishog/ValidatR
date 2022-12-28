using ValidatR.Enums;
using ValidatR.Exceptions;

namespace ValidatR.Validators;
public class RegexValidatorRule<TParameter> : ValidatorRule<TParameter, string>
{
    public RegexValidatorRule(Func<string, TParameter, string> getValueFunc) : base(getValueFunc)
    {
    }

    public override ValidatorType ValidatorType => ValidatorType.Regex;

    protected override async Task ValidateAsync<TModel, TValue>(ValidationContext<TModel, TValue> validationContext, string validationValue, CancellationToken cancellationToken)
    {
        var regex = new System.Text.RegularExpressions.Regex(validationValue);

        var valueString = Convert.ToString(validationContext.Value);

        if (valueString != null && !regex.IsMatch(valueString))
        {
            throw new ValidationException(validationContext.ValidateAttribute, $"Value '{validationContext.Value}' is not allowed. Expected to follow pattern '{validationValue}'");
        }

        await Task.CompletedTask;
    }
}
