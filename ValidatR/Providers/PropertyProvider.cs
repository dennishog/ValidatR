using System.Collections;
using ValidatR.Attributes;
using ValidatR.Enums;
using ValidatR.Factories;

namespace ValidatR.Providers;

public class PropertyProvider : IPropertyProvider
{
    public async Task<List<IValidationContext>> GetValidationContextForAllPropertiesAsync<TModel>(TModel model, CancellationToken cancellationToken)
        where TModel : class
    {
        var validationContexts = new List<IValidationContext>();

        await TraversePropertiesRecursivelyAsync(model, validationContexts, cancellationToken);
        return validationContexts;
    }

    public async Task TraversePropertiesRecursivelyAsync<TModel>(TModel model, List<IValidationContext> validationContexts, CancellationToken cancellationToken)
    {
        foreach (var property in (from properties in typeof(TModel).GetProperties()
                                  where properties.CustomAttributes.Any(x => x.AttributeType.Equals(typeof(ValidateAttribute)))
                                  select properties))
        {
            var propertyValue = property.GetValue(model, null);
            var attribute = property.GetCustomAttribute<ValidateAttribute>();

            if (propertyValue == null || attribute == null)
            {
                continue;
            }

            validationContexts.Add(ValidationContextFactory.Create(property, attribute, propertyValue, model));

            if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
            {
                if (attribute.ValidatorType.HasFlag(ValidatorType.Required))
                {
                    if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                    {
                        if (propertyValue is IEnumerable collection)
                        {
                            foreach (var item in collection)
                            {
                                await ExecuteTraversePropertiesRecursivelyNonGeneric(item.GetType(), item, validationContexts, cancellationToken);
                            }
                        }
                    }
                    else
                    {
                        await ExecuteTraversePropertiesRecursivelyNonGeneric(propertyValue.GetType(), propertyValue, validationContexts, cancellationToken);
                    }
                }
            }
        }
    }

    private async Task ExecuteTraversePropertiesRecursivelyNonGeneric(Type modelType, object model, List<IValidationContext> validationContexts, CancellationToken cancellationToken)
    {
        var traverseMethod = typeof(PropertyProvider).GetMethod(nameof(PropertyProvider.TraversePropertiesRecursivelyAsync));
        var genericTraverseMethod = traverseMethod?.MakeGenericMethod(modelType);

        var task = (Task?)genericTraverseMethod?.Invoke(this, new object[] { model, validationContexts, cancellationToken }) ?? throw new InvalidOperationException();
        await task;
    }
}
