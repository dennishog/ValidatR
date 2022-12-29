using ValidatR.Enums;
using ValidatR.Exceptions;
using ValidatR.Resolvers;

namespace ValidatR.Validators;
internal class RegexValidatorRule<TParameter> : ValidatorRule<TParameter, string>
{
    public RegexValidatorRule(IValidatorRuleValueResolver<TParameter> ruleValueResolver) : base(ruleValueResolver)
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
