using ValidatR.Enums;
using ValidatR.Exceptions;
using ValidatR.Resolvers;

namespace ValidatR.Validators;
internal class MaxLengthValidatorRule<TParameter> : ValidatorRule<TParameter, int>
{
    public MaxLengthValidatorRule(IValidatorRuleValueResolver<TParameter> ruleValueResolver) : base(ruleValueResolver)
    {
    }

    public override ValidatorType ValidatorType => ValidatorType.MaxLength;

    protected override async Task ValidateAsync<TModel, TValue>(ValidationContext<TModel, TValue> validationContext, int validationValue, CancellationToken cancellationToken)
    {
        var valueString = Convert.ToString(validationContext.Value);

        if (valueString != null && valueString.Length > validationValue)
        {
            throw new ValidationException(validationContext.ValidateAttribute, $"Value '{validationContext.Value}' should have a maximum length of '{validationValue}'");
        }

        await Task.CompletedTask;
    }
}
