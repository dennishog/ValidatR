namespace ValidatR.Exceptions;

public class ValidatorsNotFoundException : Exception
{
    public ValidatorsNotFoundException() : base("No validators found. Call SetValidationRuleValueResolver to set func for resolving validation rule values.")
    {
    }
}
