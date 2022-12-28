﻿using ValidatR.Attributes;

namespace ValidatR;

public class ValidationContext<TModel, TValue>
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
}
