using ValidatR.Attributes;

namespace ValidatR.Factories;

internal static class ValidationContextFactory
{
    internal static IValidationContext Create<TModel>(PropertyInfo property, ValidateAttribute attribute, object propertyValue, TModel model)
    {
        var constructedValidationContextType = typeof(ValidationContext<,>).MakeGenericType(typeof(TModel), property.PropertyType);
        return (IValidationContext?)Activator.CreateInstance(constructedValidationContextType, new[] { attribute, propertyValue, model }) ?? throw new Exception("fkljs");
    }
}
