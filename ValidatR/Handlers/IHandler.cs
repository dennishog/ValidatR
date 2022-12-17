namespace ValidatR.Handlers;

public interface IHandler<TModel>
{
    Task HandleAsync(TModel model);
}