
using ValidatR.Attributes;
using ValidatR.Enums;

namespace ValidatR.Validators;
public abstract class ValidatorRule<TParameter> : IValidatorRule<TParameter>
{
    public ValidatorRule(Func<string, ValidatorType, TParameter, string> getValueFunc)
    {
        GetValueFunc = getValueFunc;
    }

    public Func<string, ValidatorType, TParameter, string> GetValueFunc { get; init; }
    public abstract ValidatorType ValidatorType { get; }

    public async Task HandleAsync<TModel>(PropertyInfo propertyInfo, TModel model, TParameter parameter, CancellationToken cancellationToken)
    {
        var value = propertyInfo.GetValue(model);

        var attribute = GetValidateAttribute<TModel>(propertyInfo);

        if (attribute == null || !attribute.ValidatorType.HasFlag(ValidatorType))
        {
            return;
        }

        await ValidateAsync(attribute, value, GetValueFunc(attribute.Id, ValidatorType, parameter), cancellationToken);
    }

    protected abstract Task ValidateAsync<TProperty>(ValidateAttribute attribute, TProperty value, string validationValue, CancellationToken cancellationToken);

    private ValidateAttribute? GetValidateAttribute<TModel>(PropertyInfo propertyInfo)
    {
        var attribute = propertyInfo.GetCustomAttribute<ValidateAttribute>();

        // If attribute is not found, check constructor parameters if there is a ValidateAttribute declared
        // This is mainly to support records
        foreach (var constructor in typeof(TModel).GetConstructors())
        {
            if (attribute != null)
            {
                continue;
            }

            var parameterInfo = constructor.GetParameters().SingleOrDefault(x => x.Name.Equals(propertyInfo.Name, StringComparison.OrdinalIgnoreCase));
            if (parameterInfo != null)
            {
                attribute = parameterInfo.GetCustomAttribute<ValidateAttribute>();
            }
        }

        return attribute;
    }
}
