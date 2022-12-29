using ValidatR.Attributes;

namespace ValidatR;

internal class ValidationContext<TModel, TValue> : IValidationContext
{
    public ValidationContext(ValidateAttribute validateAttribute, TValue value, TModel owner)
    {
        ValidateAttribute = validateAttribute;
        Value = value;
        Owner = owner;
    }

    public ValidateAttribute ValidateAttribute { get; set; }
    public TValue Value { get; set; }
    public TModel Owner { get; set; }

    public Type GetModelType()
    {
        return typeof(TModel);
    }

    public Type GetValueType()
    {
        return typeof(TValue);
    }
}
