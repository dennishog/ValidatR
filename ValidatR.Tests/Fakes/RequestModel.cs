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

    public string StringValue { get; set; }
    public int IntValue { get; set; }
    public bool BoolValue { get; set; }
    public decimal DecimalValue { get; set; }
}
