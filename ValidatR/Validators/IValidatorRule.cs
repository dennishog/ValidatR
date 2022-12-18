namespace ValidatR.Validators;

public interface IValidatorRule<TParameter>
{
    Task HandleAsync<TModel>(PropertyInfo propertyInfo, TModel model, TParameter parameter, CancellationToken cancellationToken);
}