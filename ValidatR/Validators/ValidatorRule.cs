using ValidatR.Enums;
using ValidatR.Resolvers;

namespace ValidatR.Validators;
internal abstract class ValidatorRule<TParameter, TValidationRuleValue> : IValidatorRule<TParameter>
{
    private readonly IValidatorRuleValueResolver<TParameter> _ruleValueResolver;

    public ValidatorRule(IValidatorRuleValueResolver<TParameter> ruleValueResolver)
    {
        _ruleValueResolver = ruleValueResolver;
    }

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

        var ruleValue = _ruleValueResolver.GetValidatorRuleValue<TValidationRuleValue>(GetType(), validationContext.ValidateAttribute.Id, parameter);

        await ValidateAsync(validationContext, ruleValue, cancellationToken);
    }

    protected abstract Task ValidateAsync<TModel, TValue>(ValidationContext<TModel, TValue> validationContext, TValidationRuleValue validationValue, CancellationToken cancellationToken);
}
