﻿
using ValidatR.Attributes;
using ValidatR.Enums;
using ValidatR.Exceptions;
using ValidatR.Resolvers;
using ValidatR.Validators;

namespace ValidatR;
public class Validator<TParameter> : IValidator<TParameter>
{
    private List<IValidatorRule<TParameter>> _validators;
    private readonly List<IParameterResolver<TParameter>> _parameterResolvers;

    public Validator()
    {
        _validators = new List<IValidatorRule<TParameter>>();
        _parameterResolvers = new List<IParameterResolver<TParameter>>();
    }

    public void SetValidationRuleValueResolver(Func<string, ValidatorType, TParameter, string> getRuleValidationValue)
    {
        _validators = new List<IValidatorRule<TParameter>>
        {
            new RegexValidatorRule<TParameter>(getRuleValidationValue),
            new MaxLengthValidatorRule<TParameter>(getRuleValidationValue),
            new MinLengthValidatorRule<TParameter>(getRuleValidationValue),
            new RequiredValidatorRule<TParameter>(getRuleValidationValue)
        };
    }

    public void AddParameterResolver(IParameterResolver<TParameter> parameterResolver)
    {
        _parameterResolvers.Add(parameterResolver);
    }

    public async Task ValidateAsync<TModel>(TModel model, TParameter parameter, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            throw new ValidatorsNotFoundException();
        }

        var exceptionList = new List<Exception>();

        await TraversePropertiesAndValidateAsync(typeof(TModel).GetProperties(), exceptionList, model, parameter, cancellationToken);

        if (exceptionList.Count > 0)
        {
            throw new AggregateException(exceptionList.ToArray());
        }
    }

    public async Task ValidateAsync<TModel>(TModel model, CancellationToken cancellationToken)
    {
        var parameterResolver = _parameterResolvers.SingleOrDefault(x => x.ShouldHandle(model)) ?? throw new ParameterResolverNotFoundException<TModel, TParameter>(model);
        var parameter = parameterResolver.GetParameterValue(model);

        await ValidateAsync(model, parameter, cancellationToken);
    }

    public async Task TraversePropertiesAndValidateAsync<TModel>(PropertyInfo[] properties, List<Exception> exceptionList, TModel model, TParameter parameter, CancellationToken cancellationToken)
    {
        foreach (var property in properties)
        {
            if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
            {
                var propertyValue = property.GetValue(model, null);
                var attribute = property.GetCustomAttribute<ValidateAttribute>();

                if (propertyValue != null && attribute != null && attribute.ValidatorType.HasFlag(ValidatorType.Required))
                {
                    await TraversePropertiesAndValidateAsync(propertyValue.GetType().GetProperties(), exceptionList, propertyValue, parameter, cancellationToken);
                }
            }

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
    }
}