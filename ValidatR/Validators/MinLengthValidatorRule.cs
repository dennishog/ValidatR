using ValidatR.Enums;
using ValidatR.Exceptions;

namespace ValidatR.Validators;
public class MinLengthValidatorRule<TParameter> : ValidatorRule<TParameter, int>
{
    public MinLengthValidatorRule(Func<string, TParameter, int> getValueFunc) : base(getValueFunc)
    {
    }

    public override ValidatorType ValidatorType => ValidatorType.MinLength;

    protected override async Task ValidateAsync<TModel, TValue>(ValidationContext<TModel, TValue> validationContext, int validationValue, CancellationToken cancellationToken)
    {
        var valueString = Convert.ToString(validationContext.Value);

        if (valueString != null && valueString.Length < validationValue)
        {

            throw new ValidationException(validationContext.ValidateAttribute, $"Value '{validationContext.Value}' should have a minimum length of '{validationValue}'");
        }

        await Task.CompletedTask;
    }
}
