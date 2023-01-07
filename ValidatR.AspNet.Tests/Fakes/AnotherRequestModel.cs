using System.Diagnostics.CodeAnalysis;
using ValidatR.Attributes;

namespace ValidatR.AspNet.Tests.Fakes;

public class AnotherRequestModel : IEqualityComparer<AnotherRequestModel>
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

    public override bool Equals(object? obj)
    {
        if (obj is AnotherRequestModel model)
        {
            return Equals(this, model);
        }

        return false;
    }

    public bool Equals(AnotherRequestModel? x, AnotherRequestModel? y)
    {
        if (ReferenceEquals(x, y))
        {
            return true;
        }

        if (x is null || y is null)
        {
            return false;
        }

        if (x.StringValue != y.StringValue)
        {
            return false;
        }

        if (x.IntValue != y.IntValue)
        {
            return false;
        }

        if (x.BoolValue != y.BoolValue)
        {
            return false;
        }

        if (x.DecimalValue != y.DecimalValue)
        {
            return false;
        }

        return true;
    }

    public int GetHashCode([DisallowNull] AnotherRequestModel obj)
    {
        throw new NotImplementedException();
    }
}
