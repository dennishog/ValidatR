using ValidatR.Enums;

namespace ValidatR.Examples.Services;

public interface IStorageService
{
    string GetValidationRuleValue(string name, ValidatorType type, string parameter);
}
