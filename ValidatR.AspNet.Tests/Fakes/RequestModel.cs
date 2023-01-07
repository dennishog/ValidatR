using System.Diagnostics.CodeAnalysis;
using ValidatR.Attributes;

namespace ValidatR.AspNet.Tests.Fakes;

public class RequestModel : IEqualityComparer<RequestModel>
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

    public override bool Equals(object? obj)
    {
        if (obj is RequestModel model)
        {
            return Equals(this, model);
        }

        return false;
    }

    public bool Equals(RequestModel? x, RequestModel? y)
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

        if (!x.Another.Equals(y.Another))
        {
            return false;
        }

        if (x.Anothers != null && y.Anothers != null && x.Anothers.Count != y.Anothers.Count)
        {
            return false;
        }

        if (x.Anothers != null && y.Anothers != null)
        {
            for (var i = 0; i < x.Anothers.Count; i++)
            {
                if (!x.Anothers[i].Equals(y.Anothers[i]))
                {
                    return false;
                }
            }
        }

        return true;
    }

    public int GetHashCode([DisallowNull] RequestModel obj)
    {
        throw new NotImplementedException();
    }
}
