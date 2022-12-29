using ValidatR.Enums;
using ValidatR.Exceptions;
using ValidatR.Resolvers;

namespace ValidatR.Validators;
internal class RequiredValidatorRule<TParameter> : ValidatorRule<TParameter, bool>
{
    public RequiredValidatorRule(IValidatorRuleValueResolver<TParameter> ruleValueResolver) : base(ruleValueResolver)
    {
    }

    public override ValidatorType ValidatorType => ValidatorType.Required;

    protected override async Task ValidateAsync<TModel, TValue>(ValidationContext<TModel, TValue> validationContext, bool validationValue, CancellationToken cancellationToken)
    {
        var valueString = Convert.ToString(validationContext.Value);

        if (string.IsNullOrWhiteSpace(valueString) && validationValue)
        {
            throw new ValidationException(validationContext.ValidateAttribute, $"Value is required");
        }

        await Task.CompletedTask;
    }
}
