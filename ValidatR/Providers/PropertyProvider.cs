using System.Collections;
using ValidatR.Attributes;
using ValidatR.Enums;
using ValidatR.Factories;

namespace ValidatR.Providers;

public class PropertyProvider
{
    public async Task<List<IValidationContext>> GetValidationContextForAllPropertiesAsync<TModel>(TModel model, CancellationToken cancellationToken)
        where TModel : class
    {
        var validationContexts = new List<IValidationContext>();
        await TraversePropertiesRecursivelyAsync(typeof(TModel).GetProperties(), model, validationContexts, cancellationToken);
        return validationContexts;
    }

    public async Task TraversePropertiesRecursivelyAsync<TModel>(PropertyInfo[] properties, TModel model, List<IValidationContext> validationContexts, CancellationToken cancellationToken)
    {
        foreach (var property in properties)
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
                                var handleMethod = typeof(PropertyProvider).GetMethod(nameof(PropertyProvider.TraversePropertiesRecursivelyAsync));
                                var genericHandleMethod = handleMethod?.MakeGenericMethod(item.GetType());

                                var task = (Task?)genericHandleMethod?.Invoke(this, new object[] { item.GetType().GetProperties(), item, validationContexts, cancellationToken }) ?? throw new InvalidOperationException();
                                await task;
                            }
                        }
                    }
                    else
                    {
                        var handleMethod = typeof(PropertyProvider).GetMethod(nameof(PropertyProvider.TraversePropertiesRecursivelyAsync));
                        var genericHandleMethod = handleMethod?.MakeGenericMethod(propertyValue.GetType());

                        var task = (Task?)genericHandleMethod?.Invoke(this, new object[] { propertyValue.GetType().GetProperties(), propertyValue, validationContexts, cancellationToken }) ?? throw new InvalidOperationException();
                        await task;
                    }
                }
            }
        }
    }
}
