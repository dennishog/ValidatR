namespace ValidatR;
public interface IValidator
{
    Task ValidateAsync<TModel>(TModel model, CancellationToken cancellationToken) where TModel : class;
}

public interface IValidator<TParameter> : IValidator
{
    Task ValidateAsync<TModel>(TModel model, TParameter parameter, CancellationToken cancellationToken) where TModel : class;
}
