namespace ValidatR.Providers;

internal interface IPropertyProvider
{
    Task<List<IValidationContext>> GetValidationContextForAllPropertiesAsync<TModel>(TModel model, CancellationToken cancellationToken)
        where TModel : class;
}
