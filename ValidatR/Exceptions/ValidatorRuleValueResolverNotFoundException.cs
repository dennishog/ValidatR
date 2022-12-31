namespace ValidatR.Exceptions;

public class ValidatorRuleValueResolverNotFoundException : Exception
{
    public ValidatorRuleValueResolverNotFoundException(Type type) : base($"Could not resolve validator rule value for type {type}")
    {
    }
}
