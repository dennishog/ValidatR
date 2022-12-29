namespace ValidatR.Providers;

public interface IPropertyProvider
{
    Task<List<object>> GetValidationContextForAllPropertiesAsync<TModel>(TModel model, CancellationToken cancellationToken);
}
