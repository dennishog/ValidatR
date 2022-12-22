﻿
using ValidatR.Attributes;
using ValidatR.Enums;
using ValidatR.Exceptions;

namespace ValidatR.Validators;
public class RequiredValidatorRule<TParameter> : ValidatorRule<TParameter>
{
    public RequiredValidatorRule(Func<string, ValidatorType, TParameter, string> getValueFunc) : base(getValueFunc)
    {
    }

    public override ValidatorType ValidatorType => ValidatorType.Required;

    protected override async Task ValidateAsync<TProperty>(ValidateAttribute attribute, TProperty value, string defaultValue, CancellationToken cancellationToken)
    {
        var valueString = Convert.ToString(value);

        var ruleValue = bool.Parse(defaultValue);

        if (string.IsNullOrWhiteSpace(valueString) && ruleValue)
        {
            throw new ValidationException(attribute, $"Value is required");
        }

        await Task.CompletedTask;
    }
}
