namespace ValidatR;

using Enums;
using Resolvers;
using Validators;
using ValidatR.Exceptions;

public class Validator<TParameter> : IValidator<TParameter>
{
    private List<IValidatorRule<TParameter>> _validators;
    private readonly List<IParameterResolver<TParameter>> _parameterResolvers;

    public Validator(Func<string, ValidatorType, TParameter, string>? getValidatorValueFunc = null)
    {
        if (getValidatorValueFunc != null)
        {
            _validators = new List<IValidatorRule<TParameter>>
            {
               new RegexValidatorRule<TParameter>(getValidatorValueFunc),
             new MaxLengthValidatorRule<TParameter>(getValidatorValueFunc)
            };
        }
        _validators = new List<IValidatorRule<TParameter>>();
        _parameterResolvers = new List<IParameterResolver<TParameter>>();
    }

    public void SetValidationRuleValueResolver(Func<string, ValidatorType, TParameter, string> getRuleValidationValue)
    {
        _validators = new List<IValidatorRule<TParameter>>
        {
            new RegexValidatorRule<TParameter>(getRuleValidationValue),
            new MaxLengthValidatorRule<TParameter>(getRuleValidationValue)
        };
    }

    public void AddParameterResolver(IParameterResolver<TParameter> parameterResolver)
    {
        _parameterResolvers.Add(parameterResolver);
    }

    public async Task ValidateAsync<TModel>(TModel model, TParameter parameter, CancellationToken cancellationToken)
    {
        var exceptionList = new List<Exception>();

        await TraverseProperties(typeof(TModel).GetProperties(), exceptionList, model, parameter, cancellationToken);
        //foreach (var property in typeof(TModel).GetProperties())
        //{
        //    if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
        //    {

        //    }

        //    foreach (var validator in _validators)
        //    {
        //        try
        //        {
        //            await validator.HandleAsync(property, model, parameter, cancellationToken);
        //        }
        //        catch (Exception e)
        //        {
        //            exceptionList.Add(e);
        //        }
        //    }
        //}

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

    public async Task TraverseProperties<TModel>(PropertyInfo[] properties, List<Exception> exceptionList, TModel model, TParameter parameter, CancellationToken cancellationToken)
    {
        foreach (var property in properties)
        {
            if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
            {
                if (model == null)
                {
                    return;
                }

                Type type = model.GetType();
                PropertyInfo info = type.GetProperty(property.Name);

                if (info == null)
                {
                    return;
                }
                var v = info.GetValue(model, null);

                if (v != null)
                {
                    await TraverseProperties(v.GetType().GetProperties(), exceptionList, v, parameter, cancellationToken);
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