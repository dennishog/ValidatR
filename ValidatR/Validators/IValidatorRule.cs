namespace ValidatR.Validators;

using System.Reflection;

public interface IValidatorRule<TParameter>
{
    Task HandleAsync<TModel>(PropertyInfo propertyInfo, TModel model, TParameter parameter, CancellationToken cancellationToken);
}