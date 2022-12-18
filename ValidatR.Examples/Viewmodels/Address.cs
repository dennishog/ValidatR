using ValidatR.Attributes;

namespace ValidatR.Examples.Viewmodels;

public class Address
{
    public Address(string street)
    {
        Street = street;
    }

    [Validate(Enums.ValidatorType.Regex | Enums.ValidatorType.MaxLength, "Address.Street")]
    public string Street { get; set; }
}
