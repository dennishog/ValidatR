﻿
using ValidatR.Attributes;
using ValidatR.Enums;
using ValidatR.Exceptions;

namespace ValidatR.Validators;
public class MaxLengthValidatorRule<TParameter> : ValidatorRule<TParameter>
{
    public MaxLengthValidatorRule(Func<string, ValidatorType, TParameter, string> getValueFunc) : base(getValueFunc)
    {
    }

    public override ValidatorType ValidatorType => ValidatorType.MaxLength;

    protected override async Task ValidateAsync<TProperty>(ValidateAttribute attribute, TProperty value, string defaultValue, CancellationToken cancellationToken)
    {
        var valueString = Convert.ToString(value);

        if (valueString != null && valueString.Length > int.Parse(defaultValue))
        {

            throw new ValidationException(attribute, $"Value '{value}' should have a maximum length of '{defaultValue}'");
        }

        await Task.CompletedTask;
    }
}
