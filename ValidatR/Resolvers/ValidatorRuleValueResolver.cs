namespace ValidatR.Resolvers;

public class ValidatorRuleValueResolver<TParameter> : IValidatorRuleValueResolver<TParameter>
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
        var func = _validatorRuleValueFuncs[validatorRuleType];

        return (TValidatorRuleValue?)func.DynamicInvoke(id, parameter) ?? throw new Exception("lkjlkjfd");
    }
}
