using ValidatR.Attributes;

namespace ValidatR.DependencyInjection.Tests.Fakes;

public class RequestModel
{
    public RequestModel(string? stringValue, int intValue, bool boolValue, decimal decimalValue, AnotherRequestModel another, List<AnotherRequestModel> anothers)
    {
        StringValue = stringValue;
        IntValue = intValue;
        BoolValue = boolValue;
        DecimalValue = decimalValue;
        Another = another;
        Anothers = anothers;
    }

    [Validate("StringValue", Enums.ValidatorType.Regex | Enums.ValidatorType.MaxLength)]
    public string? StringValue { get; set; }
    public int IntValue { get; set; }
    public bool BoolValue { get; set; }
    public decimal DecimalValue { get; set; }
    [Validate("Another", Enums.ValidatorType.Required)]
    public AnotherRequestModel Another { get; set; }
    [Validate("Anothers", Enums.ValidatorType.Required)]
    public List<AnotherRequestModel> Anothers { get; set; }
}
