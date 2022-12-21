namespace ValidatR.Validators;

using Attributes;
using Enums;

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

        var attribute = propertyInfo.GetCustomAttribute<ValidateAttribute>();

        // If attribute is not found, check constructor parameters if there is a ValidateAttribute declared
        // This is mainly to support records
        foreach (var constructor in typeof(TModel).GetConstructors())
        {
            if (attribute != null)
            {
                continue;
            }

            var parameterInfo = constructor.GetParameters().SingleOrDefault(x => x.Name == propertyInfo.Name);
            if (parameterInfo != null)
            {
                attribute = parameterInfo.GetCustomAttribute<ValidateAttribute>();
            }
        }

        if (attribute == null)
        {
            return;
        }

        if (!attribute.ValidatorType.HasFlag(ValidatorType))
        {
            return;
        }

        await ValidateAsync(attribute, value, GetValueFunc(attribute.Id, ValidatorType, parameter), cancellationToken);
    }

    protected abstract Task ValidateAsync<TProperty>(ValidateAttribute attribute, TProperty value, string validationValue, CancellationToken cancellationToken);
}
