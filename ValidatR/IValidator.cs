namespace ValidatR;

public interface IValidator<TParameter>
{
    Task ValidateAsync<TModel>(TModel model, TParameter parameter, CancellationToken cancellationToken);
}
