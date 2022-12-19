using ValidatR.Attributes;
using ValidatR.Enums;

namespace ValidatR.Examples.Viewmodels;

public class CreateCustomerRequest
{
    public CreateCustomerRequest(string firstName, string lastName, Address address, int age, bool awesome)
    {
        FirstName = firstName;
        LastName = lastName;
        Address = address;
        Age = age;
        Awesome = awesome;
    }

    [Validate(ValidatorType.Regex | ValidatorType.MaxLength, "CreateCustomerRequest.FirstName")]
    public string FirstName { get; set; }

    [Validate(ValidatorType.Required | ValidatorType.MinLength, "CreateCustomerRequest.LastName")]
    public string? LastName { get; set; }

    [Validate(ValidatorType.Required, "Address")]
    public Address Address { get; set; }

    public int Age { get; set; }

    public bool Awesome { get; set; }
}
