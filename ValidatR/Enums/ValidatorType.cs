namespace ValidatR.Enums;

[Flags]
public enum ValidatorType
{
    Regex = 0,
    MaxLength = 1,
    MinLength = 2,
    Required = 4
}
