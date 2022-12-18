using ValidatR.Enums;

namespace ValidatR.Examples.Services;

public class StorageService : IStorageService
{
    public string GetValidationRuleValue(string name, ValidatorType type, string parameter)
    {
        if (name.Equals("CreateCustomerRequest.FirstName", StringComparison.OrdinalIgnoreCase))
        {
            return type switch
            {
                ValidatorType.Regex => @"\d\d\d\d",
                ValidatorType.MaxLength => "4",
                _ => throw new NotImplementedException()
            };
        }
        else if (name.Equals("CreateCustomerRequest.LastName", StringComparison.OrdinalIgnoreCase))
        {
            return type switch
            {
                ValidatorType.MinLength => "40",
                ValidatorType.Required => "true",
                _ => throw new NotImplementedException()
            };
        }
        else if (name.Equals("CreateOrderRequest.CompanyName", StringComparison.OrdinalIgnoreCase))
        {
            return type switch
            {
                ValidatorType.Regex => ".*",
                ValidatorType.MaxLength => "300",
                _ => throw new NotImplementedException()
            };
        }
        else if (name.Equals("Address.Street", StringComparison.OrdinalIgnoreCase))
        {
            return type switch
            {
                ValidatorType.Regex => @"^\d\d$",
                ValidatorType.MaxLength => "3",
                _ => throw new NotImplementedException()
            };
        }

        throw new NotImplementedException();
    }
}
