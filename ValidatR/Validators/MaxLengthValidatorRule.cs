using ValidatR.Enums;
using ValidatR.Exceptions;

namespace ValidatR.Validators;
public class MaxLengthValidatorRule<TParameter> : ValidatorRule<TParameter, int>
{
    public MaxLengthValidatorRule(Func<string, TParameter, int> getValueFunc) : base(getValueFunc)
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
