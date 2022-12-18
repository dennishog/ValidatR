namespace ValidatR.Enums;

[Flags]
public enum ValidatorType
{
    Regex = 1,
    MaxLength = 2,
    MinLength = 4,
    Required = 8
}
