using ValidatR.Exceptions;
using ValidatR.Providers;
using ValidatR.Resolvers;
using ValidatR.Validators;

namespace ValidatR;
internal class Validator<TParameter> : IValidator<TParameter>
{
    private readonly IEnumerable<IValidatorRule<TParameter>> _validators;
    private readonly IEnumerable<IParameterResolver<TParameter>> _parameterResolvers;
    private readonly IPropertyProvider _propertyProvider;

    public Validator(IEnumerable<IValidatorRule<TParameter>> validators, IEnumerable<IParameterResolver<TParameter>> parameterResolvers, IPropertyProvider propertyProvider)
    {
        _validators = validators;
        _parameterResolvers = parameterResolvers;
        _propertyProvider = propertyProvider;
    }

    /// <summary>
    /// Validates the provided model including nested objects
    /// </summary>
    /// <typeparam name="TModel">Type to validate</typeparam>
    /// <param name="model">Instance to validate</param>
    /// <param name="parameter">The parameter sent to the func to retrieve the validator rule values</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ValidatorsNotFoundException">No validators registered</exception>
    /// <exception cref="AggregateException">Contains ValidationExceptions</exception>
    public async Task ValidateAsync<TModel>(TModel model, TParameter parameter, CancellationToken cancellationToken)
        where TModel : class
    {
        if (!_validators.Any())
        {
            throw new ValidatorsNotFoundException();
        }

        var exceptionList = new List<Exception>();

        var validationContexts = await _propertyProvider.GetValidationContextForAllPropertiesAsync(model, cancellationToken);
        foreach (var validationContext in validationContexts)
        {
            foreach (var validator in _validators)
            {
                try
                {
                    await validator.ExecuteHandleAsync(validationContext, parameter, cancellationToken);
                }
                catch (Exception exception)
                {
                    exceptionList.Add(exception);
                }
            }
        }

        if (exceptionList.Count > 0)
        {
            throw new AggregateException(exceptionList.ToArray());
        }
    }

    /// <summary>
    /// Validates the provided model including nested objects. This overload requires that the model type has a registered parameter resolver.
    /// </summary>
    /// <typeparam name="TModel">Type to validate</typeparam>
    /// <param name="model">Instance to validate</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ParameterResolverNotFoundException{TModel, TParameter}">No parameter resolver exists for the type</exception>
    public async Task ValidateAsync<TModel>(TModel model, CancellationToken cancellationToken) where TModel : class
    {
        var parameterResolver = _parameterResolvers.SingleOrDefault(x => x.ShouldHandle(model)) ?? throw new ParameterResolverNotFoundException<TModel, TParameter>(model);
        var parameter = parameterResolver.GetParameterValue(model);

        await ValidateAsync(model, parameter, cancellationToken);
    }
}