using ValidatR.Enums;

namespace ValidatR.Exceptions;

public class ValidationException<TProperty> : Exception
{
    public ValidationException(ValidatorType validatorType, TProperty value, string validationRuleValue) : base(GetMessage(validatorType, value, validationRuleValue))
    {
    }

    private static string GetMessage(ValidatorType validatorType, TProperty value, string validationRuleValue)
    {
        return validatorType switch
        {
            ValidatorType.Regex => $"Value '{value}' is not allowed",
            ValidatorType.MaxLength => $"Value '{value}' exceeds max length allowed ({validationRuleValue})",
            _ => throw new InvalidOperationException()
        };
    }
}
