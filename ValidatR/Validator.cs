using ValidatR.Exceptions;
using ValidatR.Providers;
using ValidatR.Resolvers;
using ValidatR.Validators;

namespace ValidatR;
public class Validator<TParameter> : IValidator<TParameter>
{
    private readonly List<IValidatorRule<TParameter>> _validators;
    private readonly List<IParameterResolver<TParameter>> _parameterResolvers;
    private readonly PropertyProvider _propertyProvider;

    public Validator()
    {
        _validators = new List<IValidatorRule<TParameter>>();
        _parameterResolvers = new List<IParameterResolver<TParameter>>();
        _propertyProvider = new PropertyProvider();
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

    public async Task ValidateAsync<TModel>(TModel model, CancellationToken cancellationToken) where TModel : class
    {
        var parameterResolver = _parameterResolvers.SingleOrDefault(x => x.ShouldHandle(model)) ?? throw new ParameterResolverNotFoundException<TModel, TParameter>(model);
        var parameter = parameterResolver.GetParameterValue(model);

        await ValidateAsync(model, parameter, cancellationToken);
    }
}