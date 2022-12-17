namespace ValidatR.Handlers;

public abstract class Handler<TModel> : IHandler<TModel>
{
    public Task HandleAsync(TModel model)
    {
        throw new NotImplementedException();
    }
}
