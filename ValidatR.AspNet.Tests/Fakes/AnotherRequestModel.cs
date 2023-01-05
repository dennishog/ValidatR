using ValidatR.Attributes;

namespace ValidatR.AspNet.Tests.Fakes;

public class AnotherRequestModel
{
    public AnotherRequestModel(string stringValue, int intValue, bool boolValue, decimal decimalValue)
    {
        StringValue = stringValue;
        IntValue = intValue;
        BoolValue = boolValue;
        DecimalValue = decimalValue;
    }

    [Validate("StringValue", Enums.ValidatorType.Regex | Enums.ValidatorType.MaxLength)]
    public string StringValue { get; set; }
    public int IntValue { get; set; }
    public bool BoolValue { get; set; }
    public decimal DecimalValue { get; set; }
}
