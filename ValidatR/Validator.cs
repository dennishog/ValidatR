﻿using ValidatR.Enums;
using ValidatR.Validators;

namespace ValidatR;

public class Validator<TParameter> : IValidator<TParameter>
{
    private readonly List<IValidatorRule<TParameter>> _validators;

    public Validator(Func<string, ValidatorType, TParameter, string> getValidatorValueFunc)
    {
        _validators = new List<IValidatorRule<TParameter>>
        {
            new RegexValidatorRule<TParameter>(getValidatorValueFunc),
            new MaxLengthValidatorRule<TParameter>(getValidatorValueFunc)
        };
    }

    public async Task ValidateAsync<TModel>(TModel model, TParameter parameter, CancellationToken cancellationToken)
    {
        var exceptionList = new List<Exception>();

        foreach (var property in typeof(TModel).GetProperties())
        {
            foreach (var validator in _validators)
            {
                try
                {
                    await validator.HandleAsync(property, model, parameter, cancellationToken);
                }
                catch (Exception e)
                {
                    exceptionList.Add(e);
                }
            }
        }

        if (exceptionList.Count > 0)
        {
            throw new AggregateException(exceptionList.ToArray());
        }
    }
}