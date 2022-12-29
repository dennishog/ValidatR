namespace ValidatR.Resolvers;

internal interface IValidatorRuleValueResolver<TParameter>
{
    void AddRuleValueFunc(Type validatorRuleType, Delegate ruleValueFunc);
    TValidatorRuleValue GetValidatorRuleValue<TValidatorRuleValue>(Type validatorRuleType, string id, TParameter parameter);
}
