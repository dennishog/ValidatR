namespace ValidatR.Exceptions;

using Enums;

public class ValidationException<TProperty> : Exception
{
    public ValidationException(PropertyInfo propertyInfo, ValidatorType validatorType, TProperty value, string validationRuleValue) : base(GetMessage(propertyInfo, validatorType, value, validationRuleValue))
    {
    }

    private static string GetMessage(PropertyInfo propertyInfo, ValidatorType validatorType, TProperty value, string validationRuleValue)
    {
        var propertyName = propertyInfo.Name;
        var modelName = propertyInfo.DeclaringType?.Name;
        var prefix = $"{modelName}.{propertyName}: ";

        return validatorType switch
        {
            ValidatorType.Regex => $"{prefix}Value '{value}' is not allowed",
            ValidatorType.MaxLength => $"{prefix}Value '{value}' exceeds max length allowed ({validationRuleValue})",
            ValidatorType.MinLength => $"{prefix}Value '{value}' length under minimum allowed length ({validationRuleValue})",
            ValidatorType.Required => $"{prefix}Value is required",
            _ => throw new InvalidOperationException()
        };
    }
}
