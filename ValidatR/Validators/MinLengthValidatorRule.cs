using ValidatR.Enums;
using ValidatR.Exceptions;

namespace ValidatR.Validators;
public class MinLengthValidatorRule<TParameter> : ValidatorRule<TParameter>
{
    public MinLengthValidatorRule(Func<string, ValidatorType, TParameter, string> getValueFunc) : base(getValueFunc)
    {
    }

    public override ValidatorType ValidatorType => ValidatorType.MinLength;

    protected override async Task ValidateAsync<TModel, TValue>(ValidationContext<TModel, TValue> validationContext, string ruleValue, CancellationToken cancellationToken)
    {
        var valueString = Convert.ToString(validationContext.Value);

        if (valueString != null && valueString.Length < int.Parse(ruleValue))
        {

            throw new ValidationException(validationContext.ValidateAttribute, $"Value '{validationContext.Value}' should have a minimum length of '{ruleValue}'");
        }

        await Task.CompletedTask;
    }
}
