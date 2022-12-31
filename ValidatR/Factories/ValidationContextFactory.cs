using ValidatR.Attributes;

namespace ValidatR.Factories;

internal static class ValidationContextFactory
{
    internal static IValidationContext Create<TModel>(PropertyInfo property, ValidateAttribute attribute, object propertyValue, TModel model)
    {
        model = model ?? throw new ArgumentNullException(nameof(model));

        Type constructedValidationContextType = typeof(ValidationContext<,>).MakeGenericType(typeof(TModel), property.PropertyType);

        var instance = Activator.CreateInstance(constructedValidationContextType, new[] { attribute, propertyValue, model });

        return (IValidationContext?)instance ?? throw new ArgumentNullException(nameof(instance));
    }
}
