using ValidatR.Attributes;

namespace ValidatR.Tests.Fakes;

public class RequestModel
{
    public RequestModel(string stringValue, int intValue, bool boolValue, decimal decimalValue)
    {
        StringValue = stringValue;
        IntValue = intValue;
        BoolValue = boolValue;
        DecimalValue = decimalValue;
    }

    [Validate(Enums.ValidatorType.Regex | Enums.ValidatorType.MaxLength, "StringValue")]
    public string StringValue { get; set; }
    public int IntValue { get; set; }
    public bool BoolValue { get; set; }
    public decimal DecimalValue { get; set; }
}
