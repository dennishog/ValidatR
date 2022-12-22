using ValidatR.Attributes;

namespace ValidatR.Examples.Viewmodels;

public class Address
{
    public Address(string street)
    {
        Street = street;
    }

    [Validate("Address.Street", Enums.ValidatorType.Regex | Enums.ValidatorType.MaxLength)]
    public string Street { get; set; }
}
