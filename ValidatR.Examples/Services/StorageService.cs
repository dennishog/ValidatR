using ValidatR.Enums;

namespace ValidatR.Examples.Services;

public class StorageService : IStorageService
{
    public T GetValidationRuleValue<T>(string name, ValidatorType type, string parameter)
    {
        if (name.Equals("CreateCustomerRequest.FirstName", StringComparison.OrdinalIgnoreCase))
        {
            return type switch
            {
                ValidatorType.Regex => (T)Convert.ChangeType(@"\d\d\d\d", typeof(T)),
                ValidatorType.MaxLength => (T)Convert.ChangeType(4, typeof(T)),
                _ => throw new NotImplementedException()
            };
        }
        else if (name.Equals("CreateCustomerRequest.LastName", StringComparison.OrdinalIgnoreCase))
        {
            return type switch
            {
                ValidatorType.MinLength => (T)Convert.ChangeType(40, typeof(T)),
                ValidatorType.Required => (T)Convert.ChangeType(true, typeof(T)),
                _ => throw new NotImplementedException()
            };
        }
        else if (name.Equals("CreateOrderRequest.CompanyName", StringComparison.OrdinalIgnoreCase))
        {
            return type switch
            {
                ValidatorType.Regex => (T)Convert.ChangeType(@"\d.*", typeof(T)),
                ValidatorType.MaxLength => (T)Convert.ChangeType(10, typeof(T)),
                _ => throw new NotImplementedException()
            };
        }
        else if (name.Equals("Address.Street", StringComparison.OrdinalIgnoreCase))
        {
            return type switch
            {
                ValidatorType.Regex => (T)Convert.ChangeType(@"^\d\d$", typeof(T)),
                ValidatorType.MaxLength => (T)Convert.ChangeType(3, typeof(T)),
                _ => throw new NotImplementedException()
            };
        }
        else if (name.Equals("Address", StringComparison.OrdinalIgnoreCase))
        {
            return (T)Convert.ChangeType(true, typeof(T));
        }
        else if (name.Equals("CreateItemRequest.Name"))
        {
            return type switch
            {
                ValidatorType.Regex => (T)Convert.ChangeType(@"\d.*", typeof(T)),
                ValidatorType.MaxLength => (T)Convert.ChangeType(434, typeof(T)),
                _ => throw new NotImplementedException()
            };
        }
        else if (name.Equals("CreateItemRequest.Pictures"))
        {
            return (T)Convert.ChangeType(true, typeof(T));
        }
        else if (name.Equals("ItemPicture.UriLarge"))
        {
            return (T)Convert.ChangeType(50, typeof(T));
        }

        throw new NotImplementedException();
    }
}
