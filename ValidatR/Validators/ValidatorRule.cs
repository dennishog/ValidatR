using ValidatR.Enums;

namespace ValidatR.Validators;
public abstract class ValidatorRule<TParameter, TValidationRuleValue> : IValidatorRule<TParameter>
{
    public ValidatorRule(Func<string, TParameter, TValidationRuleValue> getValueFunc)
    {
        GetValueFunc = getValueFunc;
    }

    public Func<string, TParameter, TValidationRuleValue> GetValueFunc { get; init; }
    public abstract ValidatorType ValidatorType { get; }

    public async Task ExecuteHandleAsync<T>(IValidationContext validationContext, T parameter, CancellationToken cancellationToken)
    {
        if (parameter == null)
        {
            throw new ArgumentNullException(nameof(parameter));
        }

        var handleMethod = typeof(IValidatorRule<TParameter>).GetMethod(nameof(IValidatorRule<TParameter>.HandleAsync));
        var genericHandleMethod = handleMethod?.MakeGenericMethod(validationContext.GetModelType(), validationContext.GetValueType());

        var task = (Task?)genericHandleMethod?.Invoke(this, new object[] { validationContext, parameter, cancellationToken }) ?? throw new InvalidOperationException();
        await task;
    }

    public async Task HandleAsync<TModel, TValue>(ValidationContext<TModel, TValue> validationContext, TParameter parameter, CancellationToken cancellationToken)
    {
        if (!validationContext.ValidateAttribute.ValidatorType.HasFlag(ValidatorType))
        {
            return;
        }

        await ValidateAsync(validationContext, GetValueFunc(validationContext.ValidateAttribute.Id, parameter), cancellationToken);
    }

    protected abstract Task ValidateAsync<TModel, TValue>(ValidationContext<TModel, TValue> validationContext, TValidationRuleValue validationValue, CancellationToken cancellationToken);
}
