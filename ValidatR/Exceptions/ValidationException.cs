using ValidatR.Attributes;

namespace ValidatR.Exceptions;
public class ValidationException : Exception
{
    public ValidationException(ValidateAttribute attribute, string errorMessage) : base(errorMessage)
    {
        Attribute = attribute;
        ErrorMessage = errorMessage;
    }

    public ValidateAttribute Attribute { get; }
    public string ErrorMessage { get; set; }
}
