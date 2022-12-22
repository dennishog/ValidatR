using ValidatR.Enums;

namespace ValidatR.Attributes;

public class ValidateAttribute : Attribute
{
	public ValidateAttribute(string id, ValidatorType validatorType)
	{
		Id = id;
		ValidatorType = validatorType;
	}

	public string Id { get; }
	public ValidatorType ValidatorType { get; }
}
