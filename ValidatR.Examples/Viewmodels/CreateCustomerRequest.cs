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

    [Validate("CreateCustomerRequest.FirstName", ValidatorType.Regex | ValidatorType.MaxLength)]
    public string FirstName { get; set; }

    [Validate("CreateCustomerRequest.LastName", ValidatorType.Required | ValidatorType.MinLength)]
    public string? LastName { get; set; }

    [Validate("Address", ValidatorType.Required)]
    public Address Address { get; set; }

    public int Age { get; set; }

    public bool Awesome { get; set; }
}
