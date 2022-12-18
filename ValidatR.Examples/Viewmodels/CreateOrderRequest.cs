using ValidatR.Attributes;

namespace ValidatR.Examples.Viewmodels;

public class CreateOrderRequest
{
    public CreateOrderRequest(string companyName, int quantity)
    {
        CompanyName = companyName;
        Quantity = quantity;
    }

    [Validate(Enums.ValidatorType.Regex | Enums.ValidatorType.MaxLength, "CreateOrderRequest.CompanyName")]
    public string CompanyName { get; set; }
    public int Quantity { get; set; }
}
