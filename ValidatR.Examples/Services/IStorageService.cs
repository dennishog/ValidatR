using ValidatR.Enums;

namespace ValidatR.Examples.Services;

public interface IStorageService
{
    T GetValidationRuleValue<T>(string name, ValidatorType type, string parameter);
}
