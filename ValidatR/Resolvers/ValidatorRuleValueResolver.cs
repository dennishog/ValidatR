using ValidatR.Exceptions;

namespace ValidatR.Resolvers;

internal class ValidatorRuleValueResolver<TParameter> : IValidatorRuleValueResolver<TParameter>
{
    private readonly Dictionary<Type, Delegate> _validatorRuleValueFuncs;

    public ValidatorRuleValueResolver()
    {
        _validatorRuleValueFuncs = new Dictionary<Type, Delegate>();
    }

    public void AddRuleValueFunc(Type validatorRuleType, Delegate ruleValueFunc)
    {
        _validatorRuleValueFuncs.Add(validatorRuleType, ruleValueFunc);
    }

    public TValidatorRuleValue GetValidatorRuleValue<TValidatorRuleValue>(Type validatorRuleType, string id, TParameter parameter)
    {
        parameter = parameter ?? throw new ArgumentNullException(nameof(parameter));

        var func = _validatorRuleValueFuncs.ContainsKey(validatorRuleType)
            ? _validatorRuleValueFuncs[validatorRuleType]
            : throw new ValidatorRuleValueResolverNotFoundException(validatorRuleType);

        var validatorRuleValue = func.DynamicInvoke(id, parameter);

        return (TValidatorRuleValue?)validatorRuleValue ?? throw new ArgumentNullException(nameof(validatorRuleValue));
    }
}
