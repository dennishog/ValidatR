using ValidatR.Enums;
using ValidatR.Exceptions;

namespace ValidatR.Validators;
public class MaxLengthValidatorRule<TParameter> : ValidatorRule<TParameter>
{
    public MaxLengthValidatorRule(Func<string, ValidatorType, TParameter, string> getValueFunc) : base(getValueFunc)
    {
    }

    public override ValidatorType ValidatorType => ValidatorType.MaxLength;

    protected override async Task ValidateAsync<TModel, TValue>(ValidationContext<TModel, TValue> validationContext, string defaultValue, CancellationToken cancellationToken)
    {
        var valueString = Convert.ToString(validationContext.Value);

        if (valueString != null && valueString.Length > int.Parse(defaultValue))
        {

            throw new ValidationException(validationContext.ValidateAttribute, $"Value '{validationContext.Value}' should have a maximum length of '{defaultValue}'");
        }

        await Task.CompletedTask;
    }
}
