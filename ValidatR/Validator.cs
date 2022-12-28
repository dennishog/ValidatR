﻿using System.Collections;
using ValidatR.Attributes;
using ValidatR.Enums;
using ValidatR.Exceptions;
using ValidatR.Factories;
using ValidatR.Resolvers;
using ValidatR.Validators;

namespace ValidatR;
public class Validator<TParameter> : IValidator<TParameter>
{
    private readonly List<IValidatorRule<TParameter>> _validators;
    private readonly List<IParameterResolver<TParameter>> _parameterResolvers;

    public Validator()
    {
        _validators = new List<IValidatorRule<TParameter>>();
        _parameterResolvers = new List<IParameterResolver<TParameter>>();
    }

    public void AddValidator(IValidatorRule<TParameter> validator)
    {
        _validators.Add(validator);
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
            var propertyValue = property.GetValue(model, null);
            var attribute = property.GetCustomAttribute<ValidateAttribute>();

            if (propertyValue == null || attribute == null)
            {
                continue;
            }

            var validationContext = ValidationContextFactory.Create(property, attribute, propertyValue, model);

            foreach (var validator in _validators)
            {
                try
                {
                    await ExecuteHandleAsync<TModel>(validator, validationContext, property, parameter, cancellationToken);
                }
                catch (Exception e)
                {
                    exceptionList.Add(e);
                }
            }

            if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
            {
                if (attribute.ValidatorType.HasFlag(ValidatorType.Required))
                {
                    if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                    {
                        if (propertyValue is IEnumerable collection)
                        {
                            foreach (var item in collection)
                            {
                                await TraversePropertiesAndValidateAsync(item.GetType().GetProperties(), exceptionList, item, parameter, cancellationToken);
                            }
                        }
                    }
                    else
                    {
                        await TraversePropertiesAndValidateAsync(propertyValue.GetType().GetProperties(), exceptionList, propertyValue, parameter, cancellationToken);
                    }
                }
            }
        }
    }

    private async Task ExecuteHandleAsync<TModel>(IValidatorRule<TParameter> validator, object validationContext, PropertyInfo property, TParameter parameter, CancellationToken cancellationToken)
    {
        var handleMethod = typeof(IValidatorRule<TParameter>).GetMethod(nameof(IValidatorRule<TParameter>.HandleAsync));
        var genericHandleMethod = handleMethod?.MakeGenericMethod(typeof(TModel), property.PropertyType);

        var task = (Task?)genericHandleMethod?.Invoke(validator, new[] { validationContext, parameter, cancellationToken }) ?? throw new InvalidOperationException();
        await task;
    }
}