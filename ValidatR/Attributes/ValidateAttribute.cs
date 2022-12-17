using ValidatR.Enums;

namespace ValidatR.Attributes;

public class ValidateAttribute : Attribute
{
	public ValidateAttribute(ValidatorType validatorType, string id)
	{
		ValidatorType = validatorType;
		Id = id;
	}

	public ValidatorType ValidatorType { get; }
	public string Id { get; }
}
