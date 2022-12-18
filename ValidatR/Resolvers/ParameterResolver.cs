using System.Linq.Expressions;

namespace ValidatR.Resolvers;

public class ParameterResolver<TModel, TParameter> : IParameterResolver<TParameter>
{
    private readonly Func<TModel, TParameter> _parameterSelector;

    public ParameterResolver(Expression<Func<TModel, TParameter>> parameterSelector)
    {
        _parameterSelector = parameterSelector.Compile();
    }

    public bool ShouldHandle<T>(T model)
    {
        if (typeof(T).Equals(typeof(TModel)))
        {
            return true;
        }

        return false;
    }

    public TParameter GetParameterValue<T>(T model)
    {
        if (!typeof(T).Equals(typeof(TModel)))
        {
            throw new InvalidOperationException();
        }

        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        var convertedModel = (TModel)Convert.ChangeType(model, typeof(TModel));

        var value = _parameterSelector(convertedModel);
        if (value == null)
        {
            throw new ArgumentException("No value for parameter");
        }

        return value;
    }
}
