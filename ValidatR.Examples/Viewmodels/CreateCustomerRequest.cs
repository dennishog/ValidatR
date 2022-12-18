using ValidatR.Attributes;
using ValidatR.Enums;

namespace ValidatR.Examples.Viewmodels;

public class CreateCustomerRequest
{
    public CreateCustomerRequest(string firstName, string lastName, int age, bool awesome)
    {
        FirstName = firstName;
        LastName = lastName;
        Age = age;
        Awesome = awesome;
    }

    [Validate(ValidatorType.Regex | ValidatorType.MaxLength, "CreateCustomerRequest.FirstName")]
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public int Age { get; set; }

    public bool Awesome { get; set; }
}
